using Clinio.Api.Extensions;
using Clinio.Application.Features.Appointments.Commands.CancelAppointment;
using Clinio.Application.Features.Appointments.Commands.CompleteAppointment;
using Clinio.Application.Features.Appointments.Commands.ConfirmAppointment;
using Clinio.Application.Features.Appointments.Commands.CreateAppointment;
using Clinio.Application.Features.Appointments.Queries.GetMyAppointments;
using Clinio.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinio.Api.Controllers;

public class AppointmentsController : ApiControllerBase
{
    
    [Authorize(Roles = "Patient,Secretary")]
    [HttpPost()]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentCommand command)
    {
        var result = await Mediator.Send(command);
        return result.ToProblemDetails(this);
    }

    [Authorize(Roles = "Doctor,Secretary")]
    [HttpPut("{appointmentId:int}/confirm")]
    public async Task<IActionResult> ConfirmAppointment(int appointmentId)
    {
        var result = await Mediator.Send(new ConfirmAppointmentCommand(appointmentId));
        return result.ToProblemDetails(this);
    }

    [Authorize(Roles = "Patient,Secretary")]
    [HttpPut("{appointmentId:int}/cancel")]
    public async Task<IActionResult> CancelAppointment(int appointmentId, [FromBody] string? reason)
    {
        var result = await Mediator.Send(new CancelAppointmentCommand(appointmentId, reason));
        return result.ToProblemDetails(this);
    }

    [Authorize(Roles = "Patient")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyAppointments([FromQuery] AppointmentStatus? status)
    {
        var result = await Mediator.Send(new GetMyAppointmentsQuery(status));
        return result.ToProblemDetails(this);
    }
    
    [Authorize(Roles = "Doctor")]
    [HttpPut("{appointmentId:int}/complete")]
    public async Task<IActionResult> CompleteAppointment(int appointmentId)
    {
        var result = await Mediator.Send(new CompleteAppointmentCommand(appointmentId));
        return result.ToProblemDetails(this);
    }
}




