using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasyAdmin.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    public int contextAdminId { get => int.Parse(Request.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value); } 
}
