using System.Security.Cryptography;
using System.Text;

namespace Telegram.LoginWidget;

public sealed class LoginWidgetAuth
{
    /// <summary>
    /// Verify login widget user info.
    /// </summary>
    /// <param name="command">Command.</param>
    /// <param name="botToken">Telegram bot token.</param>
    /// <param name="allowedTimeOffset">Allowed time offset between now and telegram auth date time (Optional).</param>
    /// <returns>Validation result.</returns>
    public static AuthResult Verify(
        LoginWidgetAuthInfo command,
        string botToken,
        int? allowedTimeOffset = null)
    {
        if (command.Id <= 0)
        {
            return AuthResult.InvalidId;
        }

        if (string.IsNullOrWhiteSpace(command.Hash) || command.Hash.Length < 64)
        {
            return AuthResult.InvalidHash;
        }

        if (command.AuthDate <= 0)
        {
            return AuthResult.InvalidAuthDateTime;
        }

        var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        if (allowedTimeOffset != null && Math.Abs(DateTime.UtcNow.Subtract(unixStart).TotalSeconds - command.AuthDate) >
            allowedTimeOffset.Value)
            return AuthResult.Expired;

        var fields = new Dictionary<string, string>
        {
            { "auth_date", command.AuthDate.ToString() },
            { "first_name", command.FirstName },
            { "last_name", command.LastName },
            { "id", command.Id.ToString() },
            { "photo_url", command.PhotoUrl },
            { "username", command.UserName }
        };

        var validFields = fields
            .Where(f => !string.IsNullOrEmpty(f.Value))
            .OrderBy(f => f.Key);

        var dataCheckString = string.Join("\n", validFields.Select(f => $"{f.Key}={f.Value}"));
        var secretKey = SHA256.HashData(Encoding.UTF8.GetBytes(botToken));
        using var hmac = new HMACSHA256(secretKey);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataCheckString));
        var computedHashHex = BitConverter.ToString(computedHash).Replace("-", "").ToLower();
        return computedHashHex == command.Hash
            ? AuthResult.Success
            : AuthResult.Failed;
    }
}