using System.Globalization;
using System.Security.Claims;
using Clinio.Api.Extensions;
using Clinio.Application.Common.Localization;
using Clinio.Application.DTOs.Doctors;
using Clinio.Application.DTOs.RegisterPatientDto;
using Clinio.Application.Features.Auth.Commands.AuthToken;
using Clinio.Application.Features.Auth.Commands.DeleteUserImage;
using Clinio.Application.Features.Auth.Commands.Login;
using Clinio.Application.Features.Auth.Commands.Logout;
using Clinio.Application.Features.Auth.Commands.UpdateUserImage;
using Clinio.Application.Features.Doctors.Commands.RegisterDoctor;
using Clinio.Application.Features.Patients.Commands.RegisterPatient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Clinio.Api.Controllers;

public class AuthController (IStringLocalizer<SharedResource> _localizer) : ApiControllerBase
{
    [HttpPost("register/patient")]
    public async Task<IActionResult> RegisterPatient([FromForm] RegisterPatientDto dto, IFormFile? image)
    {
        ImageUploadRequest? imageRequest = image is null ? null : new(
            image.OpenReadStream(),
            image.FileName,
            image.ContentType,
            image.Length
        );
        
        var command = new RegisterPatientCommand(
            dto.Email,
            dto.Password,
            dto.FirstName,
            dto.LastName,
            dto.DateOfBirth,
            dto.Gender,
            dto.BloodType,
            dto.ChronicDiseases,
            dto.Allergies,
            imageRequest
        );
        
        var result = await Mediator.Send(command);
        return result.ToProblemDetails(this);
    }
    
    [HttpPost("register/doctor")]
    public async Task<IActionResult> RegisterDoctor([FromForm] RegisterDoctorDto dto, IFormFile? image)
    {
        ImageUploadRequest? imageRequest = image is null ? null : new(
            image.OpenReadStream(),
            image.FileName,
            image.ContentType,
            image.Length
        );

        var command = new RegisterDoctorCommand(
            dto.Email,
            dto.Password,
            dto.FirstName,
            dto.LastName,
            dto.Specialty,
            dto.LicenseNumber,
            dto.ConsultationFee,
            dto.RegisterClinic,
            dto.ClinicId,
            imageRequest
        );

        var result = await Mediator.Send(command);
        return result.ToProblemDetails(this);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await Mediator.Send(command);
        return result.ToProblemDetails(this);
    }
    
    [Authorize]
    [HttpPut("me/image")]
    public async Task<IActionResult> UpdateImage([FromForm] IFormFile file)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var imageRequest = new ImageUploadRequest(
            file.OpenReadStream(),
            file.FileName,
            file.ContentType,
            file.Length
        );

        var result = await Mediator.Send(new UpdateUserImageCommand(userId, imageRequest));
        return result.ToProblemDetails(this);
    }
    
    [Authorize]
    [HttpDelete("me/image")]
    public async Task<IActionResult> DeleteImage()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await Mediator.Send(new DeleteUserImageCommand(userId));
        return result.ToProblemDetails(this);
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenCommand command)
    {
        var result = await Mediator.Send(command);

        if (result.IsFailure)
        {
            return Unauthorized(new { error = result.Error });
        }

        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        [FromBody] LogoutCommand command)
    {
        var result = await Mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }
    
    [HttpGet("test-localization")]
    public IActionResult TestLocalization()
    {
        return Ok(new
        {
            TestMessage = _localizer["Validation.Required"].Value,
            CurrentCulture = CultureInfo.CurrentCulture.Name,
            CurrentUICulture = CultureInfo.CurrentUICulture.Name
        });
    }
}