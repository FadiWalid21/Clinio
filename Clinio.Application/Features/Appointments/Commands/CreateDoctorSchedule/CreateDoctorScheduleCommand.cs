using Clinio.Application.Common;
using MediatR;

namespace Clinio.Application.Features.Appointments.Commands.CreateDoctorSchedule;

public record CreateDoctorScheduleCommand(
    int DoctorId,
    int ClinicId,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int SlotDurationMinutes
) : IRequest<Result<int>>;