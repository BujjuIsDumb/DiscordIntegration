# Discord Integration

A library to help integrate Discord into .NET applications using webhooks and RPC.

## Webhooks

Send messages to Discord using webhooks.

```cs
using var myWebhookClient = new WebhookClient("MY_WEBHOOK_URL");

var myWebhookMessage = new WebhookMessage()
    .AddEmbed(new Embed()
    .WithTitle("Discord Integration")
    .WithDescription("A very cool library.")
    .WithColor(EmbedColor.Blurple));

await myWebhookClient.ExecuteAsync(myWebhookMessage)
```

![](https://i.imgur.com/3jCPW2H.png)

## Rich Presence

Let users show off your program on Discord with Rich Presence.

```cs
using var myRpcClient = new RpcClient(1234567890);

myRpcClient.Presence = new RichPresence()
    .WithState("Playing a cool game")
    .WithDetails("Doing something cool")
    .WithParty(new RichPresenceParty(currentSize: 4, maxSize: 5));
```

![](https://i.imgur.com/SFRgubA.png)