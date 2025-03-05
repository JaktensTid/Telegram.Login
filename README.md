### Telegram Login Widget \ MiniApps login

#### Usage - login widget

```csharp
var info = new LoginWidgetAuthInfo(id, username, firstname, ...)
var result = LoginWidgetAuth.Verify(info, botToken);

Assert.AreEqual(result, AuthResult.Success);
```

Documentation - https://core.telegram.org/widgets/login

#### Usage - miniapp auth

```csharp
var info = new MiniAppAuthInfo(query, hash);
var result = MiniAppAuth.Verify(info, botToken);

Assert.AreEqual(result, AuthResult.Success);
```

Documentation - https://core.telegram.org/bots/webapps#webappinitdata

#### Install

```csharp
dotnet add package Telegram.LoginWidgetAndMiniApps
```