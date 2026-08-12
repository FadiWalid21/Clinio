using Clinio.Application.Interfaces;
using Clinio.Domain.Entities.Appointments;

namespace Clinio.Infrastructure.Services;

public class TimeSlotGenerator(IClinicClock _clock) : ITimeSlotGenerator
{
    public List<TimeSlot> GenerateSlots(DoctorSchedule schedule, int daysAhead, HashSet<(DateOnly Date, TimeOnly StartTime)> existingSlots)
    {
        var today = _clock.Today;   // ← بدل DateOnly.FromDateTime(DateTime.UtcNow)
        var newSlots = new List<TimeSlot>();

        for (var i = 0; i < daysAhead; i++)
        {
            var date = today.AddDays(i);
            if (date.DayOfWeek != schedule.DayOfWeek) continue;

            var current = schedule.StartTime;

            while (current.AddMinutes(schedule.SlotDurationMinutes) <= schedule.EndTime)
            {
                var slotEnd = current.AddMinutes(schedule.SlotDurationMinutes);

                if (!existingSlots.Contains((date, current)))
                {
                    newSlots.Add(new TimeSlot
                    {
                        DoctorId = schedule.DoctorId,
                        ClinicId = schedule.ClinicId,
                        DoctorScheduleId = schedule.Id,
                        Date = date,
                        StartTime = current,
                        EndTime = slotEnd,
                        IsBooked = false
                    });
                }

                current = slotEnd;
            }
        }

        return newSlots;
    }
}