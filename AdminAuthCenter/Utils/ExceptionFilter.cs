using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdminAuthCenter.Utils;

public class ExceptionFilter : IExceptionFilter
{
    private readonly IWebHostEnvironment env;
    public ExceptionFilter(IWebHostEnvironment env)
    {
        this.env = env;
    }
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is UnauthorizedAccessException)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized; ;
            context.ExceptionHandled = true;
            return;
        }
        if (env.IsDevelopment())
        {
            context.Result = new ObjectResult(context.Exception.Message);
        }
        else
        {
            context.Result = new ObjectResult("服务器异常");
        }
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.ExceptionHandled = true;
    }
}
