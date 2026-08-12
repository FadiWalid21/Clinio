using Clinio.Api.Extensions;
using Clinio.Application.Common;
using Clinio.Application.DTOs.Doctors;
using Clinio.Application.Features.Appointments.Commands.CreateDoctorSchedule;
using Clinio.Application.Features.Appointments.Commands.UpdateDoctorSchedule;
using Clinio.Application.Features.Appointments.Queries.GetAvailableSlots;
using Clinio.Application.Features.Appointments.Queries.GetDoctorSchedules;
using Clinio.Application.Features.Doctors.Queries.GetAllDoctors;
using Clinio.Application.Features.Doctors.Queries.GetDoctorById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinio.Api.Controllers;

public class DoctorsController :  ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? searchTerm)
    {
        var result = await Mediator.Send(new GetAllDoctorsQuery(searchTerm));
        return result.ToProblemDetails(this);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await Mediator.Send(new GetDoctorByIdQuery(id));
        return result.ToProblemDetails(this);
    }
    
    [Authorize(Roles = "Doctor")]
    [HttpGet("schedules")]
    public async Task<IActionResult> GetMySchedules()
    {
        // assuming you resolve the doctor's id from current user
        var result = await Mediator.Send(new GetDoctorSchedulesQuery());
        return result.ToProblemDetails(this);
    }
    
    [Authorize(Roles = "Doctor")]
    [HttpPost("schedules")]
    public async Task<IActionResult> CreateSchedule([FromBody] CreateDoctorScheduleCommand command)
    {
        var result = await Mediator.Send(command);
        return result.ToProblemDetails(this);
    }

    [Authorize(Roles = "Doctor")]
    [HttpPut("schedules")]
    public async Task<IActionResult> UpdateSchedule([FromBody] UpdateDoctorScheduleCommand command)
    {
        var result = await Mediator.Send(command);
        return result.ToProblemDetails(this);
    }
    
    [HttpGet("{doctorId:int}/clinics/{clinicId:int}/available-slots")]
    public async Task<IActionResult> GetAvailableSlots(
        int doctorId, int clinicId,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null)
    {
        var from = fromDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var to = toDate ?? from.AddDays(30);

        var result = await Mediator.Send(new GetAvailableSlotsQuery(doctorId, clinicId, from, to));
        return result.ToProblemDetails(this);
    }
}