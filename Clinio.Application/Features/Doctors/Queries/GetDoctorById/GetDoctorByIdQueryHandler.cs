using Clinio.Application.Common;
using Clinio.Application.DTOs.Doctors;
using Clinio.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Doctors.Queries.GetDoctorById;

public class GetDoctorByIdQueryHandler(IApplicationDbContext _context, ILocalizationService _localization)
    : IRequestHandler<GetDoctorByIdQuery, Result<DoctorDto>>
{
    public async Task<Result<DoctorDto>> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
            return Result<DoctorDto>.Failure(new ResultError("Identity.UserNotFound", _localization.Get("Identity.UserNotFound")));

        var doctor = await _context.Doctors
            .Where(d => d.Id == request.Id)
            .Select(d => new DoctorDto(
                d.Id,
                d.ApplicationUser.FirstName + " " + d.ApplicationUser.LastName,
                d.ClinicId,
                d.Clinic.Name,
                d.Specialty,
                d.ApplicationUser.Image
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (doctor is null)
            return Result<DoctorDto>.Failure(new ResultError("Identity.UserNotFound", _localization.Get("Identity.UserNotFound")));

        return Result<DoctorDto>.Success(doctor);
    }
}