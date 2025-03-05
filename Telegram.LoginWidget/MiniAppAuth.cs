using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Telegram.LoginWidget;

public sealed class MiniAppAuth
{
    public static AuthResult Verify(
        MiniAppAuthInfo command,
        string botToken,
        int? allowedTimeOffset = null)
    {
        var queryParams = HttpUtility.ParseQueryString(command.Query);

        if (!queryParams.AllKeys.Contains("hash"))
        {
            return AuthResult.InvalidHash;
        }

        if (!queryParams.AllKeys.Contains("auth_date") || !long.TryParse(queryParams["auth_date"], out var authDate))
        {
            return AuthResult.InvalidAuthDateTime;
        }

        if (!queryParams.AllKeys.Contains("signature"))
        {
            return AuthResult.InvalidSignature;
        }
        
        var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        if (allowedTimeOffset != null && Math.Abs(DateTime.UtcNow.Subtract(unixStart).TotalSeconds - authDate) >
            allowedTimeOffset.Value)
            return AuthResult.Expired;
        
        queryParams.Remove("hash");

        var dataCheckString = string.Join("\n", queryParams.AllKeys
            .OrderBy(key => key)
            .Select(key => $"{key}={queryParams[key]}"));
        
        var secretKey = GetHmacSha256(botToken, Encoding.UTF8.GetBytes("WebAppData"));
        
        var hmac = GetHmacSha256(dataCheckString, secretKey);
        var computedHash = BitConverter.ToString(hmac).Replace("-", "").ToLower();
        return computedHash == command.Hash ? AuthResult.Success : AuthResult.Failed;
    }

    private static byte[] GetHmacSha256(string data, byte[] key)
    {
        using var hmacSha256 = new HMACSHA256(key);
        return hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(data));
    }
}