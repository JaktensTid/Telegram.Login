namespace Telegram.LoginWidget;

public sealed record LoginWidgetAuthInfo(
    long Id,
    string? FirstName,
    string? LastName,
    string? UserName,
    string? PhotoUrl,
    long AuthDate,
    string Hash);