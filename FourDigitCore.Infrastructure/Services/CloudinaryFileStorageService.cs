using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FourDigitCore.Application.Common.Interfaces;
using FourDigitCore.Application.DTOs.Admin;

namespace FourDigitCore.Infrastructure.Services;

public class CloudinaryFileStorageService : IFileStorageService
{
    private readonly Cloudinary? _cloudinary;
    private readonly ILogger<CloudinaryFileStorageService> _logger;
    private readonly bool _isConfigured;

    public CloudinaryFileStorageService(IConfiguration configuration, ILogger<CloudinaryFileStorageService> logger)
    {
        _logger = logger;

        var cloudinaryUrl = Environment.GetEnvironmentVariable("CLOUDINARY_URL") ?? configuration["CLOUDINARY_URL"];
        if (!string.IsNullOrWhiteSpace(cloudinaryUrl))
        {
            try
            {
                _cloudinary = new Cloudinary(cloudinaryUrl);
                _cloudinary.Api.Secure = true;
                _isConfigured = true;
                _logger.LogInformation("Cloudinary initialized via CLOUDINARY_URL.");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to initialize Cloudinary from CLOUDINARY_URL. Falling back to local storage.");
            }
        }

        var cloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME") ?? configuration["Cloudinary:CloudName"];
        var apiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY") ?? configuration["Cloudinary:ApiKey"];
        var apiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET") ?? configuration["Cloudinary:ApiSecret"];

        if (!string.IsNullOrWhiteSpace(cloudName) && !string.IsNullOrWhiteSpace(apiKey) && !string.IsNullOrWhiteSpace(apiSecret))
        {
            try
            {
                var account = new Account(cloudName, apiKey, apiSecret);
                _cloudinary = new Cloudinary(account);
                _cloudinary.Api.Secure = true;
                _isConfigured = true;
                _logger.LogInformation("Cloudinary initialized with cloud name '{CloudName}'.", cloudName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to initialize Cloudinary with credentials. Falling back to local storage.");
            }
        }
        else
        {
            _isConfigured = false;
            _logger.LogInformation("Cloudinary credentials not provided. Using local filesystem storage in wwwroot/uploads.");
        }
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType, string folder = "uploads", CancellationToken cancellationToken = default)
    {
        var ext = Path.GetExtension(fileName)?.ToLowerInvariant() ?? string.Empty;
        var isImage = ext is ".jpg" or ".jpeg" or ".png" or ".webp" or ".gif" or ".svg";

        if (isImage)
        {
            var result = await UploadImageAsync(fileStream, fileName, folder, cancellationToken);
            return result.Success ? result.Url : string.Empty;
        }
        else
        {
            var result = await UploadRawFileAsync(fileStream, fileName, folder, cancellationToken);
            return result.Success ? result.Url : string.Empty;
        }
    }

    public async Task<UploadMediaResultDto> UploadImageAsync(Stream fileStream, string fileName, string folder = "fourdigitcore", CancellationToken cancellationToken = default)
    {
        if (_isConfigured && _cloudinary != null)
        {
            try
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName, fileStream),
                    Folder = folder,
                    UseFilename = true,
                    UniqueFilename = true,
                    Overwrite = false
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

                if (uploadResult.Error != null)
                {
                    _logger.LogError("Cloudinary image upload error: {Error}", uploadResult.Error.Message);
                    return new UploadMediaResultDto { Success = false, Message = uploadResult.Error.Message };
                }

                return new UploadMediaResultDto
                {
                    Success = true,
                    Url = uploadResult.SecureUrl?.ToString() ?? uploadResult.Url?.ToString() ?? string.Empty,
                    PublicId = uploadResult.PublicId,
                    Format = uploadResult.Format,
                    Bytes = uploadResult.Bytes,
                    Message = "Image uploaded successfully to Cloudinary CDN."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during Cloudinary image upload. Falling back to local storage.");
            }
        }

        return await SaveToLocalAsync(fileStream, fileName, folder, cancellationToken);
    }

