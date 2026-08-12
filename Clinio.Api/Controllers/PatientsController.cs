using Clinio.Api.Extensions;
using Clinio.Application.Features.Appointments.Commands.CancelAppointment;
using Clinio.Application.Features.Appointments.Commands.ConfirmAppointment;
using Clinio.Application.Features.Appointments.Commands.CreateAppointment;
using Clinio.Application.Features.Appointments.Queries.GetMyAppointments;
using Clinio.Application.Features.Doctors.Queries.GetAllDoctors;
using Clinio.Application.Features.Doctors.Queries.GetDoctorById;
using Clinio.Application.Features.Patients.Commands.UpdateMyProfile;
using Clinio.Application.Features.Patients.Queries.GetMyProfile;
using Clinio.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinio.Api.Controllers;

public class PatientsController :  ApiControllerBase
{
    // [HttpGet]
    // public async Task<IActionResult> GetAll([FromQuery] string? searchTerm)
    // {
    //     var result = await Mediator.Send(new GetAllDoctorsQuery(searchTerm));
    //     return result.ToProblemDetails(this);
    // }
    //
    // [HttpGet("{id:int}")]
    // public async Task<IActionResult> GetById(int id)
    // {
    //     var result = await Mediator.Send(new GetDoctorByIdQuery(id));
    //     return result.ToProblemDetails(this);
    // }
    
    [Authorize(Roles = "Patient")]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var result = await Mediator.Send(new GetMyProfileQuery());
        return result.ToProblemDetails(this);
    }

    [Authorize(Roles = "Patient")]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateMyProfileCommand command)
    {
        var result = await Mediator.Send(command);
        return result.ToProblemDetails(this);
    }
    
    
}