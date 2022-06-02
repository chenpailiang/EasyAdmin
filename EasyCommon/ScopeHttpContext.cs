using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EasyCommon;

public class ScopeHttpContext : HttpContextAccessor
{
    public ScopeHttpContext()
    {
        currentAdminId = int.Parse(this.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        currentAdminAccount = this.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
    }
    public int currentAdminId { get; init; }
    public string currentAdminAccount { get; init; }

}
