using Clinio.Application.Common;
using Clinio.Application.DTOs.Doctors;
using Clinio.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Doctors.Queries.GetAllDoctors;

public class GetAllDoctorsQueryHandler(IApplicationDbContext _context)
    : IRequestHandler<GetAllDoctorsQuery, Result<List<DoctorDto>>>
{
    public async Task<Result<List<DoctorDto>>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
    {
        var doctors = await _context.Doctors
            .Where(d => string.IsNullOrWhiteSpace(request.SearchTerm) ||
                        EF.Functions.Like(d.ApplicationUser.FirstName, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(d.ApplicationUser.LastName, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(d.Clinic.Name, $"%{request.SearchTerm}%"))
            .Select(d => new DoctorDto(
                d.Id,
                d.ApplicationUser.FirstName + " " + d.ApplicationUser.LastName,
                d.ClinicId,
                d.Clinic.Name,
                d.Specialty,
                d.ApplicationUser.Image
            ))
            .ToListAsync(cancellationToken);

        return Result<List<DoctorDto>>.Success(doctors);
    }
}