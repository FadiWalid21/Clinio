using Clinio.Domain.Entities.Appointments;

namespace Clinio.Application.Interfaces;

public interface ITimeSlotGenerator
{
    List<TimeSlot> GenerateSlots(DoctorSchedule schedule, int daysAhead, HashSet<(DateOnly Date, TimeOnly StartTime)> existingSlots);
}