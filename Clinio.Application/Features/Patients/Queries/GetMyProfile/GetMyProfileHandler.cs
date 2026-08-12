using Clinio.Application.Common;
using Clinio.Application.DTOs.Patients;
using Clinio.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Patients.Queries.GetMyProfile;

public class GetMyProfileHandler(
    IApplicationDbContext db,
    ICurrentUserService _userService,
    ILocalizationService _localization
) : IRequestHandler<GetMyProfileQuery, Result<PatientProfileDto>>
{
    public async Task<Result<PatientProfileDto>> Handle(GetMyProfileQuery request, CancellationToken ct)
    {
        var userId = _userService.UserId;

        var profile = await db.Patients
            .Where(p => p.UserId == userId)
            .Select(p => new PatientProfileDto(
                p.Id,
                p.ApplicationUser.FirstName,
                p.ApplicationUser.LastName,
                p.ApplicationUser.Email!,
                p.ApplicationUser.PhoneNumber,
                p.ApplicationUser.Image,
                p.Age,
                p.DateOfBirth,
                p.Gender,
                p.BloodType,
                p.ChronicDiseases,
                p.Allergies
            ))
            .FirstOrDefaultAsync(ct);

        if (profile is null)
            return Result<PatientProfileDto>.Failure(
                new ResultError("Identity.UserNotFound", _localization.Get("Identity.UserNotFound")));

        return Result<PatientProfileDto>.Success(profile);
    }
}
