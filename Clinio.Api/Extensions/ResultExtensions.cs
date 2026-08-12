using Clinio.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Clinio.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToProblemDetails<T>(
        this Result<T> result,
        ControllerBase controller)
    {
        if (result.IsSuccess)
            return controller.Ok(result.Value);

        var statusCode = result.Error!.Code switch
        {
            "User.NotFound"           => StatusCodes.Status404NotFound,
            "Token.Invalid"           => StatusCodes.Status401Unauthorized,
            "Token.RefreshInvalid"    => StatusCodes.Status401Unauthorized,
            "Token.RefreshExpired"    => StatusCodes.Status401Unauthorized,
            "Auth.Forbidden"          => StatusCodes.Status403Forbidden,
            _                         => StatusCodes.Status400BadRequest
        };
        
        return controller.Problem(
            detail: result.Error!.Description,
            statusCode: statusCode,
            title: result.Error.Code
        );
    }
    
    
}