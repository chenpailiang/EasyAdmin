using EasyCommon;
using System.Security.Claims;

namespace EasyAdmin.Utilitys;

public class HttpContextMiddleware
{
    private RequestDelegate _next;

    public HttpContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        int id = int.Parse(httpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        string account = httpContext.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
        CurrentHttpContext.Init(id, account);
        await _next(httpContext);
    }
}
