using Clinio.Application.Common;
using Clinio.Application.DTOs.Appointments;
using Clinio.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Appointments.Queries.GetAvailableSlots;

public class GetAvailableSlotsHandler(
    IApplicationDbContext db
) : IRequestHandler<GetAvailableSlotsQuery, Result<List<AvailableSlotDto>>>
{
    public async Task<Result<List<AvailableSlotDto>>> Handle(GetAvailableSlotsQuery request, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var today = DateOnly.FromDateTime(now);
        var currentTime = TimeOnly.FromDateTime(now);

        var slots = await db.TimeSlots
            .Where(t =>
                t.DoctorId == request.DoctorId &&
                t.ClinicId == request.ClinicId &&
                !t.IsBooked &&
                t.Date >= request.FromDate &&
                t.Date <= request.ToDate &&
                // exclude past slots on today's date
                (t.Date > today || (t.Date == today && t.StartTime > currentTime)))
            .OrderBy(t => t.Date)
            .ThenBy(t => t.StartTime)
            .Select(t => new AvailableSlotDto(t.Id, t.Date, t.StartTime, t.EndTime))
            .ToListAsync(ct);

        return Result<List<AvailableSlotDto>>.Success(slots);
    }
}