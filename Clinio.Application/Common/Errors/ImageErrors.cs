namespace Clinio.Application.Common.Errors;

public static class ImageErrors
{
    public static readonly ResultError InvalidFormat =
        new("Image.InvalidFormat", "Only jpg, jpeg, png, webp are allowed.");

    public static readonly ResultError FileTooLarge =
        new("Image.TooLarge", "Image must be under 5MB.");
}