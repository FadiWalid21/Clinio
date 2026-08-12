using Clinio.Application.Common;
using Clinio.Application.DTOs.Auth;
using Clinio.Application.Interfaces;
using Clinio.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Clinio.Application.Features.Patients.Commands.RegisterPatient;

public class RegisterPatientCommandHandler(
    UserManager<ApplicationUser> _userManager,
    IApplicationDbContext _context,
    IJwtProvider _jwtProvider,
    ILocalizationService _localization,
    IImageService _imageService
) : IRequestHandler<RegisterPatientCommand, Result<AuthResponseDto>>
{
    public async Task<Result<AuthResponseDto>> Handle(RegisterPatientCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return Result<AuthResponseDto>.Failure(new ResultError("Auth.EmailAlreadyExists", _localization.Get("Auth.EmailAlreadyExists")));

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                var error = createResult.Errors.First();
                return Result<AuthResponseDto>.Failure(
                    new ResultError(error.Code, _localization.Get($"Identity.{error.Code}"))
                );
            }
            
            if (request.Image is not null)
            {
                var uploadResult = await _imageService.UploadUserImageAsync(request.Image, user.Id);
                
                if (!uploadResult.IsSuccess)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result<AuthResponseDto>.Failure(uploadResult.Error!);
                }
                user.Image = uploadResult.Value;
                user.ImageFileName = uploadResult.Value;
                await _userManager.UpdateAsync(user);
                
            }

            await _userManager.AddToRoleAsync(user, "Patient");

            var patient = new Patient
            {
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                BloodType = request.BloodType,
                ChronicDiseases = request.ChronicDiseases,
                Allergies = request.Allergies,
                UserId = user.Id,
            };

            await _context.Patients.AddAsync(patient, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var roles = new List<string> { "Patient" };
            var accessToken = _jwtProvider.GenerateToken(user, roles);
            var refreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshTokens ??= new List<RefreshToken>();
            user.RefreshTokens.Add(refreshToken);

            await _userManager.UpdateAsync(user);
            await transaction.CommitAsync(cancellationToken);

            var response = new AuthResponseDto(
                IsAuthenticated: true,
                Message: _localization.Get("Auth.PatientRegisterSuccess"),
                Username: user.UserName!,
                Email: user.Email!,
                Token: accessToken,
                RefreshToken: refreshToken.Token,
                RefreshTokenExpiration: refreshToken.ExpiresOn
            );

            return Result<AuthResponseDto>.Success(response);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result<AuthResponseDto>.Failure(new ResultError("Auth.RegistrationFailed",_localization.Get("Auth.RegistrationFailed")));
        }
    }
}