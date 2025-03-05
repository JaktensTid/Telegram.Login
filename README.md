### Telegram Login Widget \ MiniApps login

#### Usage - login widget

```csharp
var info = new LoginWidgetAuthInfo(...)
var result = LoginWidgetAuth.Verify(info, botToken);

Assert.AreEqual(result, AuthResult.Success);
```

Documentation - https://core.telegram.org/widgets/login

#### Usage - miniapp auth

```csharp
var info = new MiniAppAuthInfo(...)
var result = MiniAppAuth.Verify(info, botToken);

Assert.AreEqual(result, AuthResult.Success);
```

Documentation - https://core.telegram.org/bots/webapps#webappinitdata