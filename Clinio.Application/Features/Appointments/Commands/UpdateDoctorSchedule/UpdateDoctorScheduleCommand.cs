using Clinio.Application.Common;
using MediatR;

namespace Clinio.Application.Features.Appointments.Commands.UpdateDoctorSchedule;

public record UpdateDoctorScheduleCommand(
    int ScheduleId,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int SlotDurationMinutes
) : IRequest<Result<bool>>;