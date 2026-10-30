# Clash Royale battle protocol boundary

Research snapshot: 2026-10-30

This note records the first evidence-bounded pass over matchmaking and battle
protocol code. It deliberately separates confirmed same-build protocol facts,
plausible semantic matches, runtime observations, and interpretations imported
from other releases. The purpose is to establish a trustworthy starting point
for recovering the active battle flow without mixing unrelated versions.

## Build isolation

There are two `libg` identities in this investigation:

| Role | SHA-256 | Analysis / runtime source |
| --- | --- | --- |
| Legacy official-server knowledge base | `B1CAF6D94F7EFFAFEC4900A78F5D96DEFD98364B082A3CF5E560622A6858993C` | `agent_reports/libg-protocol-20261023/libg.i64` and `libg.sqlite` |
| Null client runtime, derived from a newer official client | `110AA2B5CAC391C498645E072B0D88729428C2C2845E7E2737CA8EE979059783` | `clash-royale-supply-chain/work/NullsRoyale/.../arm64-v8a/libg.so`; isolated runtime analysis uses `libg-null-runtime-110aa2b5` |

Addresses, message IDs, command IDs, vtables, and object offsets in this note
belong to the legacy `B1CAF6D9` build unless explicitly labeled as a runtime
observation. They must not be copied into the `110AA2B5` build without checking
that build's codecs and consumers. Message IDs may remain stable across nearby
releases, but that is a hypothesis to test rather than a transport rule.

## Confirmed protocol boundary

### Clientbound dispatch

The legacy dispatcher at `0xADBC1C` receives a decoded Piranha message and uses
the message ID plus fields stored at object offset `0x90`. This function is a
clientbound router: it updates UI, stores decoded state, and invokes downstream
controllers. The presence of a numeric case in this dispatcher does not prove
that the corresponding message is part of a network battle.

The following same-build codec facts are current:

| ID | Wire shape from the selected codec | Dispatcher evidence | Best-supported meaning |
| --- | --- | --- | --- |
| `23913` | one packed signed integer stored at `+0x90` | Maps values to explicit matchmaking-failure resource IDs | Matchmaking failure reason notification. High confidence. |
| `21186` | one packed signed integer stored at `+0x90` | Constructs `TID_PUSH_MATCHMAKING_TOP` and passes the decoded value into the UI event | Matchmaking presentation/status notification. The integer is UI-facing; its exact scale or enum is unresolved. |
| `21162` | bool, then a two-word integer pair (`boolean_bit`, `u32`, `u32`) | Reaches the common `sub_CA4270` path used by the dispatcher region associated with tournament matchmaking | Tournament/matchmaking notification is a strong hypothesis; the pair's semantics are unresolved. |
| `21240` | one packed signed integer stored at `+0x90` | Checks active matchmaking/sector state before pushing matchmaking UI | Matchmaking transition or presentation message; exact states are unresolved. |
| `21393` | integer, bool, plus an unresolved shared-dispatch helper | Uses the common tournament/matchmaking dispatcher path | Tournament-related candidate only. The object meaning and nested value are not established. |
| `18647` | empty payload | No battle-specific consumer established | Unknown empty-payload notification. It may be a trigger or acknowledgement, but treating it as a battle-start message would be speculation. |
| `18685` | signed integer, bool, and a fixed two-word integer pair | No battle-specific consumer established | Unknown structured notification with two scalar-like components. It should not be named from the AstralRoyale ID list alone. |

For `23913`, the dispatcher has a particularly strong semantic chain:

- `9` selects `TID_MATCHMAKE_FAILED_WAR_SECTION_ENDED`;
- `10` selects either `TID_SHUTDOWN_BATTLE_DISABLED` or
  `TID_MATCHMAKE_FAILED_DUELS_DISABLED`;
- `11` selects `TID_MATCHMAKE_FAILED_MAX_WAR_PARTICIPANTS`;
- `3` selects `TID_TOURNAMENT_MATCHMAKE_FAILED_PROCESSING`;
- `7` selects `TID_MATCHMAKE_FAILED_WAR_ANOTHER_PLAYER_BATTLING`;
- other values fall back to a generic matchmaking-failure message.

This establishes a failure-reason discriminator, not the complete matchmaking
state machine.

### UDP connection-establishment candidate

The legacy `B1CAF6D9` clientbound dispatcher contains a direct `21631` branch
that tears down any existing high-rate UDP socket, configures a replacement,
and sends `16226` over it. This is static evidence from the same build; it does
not yet prove that `21631` is the message currently delivered after a successful
match in the `110AA2B5` runtime.

The same-build codec catalogue resolves `21631` as four logical fields:

