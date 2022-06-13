using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EasyCommon;

public class CurrentUser
{
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext.User.Claims.Any())
        {
            try
            {
                id = int.Parse(httpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
                account = httpContext.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
                var role = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
                auths = role == null ? null : JsonConvert.DeserializeObject<List<int>>(role.Value);
                isSuper = auths != null && auths.Any(x => x == Utility.SuperAdmin);
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
    public int id { get; init; }
    public string account { get; init; }
    public List<int>? auths { get; init; }
    public bool isSuper { get; init; }
}
