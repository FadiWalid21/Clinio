using System;
using Clinio.Domain.Enums;

namespace Clinio.Application.DTOs.Appointments;

public record MyAppointmentDto(
    int Id,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string DoctorName,
    string ClinicName,
    string ClinicAddress,
    decimal ConsultationFee,
    AppointmentStatus Status,
    string? Notes,
    string? CancellationReason,
    DateTime CreatedAt
);