| Order | Wire value | Reconstructed object offset | Consumer evidence |
| --- | --- | --- | --- |
| 1 | UTF-8 string with a big-endian length prefix | `+0xB0` | Must be at least 80 bytes before the UDP handler builds two transform configurations from it. |
| 2 | packed signed 32-bit integer | `+0x90` | Reaches the UDP address constructor as its 16-bit port argument. |
| 3 | string with a big-endian length prefix | `+0x98` | Is split on `.` and four components are packed into the destination IPv4 address. |
| 4 | byte array with a big-endian length prefix | pointer `+0xA0`, length `+0xA8` | The UDP endpoint path consumes the first eight bytes as a connection value. |

The decoder has five primitive reads because the final byte array is read as a
length and then its bytes. The encoder has four logical writes and is the better
representation of the wire schema. Object offsets above are local to the legacy
`LibgMessage_21631` class and must not be reused for another build.

The strongest consumer chain is:

```text
21631 decoded
-> move +0x98 endpoint string
-> split host into four IPv4 components
-> use +0x90 as UDP port
-> copy eight bytes from +0xA0 as connection value
-> configure HighRateUdpSocket
-> construct HighRateUdpPacketHandler
-> use +0xB0 as >=80-byte transform configuration
-> send empty-payload Piranha message 16226 over the new UDP socket
```

The following statement is a high-confidence hypothesis rather than a field
name: `21631` is the target build's equivalent of an older
`UdpConnectionInfoMessage`. AstralRoyale composes its old `25513`
`UdpConnectionInfoMessage` from UDP port, host, logic-long session ID, and nonce.
The target has the same reusable pieces in a different order, but its message
ID and codec were re-verified locally. It is therefore valid to call
`port`, `host`, `connection value`, and `transform configuration`; it is too
early to call the four fields `ServerPort`, `ServerHost`, `SessionID`, and
`Nonce` as if those names were inherited facts.

`16226` is confirmed as an empty-payload Piranha message constructed and sent
through the newly configured high-rate UDP socket. Its direction is
client-to-server because the socket send is explicit. Whether it is named
connection acknowledgement, handshake, or keepalive remains unresolved.

The UDP packet layer is also present in the same build. It assigns sequence
and acknowledgement numbers, tracks a 16-bit sequence space with `65535` and
`65000` treated as special values, fragments transformed Piranha messages,
and uses 1456-byte and 1448-byte packet limits depending on transport
configuration. A datagram therefore contains transport envelopes carrying one
or more Piranha messages; `16226` is a message inside this layer, not proof that
the first datagram has an empty body. The exact byte order and bit layout of
the transport header still need to be closed against a packet builder or live
datagram.

### UI and unrelated battle-like code

The following were checked and removed from the network-battle candidate set:

- `16513` is a status/UI update and is not a gameplay command. It also appears
  repeatedly in the current runtime while no battle is occurring.
- `0x10BBE60` contains `BattleInit`, `Preparing`, and `Battling` names, but it is
  an AutoChess state machine. Those names do not describe Clash Royale PvP.
- `0x95729C`, `0xA97AE8`, and `0xBDDE80` are matchmaking UI, animation, or
  local state-machine code.
- `0xC92458` and `0xCCE5CC` are battle debug/context output.
- `0x1089460` and `0x11150A4` are small accessors for home battle/card data.
- `0xD888C8`, `0xD8B990`, `0xE191AC`, and `0xE19EE8` consume matchmaking
  configuration. No evidence yet shows that any of them writes a matchmaking
  request to the network.

## Runtime observation

On the Null client build `110AA2B5`, tapping the yellow home-screen battle
button opened the animated `Searching for opponent` screen. This proves that the
home UI and local matchmaking presentation transition executed.

The controlled server received no new request after the transition. Its log
continued to show only keepalive and `16513` messages. A device socket snapshot
showed:

- the existing Piranha TCP connection from the app to `127.0.0.1:9339`;
- no new TCP endpoint associated with the game;
- no UDP endpoint associated with the game.

Current interpretation: this profile is likely entering a tutorial or local
training gate that displays matchmaking UI before issuing a real PvP request.
That is a hypothesis, not a confirmed state-machine fact. The observation does
establish that opening the matchmaking screen is not sufficient evidence for a
network matchmaking message.

The experiment was repeated after a clean client start and with Frida attached
to the already-running process. The frame logger observed the same boundary for
35 seconds after the second click: the only message IDs leaving the client were
`10108` (KeepAlive) and `16513` (the status/UI update). No `10100` login,
`10101`, matchmaking, sector, or result-shaped frame occurred after the click,
and the app created no additional TCP or UDP endpoint. The search screen
remained active and the tutorial hint overlay continued to be shown.