    public async Task<UploadMediaResultDto> UploadRawFileAsync(Stream fileStream, string fileName, string folder = "fourdigitcore/docs", CancellationToken cancellationToken = default)
    {
        if (_isConfigured && _cloudinary != null)
        {
            try
            {
                var extension = Path.GetExtension(fileName)?.ToLowerInvariant() ?? ".pdf";
                var rawBaseName = Path.GetFileNameWithoutExtension(fileName);
                var cleanBaseName = System.Text.RegularExpressions.Regex.Replace(rawBaseName, @"[^a-zA-Z0-9_\-]", "_").Trim('_');
                if (string.IsNullOrWhiteSpace(cleanBaseName)) cleanBaseName = "doc";

                var uniqueSuffix = Guid.NewGuid().ToString("N")[..8];
                var cleanFolder = folder.Trim().Trim('/');
                var publicId = $"{cleanFolder}/{cleanBaseName}_{uniqueSuffix}{extension}";

                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(fileName, fileStream),
                    PublicId = publicId,
                    Overwrite = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams, "raw", cancellationToken);

                if (uploadResult.Error != null)
                {
                    _logger.LogError("Cloudinary raw document upload error: {Error}", uploadResult.Error.Message);
                    return new UploadMediaResultDto { Success = false, Message = uploadResult.Error.Message };
                }

                var finalUrl = uploadResult.SecureUrl?.ToString() ?? uploadResult.Url?.ToString() ?? string.Empty;

                return new UploadMediaResultDto
                {
                    Success = true,
                    Url = finalUrl,
                    PublicId = uploadResult.PublicId,
                    Format = extension.TrimStart('.'),
                    Bytes = uploadResult.Bytes,
                    Message = "Document uploaded successfully to Cloudinary CDN."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during Cloudinary raw file upload. Falling back to local storage.");
            }
        }

        return await SaveToLocalAsync(fileStream, fileName, folder, cancellationToken);
    }

    public async Task<bool> DeleteFileAsync(string fileUrlOrPublicId, CancellationToken cancellationToken = default)
    {
        if (_isConfigured && _cloudinary != null && !fileUrlOrPublicId.StartsWith("/uploads"))
        {
            try
            {
                var deleteParams = new DeletionParams(fileUrlOrPublicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);
                return result.Result == "ok";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete Cloudinary media with publicId '{PublicId}'", fileUrlOrPublicId);
                return false;
            }
        }

        try
        {
            var relativePath = fileUrlOrPublicId.TrimStart('/');
            var localPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);
            if (File.Exists(localPath))
            {
                File.Delete(localPath);
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete local file at path '{Path}'", fileUrlOrPublicId);
            return false;
        }
    }

    private async Task<UploadMediaResultDto> SaveToLocalAsync(Stream fileStream, string fileName, string folder, CancellationToken cancellationToken)
    {
        try
        {
            var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var targetDir = Path.Combine(webRoot, "uploads", folder.Replace('/', Path.DirectorySeparatorChar));

            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            var extension = Path.GetExtension(fileName);
            var safeName = $"{Path.GetFileNameWithoutExtension(fileName)}_{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(targetDir, safeName);

            using (var fileDestination = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await fileStream.CopyToAsync(fileDestination, cancellationToken);
            }

            var relativeUrl = $"/uploads/{folder.Trim('/')}/{safeName}";
            var fileInfo = new FileInfo(filePath);

            return new UploadMediaResultDto
            {
                Success = true,
                Url = relativeUrl,
                PublicId = safeName,
                Format = extension.TrimStart('.'),
                Bytes = fileInfo.Length,
                Message = "File saved to server local storage."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save file to local directory.");
            return new UploadMediaResultDto { Success = false, Message = ex.Message };
        }
    }
}
