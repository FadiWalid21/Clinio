namespace Clinio.Application.Interfaces;

public interface IClinicClock
{
    DateTime Now { get; }
    DateOnly Today { get; }
}