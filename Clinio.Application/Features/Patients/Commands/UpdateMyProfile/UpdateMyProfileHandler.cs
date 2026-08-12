using Clinio.Application.Common;
using Clinio.Application.Interfaces;
using Clinio.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Patients.Commands.UpdateMyProfile;

public class UpdateMyProfileHandler(
    IApplicationDbContext db,
    ICurrentUserService _userService,
    ILocalizationService _localization,
    UserManager<ApplicationUser> _userManager
) : IRequestHandler<UpdateMyProfileCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateMyProfileCommand request, CancellationToken ct)
    {
        var userId = _userService.UserId;

        var patient = await db.Patients
            .Include(p => p.ApplicationUser)
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

        if (patient is null)
            return Result<bool>.Failure(
                new ResultError("Identity.UserNotFound", _localization.Get("Identity.UserNotFound")));

        // update ApplicationUser fields
        patient.ApplicationUser.FirstName = request.FirstName;
        patient.ApplicationUser.LastName = request.LastName;
        patient.ApplicationUser.PhoneNumber = request.PhoneNumber;

        // update Patient fields
        patient.DateOfBirth = request.DateOfBirth;
        patient.Gender = request.Gender;
        patient.BloodType = request.BloodType;
        patient.ChronicDiseases = request.ChronicDiseases;
        patient.Allergies = request.Allergies;

        await _userManager.UpdateAsync(patient.ApplicationUser);
        await db.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}