# Supercell Tools & Proxy

- Brawl Stars
- Hay Day
- Clash of Clans
- Clash Royale
- Squad Busters
- Boom Beach
- mo\.co

## Projects and builds

All projects live under `src/` and share `SupercellProxy.slnx`.

| Project | Availability | Purpose |
| --- | --- | --- |
| SupercellProxy.Networking | Public | Client, Server, and Proxy implementations; all packet, message, command, and nested payload contracts and codecs. |
| SupercellProxy.Capture | Public | Generic Host application running the proxy and recording logical traffic. |
| SupercellProxy.Server | Public | Generic Host server placeholder; reports configuration and exits. |
| SupercellProxy.Simulation | Local, Git-ignored | Game execution, state, navigation, checksums, and the gameplay API. |
| SupercellProxy.Buyer | Local, Git-ignored | Buying strategy consuming the gameplay API. |
| SupercellProxy.Diagnostics | Local, Git-ignored | Simulation replay and inspection tools. |

Keys and PublicKeyExtractor remain public. Public projects have no private project dependencies. Checksum fields are public wire values; calculating them belongs to Simulation.

A public checkout deliberately omits the private project files. Build the committed public solution filter:

```bash
dotnet build SupercellProxy.Public.slnf -c Release
```

Locally, with the private projects present, build the complete solution with `dotnet build SupercellProxy.slnx`. Opening/building the complete solution requires those files; the public filter does not. Private source directories are explicitly ignored by Git. Existing Git history is unchanged.

## Capture and server hosts

```bash
dotnet run --project src/SupercellProxy.Capture -- -b 127.0.0.1 -p 9338 -u 127.0.0.1 --upstream-port 9339 -c /path/to/captures -f FARM_NAME
dotnet run --project src/SupercellProxy.Server -- --ListenAddress 127.0.0.1 --ListenPort 9339
```

Hosts support appsettings and environment configuration alongside their command lines and Ctrl+C shutdown. Capture accepts `-f`/`--farm` with an optional farm-name fragment; omitting the option leaves the downstream login unchanged. When replacement occurs, Capture logs the incoming endpoint, farm name, and account tag without logging credentials. Assets, the session ledger, and captures default to the platform's per-user local application-data directory under `SupercellProxy`; `-d`/`--asset-directory`, `-l`/`--ledger`, and `-c`/`--capture-directory` override those shared locations.

Before its listener starts, Capture downloads or reuses the current game-asset catalog for asset-dependent command decoding, then imports complete login and own-home exchanges from its retained `CaptureDirectory`. New accounts are validated against the configured upstream before being saved; rejected or incomplete captures produce warnings without preventing startup. Successful logins observed by the running proxy are retained after matching own-home data supplies the farm name. Session credentials remain plain JSON using normal platform file permissions.

The versioned ledger stores one tag-string `AccountId`, `FarmName`, `AppStore`, `PassToken`, and optional scalar `SessionToken` and `SessionRefreshToken` per account. An unversioned ledger from the initial implementation is archived beside the selected ledger and rebuilt from retained captures.

Asset-cache and capture formats remain compatible with retained local data. Playground and its build output have been removed. Private replay defaults to the surviving capture collection under `exploration/captures/replay` and retained assets under `exploration/subjects/game-assets/replay`.

## Public protocol API

`AddSupercellNetworking` registers HTTP, logging, replaceable time, a server-key source, and `ProtocolClientFactory`. Hosted roles use `IOptions<ClientOptions>`, `IOptions<ProxyOptions>`, and `IOptions<ServerOptions>`. Their registration methods return typed option builders for configuration binding and startup validation:

```csharp
var proxyOptions = builder.Services.AddProtocolProxy(options =>
{
    options.ListenAddress = "127.0.0.1";
    options.ListenPort = 9338;
}).Bind(builder.Configuration);
```

`AddProtocolClient(onMessage)` and `AddServerPlaceholder()` use the same pattern. Defaults live in the options classes, and existing command-line flags map to their properties. Consumers can replace `IServerPublicKeySource` for their endpoint. A client authenticates and decodes messages without any simulation registration.

Networking groups messages, commands, and payloads by feature under `Protocol/`. See the [Networking directory guide](src/SupercellProxy.Networking/README.md) for ownership, namespaces, and the connection API.

`MessageRegistry` and `CommandRegistry` expose read-only registration views. All message/command types and their nested payload models are public. Use `MessageRegistry.Resolve` to decode a `MessageContainer` and `IMessage.ToContainer` to encode a constructed contract. Supplied turn checksums are preserved. Asset-dependent command payloads use the public `ICommandDataResolver`/`DataTableResolver`; authenticated clients configure that context from downloaded game assets. Capture downloads the current catalog when `-d`/`--asset-directory` is omitted; passing `-d /path/to/version/fingerprint` uses that explicit codec context instead. A hosted proxy can also receive `ICommandDataResolver` through DI. Unknown payload handling is preserved.

Shared engineering rules are supplied by `NetAgents.Analyzers` through `Directory.Build.props`.

Game-table cells use the readonly `LiteralValue` struct. Its `Kind` identifies null, integer, long integer, single, double, boolean, or string values; typed `TryGet` methods preserve the declared type.

## Supercell Games Public Server Keys
[**Table of a public server keys**](https://github.com/caunt/SupercellProxy/blob/main/KEYS.md)  
If the key you need is not there, try [this tool](https://github.com/caunt/SupercellProxy/tree/main/src/SupercellProxy.PublicKeyExtractor) that extracts the public server key.

## Notes

<sub>
This repository is a noncommercial, educational playground for protocol research.<br>
Not affiliated with or endorsed by Supercell.<br>
Use only on systems you own or have written permission to test, in isolated environments.<br>
Provided “AS IS” without warranty; use at your own risk.
</sub>
<br>
<br>

**Do not connect to or interfere with Supercell production services.  
Do not cheat, automate against live games, harvest others' data, or bypass technical protection measures**.
