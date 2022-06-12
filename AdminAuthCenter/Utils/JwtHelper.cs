using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AdminAuthCenter.Utils
{
    public static class JwtHelper
    {
        private static readonly ConcurrentDictionary<int, RefreshToken> refreshTokens = new();
        private static JwtConfig jwtconfig;

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="request"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static BaseReult RefreshJwt(RefreshRequest request, IConfiguration configuration)
        {
            if (string.IsNullOrWhiteSpace(request.token) || string.IsNullOrWhiteSpace(request.refresh))
                return BaseReult.Fail("参数错误");
            var claims = GetClaims(request.token, configuration, true);
            var id = int.Parse(claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            //缓存中用户id对应refreshToken对比
            if (!refreshTokens.TryGetValue(id, out var cacheRefresh) || cacheRefresh?.token != request.refresh)
                return BaseReult.Fail();
            if (cacheRefresh.expire < DateTime.UtcNow)
                return BaseReult.Fail();
            var user = new Admin
            {
                id = id,
                account = claims.First(x => x.Type == ClaimTypes.Name).Value,
            };
            var roles = claims.Where(x => x.Type == ClaimTypes.Role);
            if (roles.Any())
                user.roles = roles.Select(x => int.Parse(x.Value)).ToList(); //TODO 具体权限
            return GenerateJwt(user, configuration, false); //生成新token
        }
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="configuration"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        public static BaseReult GenerateJwt(Admin admin, IConfiguration configuration, bool login = true)
        {
            JwtSecurityTokenHandler jwtHandler = new();
            var filepath = Directory.GetCurrentDirectory();
            if (!TryGetSecret(filepath, out var secret))
                throw new Exception("jwt密钥配置读取失败");
            jwtconfig ??= configuration.GetSection("jwtConfig").Get<JwtConfig>();
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier,admin.id.ToString()),
                new Claim(ClaimTypes.Name,admin.account)
            };
            if (admin.roles != null && admin.roles.Count > 0)
            {
                if (admin.roles.Any(x => x == 1)) // 超级管理员
                {
                    claims.Add(new Claim(ClaimTypes.Role, "[-1]"));
                }
                else
                {
                    var roles = DbContext.Db.Queryable<Role>().In(admin.roles).Where(x => x.oust == 0).ToList();
                    var funcs = DbContext.Db.Queryable<MenuFunc>().In(roles.SelectMany(x => x.funcs).Distinct()).Where(x => x.oust == 0).ToList();
                    var authIds = funcs.Select(x => x.authId).Distinct();
                    if (authIds.Any())
                    {
                        claims.Add(new Claim(ClaimTypes.Role, JsonConvert.SerializeObject(authIds)));
                    }
                }
            }
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Issuer = jwtconfig.issuer,
                Audience = jwtconfig.audience,
                Expires = DateTime.UtcNow.AddMinutes(jwtconfig.accessExpire),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new(new RsaSecurityKey(secret), SecurityAlgorithms.RsaSha256Signature)
            };
            var token = jwtHandler.CreateToken(tokenDescriptor);
            var result = JwtResult.Ok(jwtHandler.WriteToken(token), login);
            if (login)
                refreshTokens.AddOrUpdate(admin.id, new RefreshToken() { expire = DateTime.UtcNow.AddMinutes(jwtconfig.refreshExpire), token = result.refresh }, (id, token) => token);
            return result;
        }

        public static void ClearJwt(int userId)
        {
            refreshTokens.TryRemove(userId, out _);
        }

        public static bool TryGetSecret(string filepath, out RSAParameters secret)
        {
            var kpath = Path.Combine(filepath, "secretKey.json");
            var kjson = "";
            secret = default;
            if (File.Exists(kpath) && (kjson = File.ReadAllText(kpath)) != "")
            {
                secret = JsonConvert.DeserializeObject<RSAParameters>(kjson);
                return true;
            }
            return false;
        }

        public static List<Claim> GetClaims(string token, IConfiguration configuration, bool isRefresh = false)
        {
            var filepath = Directory.GetCurrentDirectory();
            if (!TryGetSecret(filepath, out var secret))
                throw new Exception("jwt密钥配置读取失败");
            jwtconfig ??= configuration.GetSection("jwtConfig").Get<JwtConfig>(); // 获取jwt配置
            JwtSecurityTokenHandler jwtHandler = new();
            ClaimsPrincipal claims;
            try
            {
                TokenValidationParameters validParams = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(secret),
                    ValidateIssuer = true,
                    ValidIssuer = jwtconfig.issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtconfig.audience,
                    ValidateLifetime = !isRefresh,
                    RequireExpirationTime = !isRefresh,
                };
                validParams.ClockSkew = isRefresh ? validParams.ClockSkew : TimeSpan.Zero;
                //验证原token
                claims = jwtHandler.ValidateToken(token, validParams, out var ss);
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException();
            }
            return claims.Claims.ToList();
        }
    }

    public class JwtConfig
    {
        public string issuer { get; set; }
        public string audience { get; set; }
        public int accessExpire { get; set; }
        public int refreshExpire { get; set; }
    }

    public class BaseReult
    {
        public bool success { get; set; }
        public string error { get; set; }
        public static BaseReult Fail(string err = "登录信息已失效，请重新登录") => new() { success = false, error = err };
        public static BaseReult Ok() => new() { success = true };
    }
    public class JwtResult : BaseReult
    {
        public string token { get; set; }
        public string refresh { get; set; }
        public static JwtResult Ok(string res, bool login) => new() { success = true, token = res, refresh = login ? Guid.NewGuid().ToString() : null };
    }
    public record RefreshRequest(string token, string refresh);

    public class RefreshToken
    {
        public string token { get; set; }
        public DateTime expire { get; set; }
    }
}
