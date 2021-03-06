using System.Security.Claims;
using EasyCommon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace EasyAdmin.Utilitys;

[AttributeUsage(AttributeTargets.Method)]
public class AuthSetAttribute : Attribute
{
    public int authId = Utility.SuperAdmin; // 默认需要超级权限
    public AuthSetAttribute(AuthEnum auth = 0)
    {
        if (auth > 0) authId = (int)auth;
    }
}
/// <summary>
/// 鉴权
/// </summary>
public class AuthCheckFilter : IAuthorizationFilter
{
    private readonly CurrentUser currentUser;
    public AuthCheckFilter(CurrentUser currentUser)
    {
        this.currentUser = currentUser;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var needAuth = context.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is AllowAnonymousAttribute) == null;
        if (needAuth)
        {
            var authSet = (AuthSetAttribute?)context.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is AuthSetAttribute);
            if (authSet != null)
            {
                try
                {
                    var authIds = currentUser.auths;
                    if (authIds == null || !authIds.Any(x => x == Utility.SuperAdmin || x == authSet.authId))
                    {
                        context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Result = new EmptyResult();
                    }
                }
                catch (Exception)
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Result = new EmptyResult();
                }
            }
        }
    }
}