using Clinio.Application.Features.Auth.Commands.UpdateUserImage;
using FluentValidation;

namespace Clinio.Application.Common.Validators.Auth;

public class UpdateUserImageValidator : AbstractValidator<UpdateUserImageCommand>
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxFileSizeBytes = 8 * 1024 * 1024; // 8MB

    public UpdateUserImageValidator()
    {
        RuleFor(x => x.Image.SizeInBytes)
            .GreaterThan(0).WithMessage("Image cannot be empty.")
            .LessThanOrEqualTo(MaxFileSizeBytes).WithMessage("Image must be under 8MB.");

        RuleFor(x => x.Image.FileName)
            .NotEmpty()
            .Must(HaveAllowedExtension)
            .WithMessage("Only jpg, jpeg, png, webp are allowed.");

        RuleFor(x => x.Image.ContentType)
            .Must(BeAllowedContentType)
            .WithMessage("Invalid image content type.");
    }

    private static bool HaveAllowedExtension(string fileName) =>
        AllowedExtensions.Contains(Path.GetExtension(fileName).ToLowerInvariant());

    private static bool BeAllowedContentType(string contentType) =>
        contentType is "image/jpeg" or "image/png" or "image/webp";
}