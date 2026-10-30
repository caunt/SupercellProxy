# Current Clash Royale startup: verified main-interface boundary

Date: 2026-10-29

## Result

The analyzed protocol can take the current Null's Royale client through
transport, encrypted login, startup profile delivery, keepalive, and into the
main lobby. The successful session remained in the lobby with the process and
connection alive for more than six minutes and produced no native crash.

The earlier native `SIGSEGV` runs were not evidence that the profile lacked
mandatory fields. They fed the 133,867-byte captured **sealed frame** to the
server as if it were the 133,844-byte decrypted `23245` **object body**. The
server then encrypted that frame a second time. Its leading bytes were the
inner transport wrapper `5ACD020AE400`, rather than the object payload
`0000000004426004`. The client consequently interpreted wrapper bytes as the
profile representation and eventually reached a null-input decompressor.

The successful run used both corrections:

- `NR_POST_MESSAGE_TRIGGER_ID=30000`, so `23245` is sent at the post-consent
  device-information transition;
- `NR_POST_LOGIN_PAYLOAD_FILE` points to the 133,844-byte decrypted profile,
  not the 133,867-byte captured frame.

The main-lobby state is therefore a verified replay boundary for the supplied
decrypted profile. It is not yet proof that the local server can synthesize or
redact that profile field by field.

## Build identity

Two `libg` builds must remain separate:

| Role | SHA-256 |
| --- | --- |
| Current Null's Royale runtime | `110AA2B5CAC391C498645E072B0D88729428C2C2845E7E2737CA8EE979059783` |
| Older official-server analysis base | `B1CAF6D94F7EFFAFEC4900A78F5D96DEFD98364B082A3CF5E560622A6858993C` |

The current runtime build is the official `libg` lineage with the Null
public-key and URL patches. The older build contains the accumulated analysis,
renames, comments, schemas, and cross-references. Semantic knowledge can be
compared across the builds, but absolute addresses and type-dependent views
cannot be mixed.

The current runtime has its own database and IDB under:

```text
D:\IDA Scripts\ida-ai\agent_reports\libg-null-runtime-110aa2b5
```

## Verified transport and startup sequence

The Android device forwarded `9339` to a local server on `9342`. The separate
real-server relay on `9341` was listening but was not in the active socket path.

The observed sequence was:

```text
client -> 10100 ClientHello
server -> 20100 ServerHello
client -> encrypted 10101 Login
server -> 20963 LoginOk
client -> 17537 AnalyticEvent
client -> 10108 KeepAlive
client -> 10108 KeepAlive
client -> 30000 PostConsentDeviceInfo
server -> 23245, version 10, 133844-byte plaintext profile
client -> repeating 10108 KeepAlive
client -> 14760 and 16513 status/update messages
```

After this sequence the client rendered the lobby. The server log records
traffic in the same event loop as `23245` followed by the `30000` dispatch;
the push is emitted before the incoming frame reaches its handler log line.
The wire order visible in the capture is therefore still `30000` then `23245`.

The earlier failures occurred at `libg.so+0x138AD14` in nested profile string
processing and at `libg.so+0x136AFD4` while calling
`decompressInMySQLFormat` with a null input. Those observations remain useful
crash-path evidence, but they must not be attributed to an incomplete valid
profile until a control run with the correct plaintext body reproduces them.

## Evidence

The current local server is `nulls-royale-server`, listening on `0.0.0.0:9342`.
Its process owns an established loopback client connection and has no upstream
server connection for the successful session.

Relevant artifacts:

| Artifact | SHA-256 |
| --- | --- |
| Decrypted `23245` profile body, 133,844 bytes | `16D983559A639552ABDBEB43814EA64FD29976086A2AAC6D55255107362BBD2E` |
| Captured sealed clientbound `23245` frame, 133,867 bytes | `43DB0A4FC8AEE02A1B8480F2DE3B2AD9E2170DA16711C11F7CC33F0F934AD01B` |
| Successful lobby screenshot | `371DD85C9B8E0CBA0B243091819D3E8295F34F39B0A47C203F543BEAB72F7C66` |
| Matchmaking screenshot | `E6349B02EAD3E8D2839E78B45B2B696BE63807D4D900FCA6605DD7E854957681` |

Local artifact roots:

```text
D:\IDA Scripts\nulls-royale-server\tmp\live-reconnect-20261029
D:\IDA Scripts\nulls-royale-server\tmp\live-trigger30000-20261029
D:\IDA Scripts\clash-royale-supply-chain\work\null-libg-analysis\live-monitor-20261029
```

## Implemented boundary

The server currently implements:

- `10100` ClientHello parsing.
- `20100` ServerHello generation.
- current handshake and established SecretBox processing.
- `10101` authenticated login acceptance.
- current `20963` LoginOk generation.
- `17537` analytic-event acceptance.
- repeating empty `10108` KeepAlive acceptance.
- replay of the captured `23245` profile fixture.
- continued `16513` status traffic from the client.

## Remaining boundary

The following are not yet complete:

- a minimal independently generated `23245` payload;
- complete semantic names for object A, object B, and the outer `23245` values;
- dynamic home/account state rather than replaying one fixture;
- matchmaking, match-found, battle loading, and gameplay state;
- authoritative server rules and persistence for the entered menu state.

The next useful milestone is to reduce and regenerate the decrypted profile
without changing the verified wire order. Keep the trigger and payload layers
separate in tooling: trigger on `30000`; read the 133,844-byte plaintext body;
let the transport seal it exactly once. A valid end state is a local Clash
Royale server process that starts from a generated in-memory profile, renders
the main interface, and remains responsive without reading the captured packet.

## Related notes

- [LoginOk 20963](loginok-20963.md)
- [Analytic event 17537](analytic-event-17537.md)
- [23245 runtime dispatch evidence](profile-23245-runtime-evidence.md)
- [Profile reference audit](profile-reference-audit.md)
