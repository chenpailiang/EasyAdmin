using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using AdminAuthCenter.Utils;
using SqlSugar;

namespace AdminAuthCenter.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration configuration;
        private readonly ISqlSugarClient db;

        public AuthController(ILogger<AuthController> logger, IConfiguration configuration, ISqlSugarClient db)
        {
            _logger = logger;
            this.configuration = configuration;
            this.db = db;
        }

        public record LoginModel(string account, string pwd);

        /// <summary>
        /// 登录 生成token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login([FromBody] LoginModel user)
        {
            var admin = db.Queryable<Admin>().First(x => x.account == user.account && x.pwd == user.pwd && !x.oust);
            if (admin == null)
                return Ok(new BaseReult { error = "账号或密码错误，登录失败请重试" });
            var token = JwtHelper.GenerateJwt(admin, configuration);
            return Ok(token);
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Refresh([FromBody] RefreshRequest request) => Ok(JwtHelper.RefreshJwt(request, configuration));

        public record LogoutModel(string token);

        /// <summary>
        /// 移除refreshToken
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns> 
        public ActionResult Logout([FromBody] LogoutModel request)
        {
            var claims = JwtHelper.GetClaims(request.token, configuration);
            var id = int.Parse(claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var admin = db.Queryable<Admin>().First(x => x.id == id);
            if (admin == null)
                return Ok(new BaseReult { error = "账号不存在" });
            JwtHelper.ClearJwt(id);
            return Ok();
        }

        public record ChangeModel(string token, string oldPwd, string newPwd1, string newPwd2);
        public ActionResult ChangePwd([FromBody] ChangeModel request)
        {
            if (request.newPwd2 != request.newPwd1)
                return Ok(new BaseReult { error = "新密码两次输入不一致" });
            var claims = JwtHelper.GetClaims(request.token, configuration);
            var id = int.Parse(claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var admin = db.Queryable<Admin>().First(x => x.id == id);
            if (admin == null)
                return Ok(new BaseReult { error = "账号不存在" });
            if (admin.pwd != request.oldPwd)
                return Ok(new BaseReult { error = "原密码错误" });
            if (request.oldPwd == request.newPwd1)
                return Ok(new BaseReult { error = "新密码不能与原密码一样" });
            JwtHelper.ClearJwt(id);
            admin.pwd = request.newPwd1;
            db.Updateable<Admin>(admin).ExecuteCommand();
            return Ok();
        }
    }
}