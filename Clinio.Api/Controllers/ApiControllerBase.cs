using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clinio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiControllerBase : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator => 
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}