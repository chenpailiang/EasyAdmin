using EasyCommon;
using EasyService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace EasyAdmin.Utilitys;

/// <summary>
/// 全局异常处理
/// </summary>
public class GlobalExceptionFilter : IExceptionFilter
{
    public GlobalExceptionFilter()
    {

    }
    public void OnException(ExceptionContext context)
    {
        var ex = context.Exception;
        (context.Result, context.HttpContext.Response.StatusCode) = ex switch
        {
            BadRequestException => (GetResult(ex), StatusCodes.Status400BadRequest),
            NotFoundException => (GetResult(ex), StatusCodes.Status404NotFound),
            UnauthorizedAccessException => (GetResult(ex), StatusCodes.Status403Forbidden),
            _ => (new ObjectResult(new { message = $"服务器异常:{ex.Message}" }), StatusCodes.Status500InternalServerError)
        };
        context.ExceptionHandled = true;
    }

    private ActionResult GetResult(Exception ex) => new ObjectResult(new { message = ex.Message });
}
