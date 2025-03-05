namespace Telegram.LoginWidget;

public enum AuthResult
{
    Success,
    Failed,
    Expired,
    InvalidId,
    InvalidSignature,
    InvalidHash,
    InvalidAuthDateTime
}