using EasyCommon;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace EasyAdmin.Utilitys;

public static class JwtAuthConfig
{
    /// <summary>
    /// jwt配置
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddJwtAuthSetup(this IServiceCollection services)
    {
        var jwtconfig = ConfigTool.Get<JwtConfig>("JwtConfig");
        var pubicKey = new RsaSecurityKey(JsonConvert.DeserializeObject<RSAParameters>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "publicKey.json"))));
        services.AddAuthentication().AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = pubicKey,
                    ValidateIssuer = true,
                    ValidIssuer = jwtconfig.issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtconfig.audience,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
    }
    public class JwtConfig
    {
        public string issuer { get; set; }
        public string audience { get; set; }
        public int accessExpire { get; set; }
        public int refreshExpire { get; set; }
    }
}
