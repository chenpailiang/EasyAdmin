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
        if (context.Exception is BadRequestException bad)
        {
            context.Result = new ObjectResult(bad.Message);
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
        if (context.Exception is UnauthorizedAccessException noAuth)
        {
            context.Result = new ObjectResult(noAuth.Message);
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        }
        else
        {
            context.Result = new ObjectResult("服务器异常 " + context.Exception.Message);
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
        context.ExceptionHandled = true;
    }
}
