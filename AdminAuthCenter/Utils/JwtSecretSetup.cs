using Newtonsoft.Json;
using System.Security.Cryptography;

namespace AdminAuthCenter.Utils;

public static class JwtSecretSetUp
{
    public static void AddJwtSecretSetUp(this IServiceCollection services)
    {
        var filepath = Directory.GetCurrentDirectory();
        if (!JwtHelper.TryGetSecret(filepath, out var secret))  // 获取RSA私钥
        {
            //生成密钥对
            using var rsa = RSA.Create(2048);
            secret = rsa.ExportParameters(true);
            File.WriteAllText(Path.Combine(filepath, "secretKey.json"), JsonConvert.SerializeObject(secret));
            File.WriteAllText(Path.Combine(filepath, "publicKey.json"), JsonConvert.SerializeObject(rsa.ExportParameters(false)));
        }
    }
}
