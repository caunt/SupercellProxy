# Networking

This public library connects clients and proxies, hosts the server placeholder, and encodes and decodes protocol contracts. It has no dependency on Simulation. Game actions, state advancement, pathfinding, and game checksum calculation belong to the private Simulation project; their wire fields remain public here.

## Finding code

| Directory | Responsibility |
| --- | --- |
| `Client` | Protocol client lifetime, authentication, hosted service, and typed client options. |
| `Proxy` | Listener, one connection's forwarding pumps, handshake, temporary home visits, and optional capture writing. |
| `Server` | The hosted placeholder and its typed endpoint options. |
| `Sessions` | Session records, file persistence, token decoding, refresh, and request signing. |
| `Transport` | Binary values, framed messages, socket utilities, and endpoint resolution. |
| `Cryptography` | Protocol encryption, nonces, key material, and replaceable upstream public-key discovery. |
| `Events` | Typed message interception and the event bus. |
| `Hosting` | DI registration and shared connection logging. |
| `Assets` | Asset files, fingerprints, and the client cache. `Tables` contains CSV parsing, typed literals, reference lookup, and table patching. |
| `Json` | Compressed JSON documents, validation, and array enumeration. |
| `Protocol` | Public message, command, and payload contracts grouped by feature. |

Within `Protocol`, a feature owns its messages, commands, server responses, and nested payloads together. For example, `RoadsideShops` contains purchase requests, purchase results, stock and sale commands, listing contracts, and shop entries. `Newspapers` contains refresh requests, newspaper messages, placements, stories, and snapshots.

`MessageEncoding` owns message envelopes, direction, passthrough handling, and the message registry. `CommandEncoding` owns command envelopes, schemas, fields, and registry dispatch. `MapGame/Events/Fields` describes event fields; `CommandEncoding/*Fields` describes command fields. `CropFields` describes farm-field action contracts.

Numeric identities remain where a feature's meaning is unverified. Such contracts live in `ScalarPayloads`, `CollectionPayloads`, or `OpaquePayloads` according to their proven wire structure. Unknown fields retain neutral names, and opaque bytes remain available for forwarding and re-encoding.

Namespaces follow directories. The two `GameAssetFiles.cs` parts share `SupercellProxy.Networking.Assets`: the production-goods catalog has its own file to keep both parts below the analyzer's file-length limit while retaining one authoritative asset-path owner.

## Connections and hosting

- `Hosting.NetworkingServiceCollectionExtensions.AddSupercellNetworking` registers shared HTTP, logging, replaceable time, public-key discovery, and `Client.ProtocolClientFactory`.
- `AddProtocolClient(onMessage)` registers a hosted authenticated client and returns an `OptionsBuilder<ClientOptions>`.
- `AddProtocolProxy()` registers the proxy and returns an `OptionsBuilder<ProxyOptions>`.
- `AddServerPlaceholder()` returns an `OptionsBuilder<ServerOptions>`; its configuration overload binds the supplied `IConfiguration` directly.
- `ProtocolClientFactory.Create` creates a caller-owned client from an immutable `ClientConfiguration` or an existing `MessageStream`.
- `Sessions.ClientSessionLedger` validates, lists, selects, and atomically saves multiple typed `ClientSession` records keyed by account ID.

Authenticated clients require `SessionAccountIdentifier` and load that account from the ledger at `SessionLedgerPath`, which defaults to the shared per-user local application-data directory. `ClientSessionSelector` provides case-insensitive farm-name matching and numbered console selection without exposing credentials. Account IDs and session tokens remain typed in memory and serialize as tag and JWT strings; protocol compression exists only at the `LoginMessage` wire boundary. The client authenticates, configures asset-dependent codecs, exchanges messages, sends keep-alives, and saves refreshed session-token material after successful authentication. Its internal authenticator and asset cache share the connection's dependencies and lifetime. Consuming a message does not apply its commands or calculate game checksums.

`ProxyConnection` owns both sockets and the forwarding pumps. `ProxyHandshake` handles authentication exchange, optional ledger-account injection, and retention of successful live logins after matching own-home data supplies `FarmName`; `ProxyHomeVisitor` coordinates a temporary visit and response matching. Capture is enabled through `ProxyOptions.CaptureDirectory` by the Capture host and defaults to the shared per-user application-data directory. A null capture directory disables recording.

## Encoding and direction

`MessageStream` reads and writes binary protocol values. Its internal `MessageTransport` owns framing and read/write coordination; `MessageEncryption` owns handshake and nonce state. Public stream operations work with online and memory-backed streams.

`MessageRegistry.Resolve` decodes a `MessageContainer`; `IMessage.ToContainer` encodes a contract. `MessageRegistry` is the sole packet-ID mapping owner. Asset-dependent command fields accept the public `ICommandDataResolver`, implemented by `Assets.Tables.DataTableResolver` without simulation.

`MessageDirection.Clientbound` means traffic travelling toward the game client; `Serverbound` means traffic travelling toward the upstream server. `RemotePeerRole` identifies the peer during encryption setup, so an upstream connection uses `RemotePeerRole.Server`.

Registry entries expose stable `CaptureName` labels. `MessageRegistry.GetCaptureName` supplies capture filenames independently of CLR type renames. Packet versions, JSON member names, session paths, capture formats, and raw payload preservation are independent of this source layout.
