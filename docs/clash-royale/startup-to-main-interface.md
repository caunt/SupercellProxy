# Current Clash Royale startup: transient main-interface boundary

Date: 2026-10-29

## Result

The analyzed protocol can take the current Null's Royale client through
transport, encrypted login, startup profile delivery, keepalive, and briefly
into the main lobby. The same client can enter the matchmaking UI when the
Battle button is pressed.

This is not yet a stable or deliverable main-interface server. Three native
`SIGSEGV` crashes were observed: once in nested profile string processing after
the `23245` decode path, and twice while attempting to decompress a null input
buffer during a later load. The lobby therefore renders from the current
fixture, but the captured profile does not supply all state required for
sustained execution.

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
server -> 23245, version 10, 133867-byte profile fixture
client -> repeating 10108 KeepAlive
client -> 14760 and 16513 status/update messages
```

After this sequence the client rendered the lobby. Pressing the Battle button
entered the matchmaking UI and produced `16513` list-bearing updates. No
match-found response or playable battle has been implemented.

Later runs crashed in the current runtime at `libg.so+0x138AD14` in the nested
profile string path and at `libg.so+0x136AFD4` while calling
`decompressInMySQLFormat` with a null input. The latter fault was a read from
`X0+3`, confirming a missing or null compressed subdocument rather than a UI
navigation problem.

## Evidence

The current local server is `nulls-royale-server`, listening on `0.0.0.0:9342`.
Its process owns an established loopback client connection and has no upstream
server connection for the successful session.

Relevant artifacts:

| Artifact | SHA-256 |
| --- | --- |
| Captured clientbound `23245` body | `43DB0A4FC8AEE02A1B8480F2DE3B2AD9E2170DA16711C11F7CC33F0F934AD01B` |
| Lobby screenshot | `7F320DD46E50ACDD97B8FAD71FE32BFF5532C9990FCB0A66F4B33E56C5108A51` |
| Matchmaking screenshot | `E6349B02EAD3E8D2839E78B45B2B696BE63807D4D900FCA6605DD7E854957681` |

Local artifact roots:

```text
D:\IDA Scripts\nulls-royale-server\tmp\live-reconnect-20261029
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

- stable operation after the lobby renders;
- non-null defaults for every optional profile string and compressed blob;
- a root-cause fix for the null-input `decompressInMySQLFormat` crash;
- a minimal independently generated `23245` payload;
- complete semantic names for object A, object B, and the outer `23245` values;
- dynamic home/account state rather than replaying one fixture;
- matchmaking, match-found, battle loading, and gameplay state;
- authoritative server rules and persistence for the entered menu state.

The next useful milestone is to make the current startup fixture survive a
bounded soak interval without native crashes. Preserve the fixture while
identifying which profile subdocuments are absent or malformed, then test
non-null defaults before reducing and mutating `23245`. A valid end state is a
local Clash Royale server process that starts from a generated in-memory profile,
renders the main interface, and remains responsive without reading the captured
packet.

## Related notes

- [LoginOk 20963](loginok-20963.md)
- [Analytic event 17537](analytic-event-17537.md)
- [23245 runtime dispatch evidence](profile-23245-runtime-evidence.md)
- [Profile reference audit](profile-reference-audit.md)
