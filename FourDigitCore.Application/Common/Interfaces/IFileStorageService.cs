using FourDigitCore.Application.DTOs.Admin;

namespace FourDigitCore.Application.Common.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType, string folder = "uploads", CancellationToken cancellationToken = default);
    Task<UploadMediaResultDto> UploadImageAsync(Stream fileStream, string fileName, string folder = "fourdigitcore", CancellationToken cancellationToken = default);
    Task<UploadMediaResultDto> UploadRawFileAsync(Stream fileStream, string fileName, string folder = "fourdigitcore/docs", CancellationToken cancellationToken = default);
    Task<bool> DeleteFileAsync(string fileUrlOrPublicId, CancellationToken cancellationToken = default);
}
