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
    public int authId = (int)RoleEnum.超级管理员; // 默认需要超级权限
    public AuthSetAttribute(AuthEnum auth)
    {
        authId = (int)auth;
    }
}
/// <summary>
/// 鉴权
/// </summary>
public class AuthCheckFilter : IAuthorizationFilter
{
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
                    var role = context.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role);
                    var authIds = role.Select(x => int.Parse(x.Value));
                    var req_authId = int.Parse(context.HttpContext.Request.Headers["aid"]);
                    if (req_authId != authSet.authId || !authIds.Any(x => x == (int)AuthEnum.sup || x == req_authId))
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