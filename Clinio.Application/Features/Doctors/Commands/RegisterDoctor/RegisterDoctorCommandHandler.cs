using Clinio.Application.Common;
using Clinio.Application.DTOs.Auth;
using Clinio.Application.Interfaces;
using Clinio.Domain.Entities.Clinics;
using Clinio.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Doctors.Commands.RegisterDoctor;

public class RegisterDoctorCommandHandler(
    UserManager<ApplicationUser> _userManager,
    IApplicationDbContext _context,
    IJwtProvider _jwtProvider,
    ILocalizationService _localization,
    IImageService _imageService
) : IRequestHandler<RegisterDoctorCommand, Result<AuthResponseDto>>
{
    public async Task<Result<AuthResponseDto>> Handle(RegisterDoctorCommand request, CancellationToken cancellationToken)
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
                LastName = request.LastName,
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

            await _userManager.AddToRoleAsync(user, "Doctor");
            
            // handle image upload after user is created so we have user.Id
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

            Clinic clinic;

            if (!request.ClinicId.HasValue)
            {
                clinic = new Clinic
                {
                    Name = request.RegisterClinic!.Name,
                    Address = request.RegisterClinic.Address,
                    PhoneNumber = request.RegisterClinic.Phone
                };

                await _context.Clinics.AddAsync(clinic, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                var existingClinic = await _context.Clinics
                    .FirstOrDefaultAsync(c => c.Id == request.ClinicId.Value, cancellationToken);

                if (existingClinic is null)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result<AuthResponseDto>.Failure(new ResultError( "Auth.ClinicNotFound",_localization.Get("Auth.ClinicNotFound")));
                }

                clinic = existingClinic;
            }

            var doctor = new Doctor
            {
                Specialty = request.Specialty,
                LicenseNumber = request.LicenseNumber,
                ConsultationFee = request.ConsultationFee,
                ClinicId = clinic.Id,
                UserId = user.Id,
            };

            await _context.Doctors.AddAsync(doctor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var roles = new List<string> { "Doctor" };
            var accessToken = _jwtProvider.GenerateToken(user, roles);
            var refreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshTokens ??= new List<RefreshToken>();
            user.RefreshTokens.Add(refreshToken);

            await _userManager.UpdateAsync(user);
            await transaction.CommitAsync(cancellationToken);

            var response = new AuthResponseDto(
                IsAuthenticated: true,
                Message: _localization.Get("Auth.DoctorRegisterSuccess"),
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