Runtime artifacts:

- `ida-ai/.ida-ai-cache/battle-runtime-20261030-current2.png`
- `ida-ai/.ida-ai-cache/battle-after-cancel3.png`
- `ida-ai/.ida-ai-cache/battle-reclick2.png`
- `ida-ai/.ida-ai-cache/battle-search-wait35.png`
- `ida-ai/.ida-ai-cache/battle-after-frida-click.png`
- `ida-ai/.ida-ai-cache/battle-live-click2.png`
- `ida-ai/.ida-ai-cache/battle-runtime-click-20261030.jsonl`
- `ida-ai/.ida-ai-cache/battle-runtime-live-20261030.jsonl`
- `nulls-royale-server/tmp/profile-sentinel-20261030/baseline/server.out.log`
- `nulls-royale-server/tmp/profile-sentinel-20261030/baseline/capture/`

## Cross-version prior, not fact

AstralRoyale is a 2.1.8 reference. Its flow is useful as a list of questions:

```text
EndClientTurn(command 594)
-> MatchmakeInfo
-> StopHomeLogic
-> UdpConnectionInfo
-> SectorState
-> RequestSectorState
-> SectorCommand / SectorHeartbeat
-> BattleResult
```

It also documents the following IDs for that older build:

```text
EndClientTurn       18688
CancelMatchmake     12269
UdpConnectionInfo   25513
SectorState         21873
SectorHeartbeat     21443
BattleResult        28934
```

The legacy official `B1CAF6D9` catalogue contains none of those message IDs as
codec entries. The only nearby IDs are `18647` and `18685`, whose meanings have
not been established. Consequently, the Astral IDs are excluded from this
build's fact set. The command type `594` belongs inside `EndClientTurn`; it is
not a Piranha message ID.

The older reference representation of a card-placement command is also only a
structural search seed:

```text
command type 51
tick
checksum
user ID high/low
deck index
card SCID high/low
spell index
card level
x
y
```

This shape must be matched against a codec and a live frame in the target
build before any field name is accepted.

## Working model and boundaries

The evidence supports a layered model:

1. Piranha clientbound messages are decoded and dispatched through the
   `0xADBC1C` router.
2. Several nearby messages manipulate matchmaking UI or report matchmaking
   failure, but none yet proves the beginning of a network battle.
3. Legacy message `21631` can create and configure a separate UDP endpoint,
   and the successful path immediately sends empty message `16226` over it.
   This is the strongest same-build evidence for the sector/high-rate
   transport bootstrap.
4. The older reference suggests that matchmaking may issue a logical
   `EndClientTurn` command before a separate battle/sector session is created.
5. The legacy build demonstrably supports switching selected outbound
   messages from the main Piranha stream to a high-rate UDP socket. The current
   runtime's use of that path has not yet been captured.
6. Gameplay commands should be identified from command codecs and live frames,
   not from the loose names of local animation or AutoChess state machines.

## Next experiments

The next pass should proceed in this order:

1. Find the client-to-server matchmaking request, then capture the `110AA2B5`
   clientbound response whose handler creates the UDP socket. Re-run the
   `21631` codec comparison on that exact build before transferring the field
   names.
2. Decode the high-rate UDP transport header from its packet builder and
   reconcile it with a captured datagram, including sequence, acknowledgement,
   fragmentation, message count, and encryption state.
3. Search command codecs for the EndClientTurn header shape: tick/checksum,
   bounded command count, then command-type plus payload.
4. Search sector codecs by structure: connection endpoint/port pair, battle
   identity, participant descriptors, tick/checksum, and bounded command arrays.
5. Inspect the visible tutorial overlay and the runtime checks around
   `tutorial_battles_completed`, `tutorial_state`, `matchmaking_start`,
   `BATTLE_TRAINING`, and `TUTORIAL_BATTLE_PREFER_OLD_TUTORIALS` before
   changing profile state. The exact force-gate or local-training decision has
   not been established.
6. Capture the first new serverbound frame and map every parsed field back to
   its same-build decoder and consumer.
7. Stop before assigning card-command semantics until a command codec and a
   live command agree on the field order and widths.

## Rejected interpretations

- `16513` is not a battle command.
- The `BattleInit`/`Battling` state names at `0x10BBE60` do not identify the
  Clash Royale PvP protocol; that code is AutoChess.
- A displayed matchmaking screen is not evidence that a request reached the
  backend.
- AstralRoyale message IDs and field offsets cannot be copied directly into the
  legacy or current builds.
- Structural similarity alone does not establish a class, message, or command
  identity.

