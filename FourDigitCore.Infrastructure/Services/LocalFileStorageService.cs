using FourDigitCore.Application.Common.Interfaces;
using FourDigitCore.Application.DTOs.Admin;

namespace FourDigitCore.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _baseStoragePath;

    public LocalFileStorageService()
    {
        _baseStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType, string folder = "uploads", CancellationToken cancellationToken = default)
    {
        var targetFolder = Path.Combine(_baseStoragePath, folder);
        if (!Directory.Exists(targetFolder))
        {
            Directory.CreateDirectory(targetFolder);
        }

        var extension = Path.GetExtension(fileName);
        var safeFileName = $"{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(targetFolder, safeFileName);

        using (var destStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await fileStream.CopyToAsync(destStream, cancellationToken);
        }

        var relativeUrl = $"/{folder.Replace('\\', '/')}/{safeFileName}";
        return relativeUrl;
    }

    public async Task<UploadMediaResultDto> UploadImageAsync(Stream fileStream, string fileName, string folder = "fourdigitcore", CancellationToken cancellationToken = default)
    {
        try
        {
            var relativeUrl = await SaveFileAsync(fileStream, fileName, "image", folder, cancellationToken);
            return new UploadMediaResultDto
            {
                Success = true,
                Url = relativeUrl,
                PublicId = Path.GetFileName(relativeUrl),
                Format = Path.GetExtension(fileName).TrimStart('.'),
                Message = "File saved to local server storage."
            };
        }
        catch (Exception ex)
        {
            return new UploadMediaResultDto { Success = false, Message = ex.Message };
        }
    }

    public async Task<UploadMediaResultDto> UploadRawFileAsync(Stream fileStream, string fileName, string folder = "fourdigitcore/docs", CancellationToken cancellationToken = default)
    {
        try
        {
            var relativeUrl = await SaveFileAsync(fileStream, fileName, "application/octet-stream", folder, cancellationToken);
            return new UploadMediaResultDto
            {
                Success = true,
                Url = relativeUrl,
                PublicId = Path.GetFileName(relativeUrl),
                Format = Path.GetExtension(fileName).TrimStart('.'),
                Message = "Raw document saved to local server storage."
            };
        }
        catch (Exception ex)
        {
            return new UploadMediaResultDto { Success = false, Message = ex.Message };
        }
    }

    public Task<bool> DeleteFileAsync(string fileUrlOrPublicId, CancellationToken cancellationToken = default)
    {
        try
        {
            var relativePath = fileUrlOrPublicId.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(_baseStoragePath, relativePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
}
