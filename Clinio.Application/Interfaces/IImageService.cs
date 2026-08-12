using Clinio.Application.Common;

namespace Clinio.Application.Interfaces;

public interface IImageService
{
    Task<Result<string>> UploadUserImageAsync(ImageUploadRequest image, int userId);
    Task<Result<string>> UploadClinicImageAsync(ImageUploadRequest image, int clinicId);
    Task<Result<bool>> DeleteImageAsync(string fileName);
}