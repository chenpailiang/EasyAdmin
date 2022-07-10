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
            BadRequestException => (GetExpResult(ex), StatusCodes.Status400BadRequest),
            NotFoundException => (GetExpResult(ex), StatusCodes.Status404NotFound),
            UnauthorizedAccessException => (GetExpResult(ex), StatusCodes.Status403Forbidden),
            _ => (Get500ExpResult(ex), StatusCodes.Status500InternalServerError)
        };
        context.ExceptionHandled = true;
    }

    private ActionResult GetExpResult(Exception ex) => new ObjectResult(new { message = ex.Message });
    private ActionResult Get500ExpResult(Exception ex)
    {
        NLog.LogManager.GetLogger("ServerErrorLogger").Error(ex);
        return new ObjectResult(new { message = "服务器异常,请联系技术人员" });
    }
}
