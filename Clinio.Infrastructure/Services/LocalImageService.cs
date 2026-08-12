using Clinio.Application.Common;
using Clinio.Application.Common.Errors;
using Clinio.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Clinio.Infrastructure.Services;

public class LocalImageService(
    IWebHostEnvironment env,
    IHttpContextAccessor httpContextAccessor
) : IImageService
{
    private readonly string _baseFolder = Path.Combine(env.WebRootPath, "images");
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxFileSizeBytes = 8 * 1024 * 1024; // 8MB

    public async Task<Result<string>> UploadUserImageAsync(ImageUploadRequest image, int userId)
    {
        var validationResult = Validate(image);
        if (!validationResult.IsSuccess) return validationResult;

        var folder = Path.Combine(_baseFolder, "users", userId.ToString());
        return await SaveAsync(image, folder, $"users/{userId}");
    }

    public async Task<Result<string>> UploadClinicImageAsync(ImageUploadRequest image, int clinicId)
    {
        var validationResult = Validate(image);
        if (!validationResult.IsSuccess) return validationResult;

        var folder = Path.Combine(_baseFolder, "clinics", clinicId.ToString());
        return await SaveAsync(image, folder, $"clinics/{clinicId}");
    }

    public Task<Result<bool>> DeleteImageAsync(string fileName)
    {
        // strip base URL if full URL was stored, get just the path part
        var uri = new Uri(fileName);
        var relativePath = uri.AbsolutePath.TrimStart('/');
        var fullPath = Path.Combine(env.WebRootPath, relativePath);

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.FromResult(Result<bool>.Success(true));
    }

    // ---- private helpers ----

    private string GetBaseUrl()
    {
        var request = httpContextAccessor.HttpContext!.Request;
        return $"{request.Scheme}://{request.Host}";
    }

    private Result<string> Validate(ImageUploadRequest image)
    {
        var ext = Path.GetExtension(image.FileName).ToLowerInvariant();

        if (!_allowedExtensions.Contains(ext))
            return Result<string>.Failure(ImageErrors.InvalidFormat);

        if (image.SizeInBytes > MaxFileSizeBytes)
            return Result<string>.Failure(ImageErrors.FileTooLarge);

        return Result<string>.Success(string.Empty);
    }

    private async Task<Result<string>> SaveAsync(ImageUploadRequest image, string folder, string urlPrefix)
    {
        Directory.CreateDirectory(folder);

        var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
        var fileName = $"{Guid.NewGuid()}{ext}";
        var fullPath = Path.Combine(folder, fileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await image.Content.CopyToAsync(stream);

        var baseUrl = GetBaseUrl();
        return Result<string>.Success($"{baseUrl}/images/{urlPrefix}/{fileName}");
    }
}