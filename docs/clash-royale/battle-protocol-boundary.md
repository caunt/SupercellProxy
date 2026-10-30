# Clash Royale battle protocol boundary

Research snapshot: 2026-10-31

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
| Null client runtime, derived from a newer official client | `110AA2B5CAC391C498645E072B0D88729428C2C2845E7E2737CA8EE979059783` | `clash-royale-supply-chain/artifacts/extracted/15.535.13-android-a6693e925c082a46/aarch64/libg.so`; isolated runtime analysis uses `libg-null-runtime-110aa2b5` and `runtime-match-110aa2b5-20261030` |

Addresses, message IDs, command IDs, vtables, and object offsets in this note
belong to the legacy `B1CAF6D9` build unless explicitly labeled as a runtime
observation. They must not be copied into the `110AA2B5` build without checking
that build's codecs and consumers. Message IDs may remain stable across nearby
releases, but that is a hypothesis to test rather than a transport rule.

The follow-up static work uses the isolated current-build copy:

```text
D:\IDA Scripts\ida-ai\.ida-ai-cache\battle-research-current-20261030-v2\libg.sqlite
D:\IDA Scripts\ida-ai\.ida-ai-cache\battle-research-current-20261030-v2\libg.i64
```

Its pre-research SHA-256 values were
`55CA64A2AEDC8578E235C46FC1802638D326A2F2EFA026D53CF747510F63D186`
for SQLite and
`20CB073E70D8C0B1A0A019BDE6EC1666F1ED23258FE3B9ACEE0A163CB260C4E5`
for the IDB. These copies prevent additional static exports from mutating the
shared runtime analysis database.

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

### UI state machine versus command queue boundary

The local matchmaking presentation and the network request are separate
boundaries. At `0xA97AE8`, the `match_making_start`,
`matchmaking_start`, `appear_start`, and related strings are passed to local
timeline/UI helpers. This function changes presentation state, but it does not
construct a Piranha message and does not call
`LogicCommandScheduler::enqueue`. A visible "Searching for opponent" screen
therefore cannot be used as evidence that a matchmaking command left the
client.

The first inspected enqueue path after the UI code is `0x93D2C8`. It reads a
controller/global byte at `+0x251`, resolves the string `Friendly`, constructs
a 0x78-byte logic command through `0xD4FAAC`, and finally calls
`LogicCommandScheduler::enqueue`. The constructed command uses dynamic vtable
`off_1885720`. Its relevant slots are:

```text
+0x18  0xD4FB2C   validation/execution-style method
+0x28  0xD502A8   method still to inspect
+0x30  0xD503B4   method still to inspect
+0x38  0xD368D4
+0x40  0xD364CC
+0x48  0xD36818
+0x50  0xD36898
+0x58  0xD36528
```

`0xD4FB2C` is a large validation/execution-style method that reads selected
players/decks, battle data, arena/event/tournament context, and challenge/party
state, and can return a broad set of status codes. The `Friendly` string, size
and vtable establish that this is a real logic-command wrapper. It does not
establish that this is the ordinary home-screen PvP matchmaking request.

The Battle page has at least two construction candidates, `0x96D450` and
`0x9803AC`. `0x9803AC` binds `button_battle`, `button_edit_deck`, and
`button_close`, then registers several listener/vtable regions. The actual
ordinary Battle button callback and its destination command object remain
unresolved. Until that callback is followed to a command constructor and a
same-build serialized frame, the `0x93D2C8` Friendly path must not be promoted
to ranked PvP matchmaking.

### Current-build home-battle request

The newer `110AA2B5` build was analyzed from an isolated copy of its IDB and
SQLite database. Its addresses are independent of the legacy build:

```text
UI_menu_arena constructor             0xBD2E68
PlayArena presenter constructor       0xBD1278
Arena presenter click handler         0xBD5F34
deferred battle action                 0xBD9528
matchmaking transition                 0xBD5A80
local matchmaking screen setup         0xBD5C38
candidate matchmaking command ctor     0xD5C6D0
candidate command execute              0xD5C70C
candidate command codec                0xD5CAAC
```

`0xBD2E68` constructs `UI_menu_arena`, obtains `play_arena`, binds the
`button_battle_mode_switch` elements, and builds a `0xBD1278` presenter.
`0xBD1278` creates an input handler for `play_arena` and links it to the arena
screen callback interface. The click reaches `0xBD5F34`; the normal delayed
action is forwarded through `0xBD9528(..., 20, ...)` to `0xBD5A80`.

The same-build command object created by `0xBD5A80` uses vtable
`off_187C980`. Its `+0x20` getter returns command type `824`, and its `+0x28`
method is the codec at `0xD5CAAC`. After the command-type VInt, the codec order
is:

```text
base LogicCommand fields
  VInt at object +16
  VInt at object +12
  logic-long at object +20
subclass 824 fields
  VInt at object +44
  VInt at object +52
  VInt at object +48
  bool at object +41
  bool at object +40
```

The base codec writes object offsets `+16`, `+12`, then the logic-long at
`+20`. The current symbols do not establish semantic names for the three
subclass integers or two booleans. `0xBD5A80` initializes the second boolean
and the first integer from the selected action, passes a value into the third
integer, and copies the current counter into the second integer. This proves
the shape and producer path, not names such as arena, deck, or `Is2v2`.

`0xD5C70C` is the execute-like command method. Once its preconditions pass, it
uses the boolean at `+41` to select a call at virtual offset `0x190` (`400`) or
`0x5C0` (`1472`) on the object returned by `*(command_context + 256)`, then
stores object `+52` into the game-data object at `+1356`. That call is the
remaining bridge to the outbound Piranha envelope; its dynamic target has not
yet been identified.

The runtime trace closes the transport boundary for this build. A real home
Arena battle click constructed command `824`, executed it with flags `1` and
then `3`, and caused the same command object to be serialized by message
`16513` on the existing TCP connection:

```text
LogicCommand 824 constructor             0xD5C6D0
LogicCommand 824 execute                 0xD5C70C
generic command writer                   0xD2F06C
message 16513 encoder                    0x1051120
message 16513 decoder                    0x10511F0
message 16513 ID getter                  0x10513F0 -> 16513
```

`0xD2F06C(encoder, command)` first calls `command->vtable+0x20` for the command
type, writes that VInt through `encoder->vtable+0x88`, and then calls
`command->vtable+0x28` to encode the command body. This establishes the
command-type tag and body boundary directly.

Message `16513` has the following current-build wire order:

```text
VInt object+0xA0
VInt object+0xA4
VInt command-list count from list+0x0C
repeat command-list count:
  VInt command type
  command base codec
  command subclass codec
blob tail selected by object+0xA8 and object+0xB0
```

The pointer-shaped value at object `+0x90` is not part of this wire schema; it
is the command registry/context used by the decoder.

For the active `play_arena` click, one trace observed these values in order:

```text
message 16513
  +0xA0 = 18665
  +0xA4 = 524288
  command count = 1
  command type = 824
  blob state with empty source and length -1

command 824
  base +0x10 = 18664
  base +0x0C = 18664
  base logic-long at +0x14 = 0
  subclass +0x2C = 0
  subclass +0x30 = -1
  subclass +0x34 = 0
  subclass +0x29 = false
  subclass +0x28 = false
```

The logical bytes after the 32-byte transform prefix were:

```text
a9a302 808040 01 b80c a8a302 a8a302
00000000 7f 00 ffffffff
```

This exact byte stream is an observation for one instance. The field order
comes from both encoder and decoder pseudocode, while the byte-level encoding
of the base logic-long and the two boolean fields still needs a dedicated codec
walk before it should be used to build a general serializer.

The server capture
`conn-000006-seq-221-serverbound-16513.bin` is a 46-byte frame whose header
identifies message `16513`. It proves the controlled server received a
serverbound `16513` during the battle-click experiment. The separate Frida
plaintext trace proves that `16513` instances can carry command `824`; the two
artifacts should not be described as a cryptographic replay of the same frame.

This establishes the home-battle-click request path: 824 is a real command in
the outbound command list, and 16513 is its current transport envelope. Calling
824 specifically "ordinary ranked/1v1 matchmaking" remains a semantic
hypothesis until the server response or a controlled payload mutation shows
which queue or mode that command selects. The command ID, message ID, object
offsets, and field order must not be copied into another build without the same
same-build checks.

The separately named Friendly path was also rechecked in this build.
`0x937B60` resolves `Friendly`, branches to the `0x9F6538` friend-list popup,
and eventually constructs a different command. It must remain separate from
the `play_arena` command type `824`.

### UI and unrelated battle-like code

The following were checked and removed from the network-battle candidate set:

- `0x10BBE60` contains `BattleInit`, `Preparing`, and `Battling` names, but it is
  an AutoChess state machine. Those names do not describe Clash Royale PvP.
- `0x95729C`, `0xA97AE8`, and `0xBDDE80` are matchmaking UI, animation, or
  local state-machine code.
- `0xC92458` and `0xCCE5CC` are battle debug/context output.
- `0x1089460` and `0x11150A4` are small accessors for home battle/card data.
- `0xD888C8`, `0xD8B990`, `0xE191AC`, and `0xE19EE8` consume matchmaking
  configuration. No evidence yet shows that any of them writes a matchmaking
  request to the network.

`29938` is a separate current-build message class with ID getter
`0x1051004`, constructor `0x1050EAC`, and codec `0x1050EE4`. It is present in
the binary, but neither its constructor nor its codec ran during the traced
`play_arena` click. The later same-build Dispatcher review below finds
`TID_DECK_EXPLORER_*` handling in its branch, so it is no longer a leading
battle-response candidate. It is not the observed matchmaking envelope.

## Runtime observation

On the Null client build `110AA2B5`, tapping the yellow home-screen battle
button opened the animated `Searching for opponent` screen. A later instrumented
run recorded the exact command lifecycle below:

```text
LogicCommand 824 constructor                 13:07:01.234
LogicCommand 824 execute flags=1             13:07:02.250
message 16513 with command count 0            13:07:02.252
LogicCommand 824 execute flags=3             13:07:02.258
message 16513 with command 824                13:07:02.260
write command type 824                        13:07:02.261
write command 824 body                        13:07:02.262
outbound frame                                13:07:02.264
```

The near-simultaneous zero-count frame is a separate `16513` encode from the
same scheduler pass. It must not be treated as the command-carrying frame.
The command-carrying `16513` encode contains exactly one item whose type getter
is `0xD5CAA0` and whose body encoder is `0xD5CAAC`.

This corrects two earlier negative observations:

- the local searching-screen transition does not prove that no request was
  sent;
- `16513` is not merely a status/UI notification. It is a command-list envelope
  that can carry the home-battle command.

No `29938` constructor or encode event occurred on this path. Whether the
tutorial state gates the server response or local progression is still
unresolved.

Runtime artifacts:

- `ida-ai/.ida-ai-cache/battle-message-v7.js`
- `ida-ai/.ida-ai-cache/battle-message-v7.jsonl`
- `ida-ai/.ida-ai-cache/battle-message-v7-frame.jsonl`
- `ida-ai/.ida-ai-cache/battle-v7-after-battle-tap.png`
- `ida-ai/.ida-ai-cache/battle-v7-third-click.png`
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
- `nulls-royale-server/tmp/profile-sentinel-20261030/player-info-scr2/capture/conn-000006-seq-221-serverbound-16513.bin`

The final passive run before this note was frozen at:

```text
path      ida-ai/.ida-ai-cache/battle-runtime-passive-20261030.jsonl
size      131151
lines     411
sha256    3DA40910AAC9085B908C98842E83D2921E79DE3F23CC00E972111C0961203E00
```

Its final tick was `19600`. The client remained in matchmaking and emitted 101
`16513` encode events, including 308 TCP frame events, 101 `10108/v1`
keepalives, and two high-level dispatch pairs. No `26349`, `11865`, `21631`,
`20941`, `13584`, `16226`, or UDP bootstrap appeared. The absence of those
messages is a property of this observed server/session state, not proof that
the code paths do not exist.

## Current-build static follow-up

This section records a same-build recheck of the current Null client
`110AA2B5`, separate from the legacy `B1CAF6D9` knowledge base.

### `16513` scheduling and send boundary

The current-build LogicCommand queue scheduler is `0xD2F37C`. It owns a command
list at `+8` and a count at `+0x20`, asks `0x1062BC0` for the current logical
tick, invokes a command through its vtable slot at `+0x18`, and can invoke a
queue callback at `(*(queue + 0x18)) + 0x20` after a successful command. Its
outer logical-tick driver is `0x1062274`.

The runtime timestamps agree with that call structure:

```text
LogicCommand 824 constructor       13:07:01.234Z
message 16513 encode               13:07:02.260Z
outbound command-824 frame         13:07:02.264Z
```

The one-item `16513` is therefore formed during a later logical tick after the
scheduler accepted command 824. This establishes the queue/scheduler boundary
and that the click handler does not directly serialize the frame on its own
call stack. It does not yet establish what `flags=3` means beyond the scheduler
execution mode.

### Current LogicCommand type 2

The current build contains a complete LogicCommand subclass whose type getter
returns `2`. Its main vtable is a bounded 16-slot table:

```text
main vtable       0x1877B20..0x1877BAD
next vtable       0x1877BB0
```

The next vtable must not be included. The following methods agree on the class
boundary and codec:

```text
constructor                         0xD32974
copy from actor                     0xD329A8
execute                             0xD32A08
type getter                         0xD32E94 -> 2
wire encode / decode                0xD32EA0 / 0xD32F10
JSON encode / decode                0xD32D98 / 0xD32E1C
```

The wire codec emits:

```text
base +0x10
base +0x0C
base logic-long +0x14
subclass +0x30
subclass +0x28
subclass +0x34
subclass +0x2C
```

Offsets `+0x28`, `+0x2C`, and `+0x30` are fields of this subclass. The
copy-from-actor method populates them from the input actor:

```text
+0x28 <- actor+0x08
+0x2C <- sub_DC2570(sub_F171B4(actor))
+0x30 <- sub_F2F298(actor)
```

The JSON key for `+0x28` is `cgid`. The execute method resolves actor/card
state, checks ownership, slot/position state, and schedules an actor operation
with an execution tick. This is enough to describe type `2` as a high-confidence
card/actor placement command. AstralRoyale's old `Place card` type `51` is a
useful semantic prior, but its numeric type and serialized fields are not
imported into this build.

### Current `21631` is the UDP bootstrap

The current clientbound factory `0x1026E64` allocates a `0xC0`-byte object for
message `0x547F` (`21631`) and calls constructor `0x106062C`. Its vtable is
`0x18AB1A0`; ID getter `0x1060800` returns `21631`, while version getter
`0x1060808` returns `27`.

Encoder `0x10606CC` and decoder `0x1060734` establish this structural order:

| Object offset | Current structural role |
| --- | --- |
| `+0xB0` | transform/configuration string written first |
| `+0x90` | signed 32-bit endpoint value consumed as the UDP port |
| `+0x98` | endpoint string/address detached and passed to the socket setup |
| `+0xA0` / `+0xA8` | opaque byte buffer and its count |

The Dispatcher branch in `0xAD3DA8` destroys any previous socket, allocates a
`0x6B0` object with `0x1331C24` (the constructor labels it
`"HighRateUdpSocket"`), and calls `0x1331F2C(socket, endpoint_string,
endpoint_i32, byte_blob, ..., transform_object)`. On success it constructs
message `16226` through `0x10605AC` and registers it with the high-rate socket
through `0x13329DC`.

Current `16226` has ID getter `0x10605E8`, version getter `0x10605F0` returning
`1`, and empty encode/decode thunks. This promotes the previous cross-version
prior to a current-build fact: after installing the endpoint and transform,
the client queues empty message `16226` as the first message on the high-rate
UDP path.

The byte blob, transform string, and endpoint string are still structural
names. Their battle/session semantics remain unproven. In particular, do not
call the blob a nonce or the transform string a key until the transform
constructor and UDP packet capture agree.

The ordinary TCP keepalive is a separate current-build message. `10108/v1`
has an empty codec and was observed on the established connection approximately
every five seconds while the client remained on the matchmaking screen. It is
not a battle-state or matchmaking notification.

### `29938` is a deck-explorer response candidate

The current Dispatcher branch for `29938` consumes up to three packed integers:
`+0x90`, `+0xA4`, and an array pointer/count at `+0x98`/`+0xA0`. The first
value selects success versus error handling. The non-success branch writes
`TID_DECK_EXPLORER_ERROR_GETTING_AVAILABLE_CARDS` or
`TID_DECK_EXPLORER_ERROR_GETTING_RESULTS`; the success branch feeds the integer
array into objects rooted around `0xD3F6B4` and `0xD3F6EC`.

This is strong evidence that `29938` belongs to the deck-explorer feature and
must be excluded from the high-confidence matchmaking/battle-response set.
The remaining question is only its exact deck-explorer message semantics, not
whether it starts a battle.

### Existing IDs that do not transfer

ARM64 immediate scans of `110AA2B5` find no current occurrences of the older
cross-version candidates `24696`, `25513`, or `21873`. Current `16226` and
`21631` still exist, but every transferred field must use the current codecs
above. The next matchmaking response must therefore be located by dispatcher
consumer/state transition or a controlled server response, not by guessing
that an old ID survived.

### Current matchmaking, loading-control, and event-control candidates

Three current-build message classes were inspected. `26349` and `11865` are
loading/application-control messages, not confirmed match-found messages.
`29613` is not one: its nested object is now identified as event configuration
and must be kept out of the match-found candidate set.

```text
26349 / v16
  constructor       0x105E870
  vtable            0x18AAED0
  encode / decode   0x105E8A4 / 0x105E8A8 (both empty)
  ID / version      0x105E8AC / 0x105E8B4

11865 / v27
  constructor       0x105E8F0
  vtable            0x18AAF20
  encode / decode   0x105E924 / 0x105E928 (both empty)
  ID / version      0x105E92C / 0x105E934

29613 / v9
  constructor       0x1041220
  vtable            0x18A7110
  encode / decode   0x10412D8 / 0x1041388
  ID / version      0x10414AC / 0x10414B4
```

The current Dispatcher at `0xAD3DA8` has a conditional `26349` branch. It first
checks the controller byte at `+0x664` and the battle/context pointer at
`+0x424`. When either readiness check selects the existing-state path, control
goes through the controller virtual slot at `+448` or `sub_BDBB2C` and does not
call `sub_AD9070`. The loader call is reached only when those checks and
`sub_11FF54C` do not select the existing-state branch.

Once called, `sub_AD9070` reads a virtual boolean at incoming-object `+0x150`,
sets the application state at `+8` to either `9` or `4`, performs presentation
cleanup, forwards the loading controller at `+0x80` to `SectorManager`,
allocates a `0x90`-byte object, constructs outbound message `11865`, and inserts
it with `sub_AD3AC0`. The branch therefore has at least two state-dependent
outcomes; `26349 -> AD9070` is not an unconditional battle-loader edge.

The controller field read on the `26349` path at `+0x298` is sourced from the
configuration key `CACHE_MAX_EXTRA_SC_FILES`; it is not a battle-state field.
Together with the loading/SectorManager calls, this places `26349` on the
loading/application-controller branch. Calling either current `26349` or
`11865` `MatchFound` would therefore be unsupported.

### Clan-War controllers are not ordinary PvP matchmaking evidence

Several nearby controller constructors contain matchmaking-themed UI code but
belong to Clan/Social features. They must be kept separate from the ordinary
home-screen PvP response:

```text
94D728       Clan-War map presentation controller
968844       large Clan-War map controller
C2FDBC       facade that constructs the Clan-War controller family
C2FF3C       mode/state slot updater for that family
C31824       inserts mode-specific child controllers, including 94D728
```

`94D728` installs vtable `0x180E740` and constructs the
`war_map_params_cw2` configuration, `water_bg`, `screen_boat`, three
`defense%d` slots, and clan/opponent presentation children. `968844` installs
vtable `0x18114B0` and independently constructs the same war-map, boat, water,
defense, opponent, and defense-editor UI.

`C2FDBC` constructs `952C78`, whose body imports
`panel_battleSpells_clan_war`, deck UI, and mode-specific Clan-War state.
`C2FF3C` refreshes the controller's mode slots, and `C31824` constructs
`94D728` for slot 2. Direct callers of the `C322F0`/`C32468` facade include
`94F870`, `96C59C`, and `96C610`; the latter two are Clan-War controller
methods. These relations reject the earlier broad claim that `94D728` and
`C2FDBC` are ordinary matchmaking/loading controllers.

The separate `951B34`/`94D384` family remains a specialized matchmaking UI
candidate. `951B34` contains `shield_2v2`,
`matchmaking_shield_clan_wars`, `match_making_ladder`, `play_arena`, and
`TID_MATCHMAKE_FAILED`. `94D384` calls `951B34(a1, 1, 0)` and installs vtable
`0x180E548`; `C32468` is one constructor that can create it after a profile
predicate, otherwise showing `TID_MATCHMAKE_FAILED`. That presentation family
may cover ladder, 2v2, or Clan-War matchmaking, but it contains no proved
Piranha message constructor. Its presence is not a network MatchFound signal.

The `29613` Dispatcher case at `0xAD4958` loads one of its optional values and
reaches `sub_BDB444` at `0xAD497C`. Its top-level encoder/decoder order is:

```text
nullable packed-VInt object at +0x98
nullable 0xA8-byte nested object at +0xA0
nullable packed-VInt object at +0x90
```

The nested helper is initialized by `0xEBFD50` and encoded/decoded by
`0xEC00C8` / `0xEC0440`. Both codec directions establish this structural
order:

```text
i32 x3
string
i32 x5
string x3
conditional packed array
i32
bool
i32 x4
bool x2
i32
i32
bool
optional byte length + byte payload
```

The nested helper is an event configuration rather than a battle-start record.
Function `0xEC0A40` creates its field keys, while encoder `0xEC00C8` and
decoder `0xEC0440` write and read the same object offsets:

| Offset | Field |
| --- | --- |
| `+0x00` | `id` (i32) |
| `+0x04` | `version_id` |
| `+0x08` | `variant_id` |
| `+0x10` | `name` |
| `+0x20` | `category` |
| `+0x24` | `start_time` |
| `+0x28` | `end_time` |
| `+0x2C` | `visible_time` |
| `+0x30` | `visible_end_time` |
| `+0x38` | `start_notif_tid` |
| `+0x48` | `end_notif_tid` |
| `+0x58` | `event_name` |
| `+0x68` | optional `json` string pointer |
| `+0x78` | `targeting_id` |
| `+0x7C` | `personally_targeted` |
| `+0x80` | `scheduling_type` |
| `+0x84` | empty-string key |
| `+0x88` | `event_top_group_id` |
| `+0x8C` | `impression_id` |
| `+0x90` | `scstore_exclusive` |
| `+0x92` | `enabled` |
| `+0x94` | `num_variants` |

The time meanings have independent arithmetic support: `0xEC0C44` and
`0xEC0C64` multiply `+0x24` and `+0x28` by `1000`, `0xEC0C9C` subtracts
`+0x24` from `+0x28` to produce a duration, and `0xEC0CA8` returns the optional
JSON pointer. This is high-confidence event schema evidence.

`sub_BDB444` compares an identity through `0x121A9F0`, reads the event status
through `0xEC0C24`, and tests it against `20`. The matching branch calls the
general UI transition `sub_BD5C38(controller, 16, event_id, 1, ...)`.
`match_found_start` and `team_found_start` are generic presentation strings in
an alternate transition path; they do not prove that `29613` itself is a
MatchFound or TeamFound message. The earlier "MatchFound/TeamFound consumer"
interpretation is therefore rejected.

### Controlled `26349` experiment and crash boundary

A runtime probe repeated the real home battle click and recorded the following
current-build sequence:

```text
natural 16513 command list       command type 798
home battle 16513 command list   command type 824
                                 base ticks 188 / 188
inject 26349/v16
dispatch(26349)                  result 1
construct 11865/v27              vtable 0x18AAF20
send 11865                       Piranha frame ID 0x2E59
Mainloop SIGTRAP                 libg+0x11C2F24
```

This turns the static `26349 -> 11865` relation into an observed runtime edge
for this test instance. Both message codecs are empty, so the experiment did
not fabricate or parse a payload. It also demonstrates that the response is not
only queued: the client sends it immediately.

The process then crashes with build ID
`90e6f351f0dec4a28b494c3812d2629fd45d5a83`; therefore an empty `26349` is not
sufficient to enter a battle. The libg backtrace includes:

```text
0x11C2F24
0x995510
0x99559C
0xBBD8A0
0xC85EC4
0xC868F4
0xC8DDDC
0xC8DA70
0x759F28
```

The static overlay places `0x11C2F24` at the return edge of the destructor
`sub_11C2EFC`, reached from a virtual destructor call at `0x995510`. The same
object-cleanup path calls the loading controller tick `sub_C8DA38`; that tick
can send `13584` when flags `+0x116` and `+0x117` transition. This supports a
cleanup/loading contract violation in the synthetic branch, but does not yet
identify the missing member. It is not evidence that the empty payload failed
to parse.

The next runtime pass must stay passive on a fresh client session and capture
the normal messages and object setup after the real command-824 request. Until
that sequence exists, do not name `26349` or `11865` `MatchFound`, and do not
treat the synthetic exchange as a complete handshake.

This also corrects a proximity assumption: `26349` does not directly call
`sub_BDB444`, and `match_found_start` is not a direct consequence of the
`26349 -> 11865` branch.

### Current stage-3 preloading controller

The current loading controller `sub_C90A4C` is a stage-3 ordinary-PvP
preloading path. It waits for login/application readiness, advances to stage 4,
initializes arena/battle data, and then selects one of two outbound setup
messages by a branch:

```text
controller has the optional match/common context
  -> construct 10500/v9 from arena, battle, and a "FirstMatch" data lookup
otherwise
  -> construct 14300/v9 with fields initialized to -1 / -1
```

`FirstMatch` is only a data lookup key in this code. Its presence does not prove
that the tutorial or a particular battle mode was selected.

Current `14300/v9` is constructed at `0x1053A44`; its encoder, decoder, and ID
getters are `0x1053AA0`, `0x1053AF0`, `0x1053B4C`, and `0x1053B54`.
Its top-level wire order is:

```text
nullable/int-like field from +0xA4
nested tagged object from +0x98
nullable/int-like field from +0xA0
```

A captured body `7f 01 12 a8 80 7f` decodes to the observed logical sequence
`-1, 18000000, -1`. The current `14300` therefore cannot be treated as the
older reference's one-field `StopHomeLogic` message.

Current `10500/v9` is constructed at `0x1053D84`; its encoder, decoder, and ID
getters are `0x1053E60`, `0x1053ECC`, `0x1053F64`, and `0x1053F6C`.
Its top-level wire order is:

```text
i32 +0xB8
tagged nested object +0xA8
tagged nested object +0x98
tagged nested object +0x90
tagged nested object +0xA0
tagged nested object +0xB0
```

The nested objects contain arrays and containers. This is a complex setup
message, not the older reference's single packed-integer `10500`.

Current `19314/v9` is a compact integrity/feature report candidate, not the
previously suggested sector setup message. Constructor `0x1051534`, encoder
`0x1051574`, decoder `0x10515E0`, ID getter `0x1051654`, and version getter
`0x105165C` establish this wire order:

```text
VInt +0x98
VInt +0x9C
VInt +0x94
bool +0x90
```

`sub_BB2468` fills `+0x94` from the packed-feature builder
`loc_118296C`, `+0x98` from the accessor of `dword_1998934`, and `+0x9C`
from the accessor of `dword_1999680`. The encoder then writes them in the
order `+0x98, +0x9C, +0x94`, followed by bool `+0x90`.

The two backing globals expose integrity state rather than battle state:

- `sub_7754A4` computes `dword_1998934` as a 32-bit FNV-1a-style hash over
  selected in-memory ranges and a byte-classification result.
- `sub_778F30` maps a file as a ZIP archive, hashes selected entries, compares
  them with embedded signatures, and stores `dword_1999680` as `0`, `1`, or
  `2`.
- `loc_118296C` packs several global configuration/feature values into a
  bit vector and folds in flags that equal `1`.

`sub_B19A80` is an alternate chunk entry that sends the same message class
with only `+0x94` populated and tracks its own controller byte at `+0x250`.
`sub_BB2468` populates all three fields and tracks a separate byte at `+0x369`.
The strongest current interpretation is therefore a client
integrity/feature-report message emitted around battle activity. The historical
class name remains unresolved; `RequestSectorState` is no longer supported by
the field semantics.

### Battle-entry string and structure candidates

Current-build strings and their local xrefs provide the next battle-search
frontier. These are navigation clues until their codecs and callers agree:

| Search term | Address | Current function / role candidate |
| --- | --- | --- |
| `CommandTick` | `0x3120E7` | `sub_D2F37C`, the confirmed LogicCommand scheduler |
| `ServerTick` | `0x31A87B` | `sub_D2F37C` |
| `CurrentTick` | `0x32C009` | `sub_D2F37C` |
| `ExecuteCommand` | `0x33CF64` | `sub_D2F37C` |
| `SectorCommandMessage` | `0x31F25F` | read as one entry by the large global configuration loader `sub_DAA1D4`; not a message codec |
| `Sectormanager` | `0x32BDB3` | references `0xC88238`, `0xC89B58`, `0xC8AB3C` |
| `Checksums` | `0x2F1A35` | one key in the replay/JSON schema initializer `sub_11549D8`; not the network checksum codec |
| `Checksum` | `0x324746` | `sub_EC4CD0`, `sub_EC51EC`; local file-report path |
| `ChecksumReport` | `0x338B2C` | `sub_C9942C`; creates `checksum_report.csv`, not wire proof |
| `battleId` | embedded in `sub_D1B804` | field-map constructor with `allianceId`, `players`, `mapItemId`, `battleId`, `spectatorCount` |

The first two rows were string-proximity false leads. `sub_11549D8` initializes
the replay/JSON keys `battle`, `endTick`, `checksums`, `cmd`, `rndSeed`, `evt`,
`time`, `deck`, `avatar`, `location`, `NPC`, `arena`, `tournament`, and related
fields. Its caller chain runs through `sub_1153780` to `sub_12173D0`, and the
reached code contains the explicit diagnostics
`cannot find battle object from replay json` and
`cannot find cmd array from replay json`. `sub_D1B804` has the same kind of
field-map shape. Neither function is a current network turn or sector codec.

`sub_DAA1D4` is a large global configuration loader. The embedded
`SECTOR_COMMAND_MESSAGE_INTERVAL_SECONDS` string is read into one configuration
field and supports an interval decision; it does not identify the enclosing
function as a message codec. The `Checksum`/`ChecksumReport` file-map functions
at `0xEC4CD0`, `0xEC51EC`, and `0xC9942C` are local diagnostics and are also
excluded from the wire schema. `sub_C8845C` prints battle, spectate, replay, and
`battle_id` diagnostic data; it is useful for confirming object relationships,
not for naming wire fields by itself.

### Current command and sector envelopes

The same-build codecs and consumers now separate five message roles. IDs and
versions below belong only to the current `110AA2B5` build.

| Message | Direction and evidence | Current wire structure | Current role |
| --- | --- | --- | --- |
| `16513/v9` | client to server; `sub_C89890` constructs it and `sub_AD3AC0` queues it | tick, checksum, command count, command list, optional tail blob | general command-list envelope; structurally the current `EndClientTurn`-like form and the proven carrier for home command `824` |
| `18193/v27` | client to server; `sub_C89778` constructs it and `sub_AD3A14` prefers the battle UDP socket | checksum, tick, optional single command, integrity bits | real-time battle command envelope |
| `20941/v16` | server to client; Dispatcher branch `0x51CD` reaches `sub_C89B20` | server tick, checksum, command count, command list, optional raw payload | authoritative battle state/turn batch |
| `13584/v9` | client to server; `sub_C89F28` sends it from the loading-state controller | current tick, secondary battle counter, command count, command list | loading-state synchronization or sector-state request candidate |
| `27679/v9` | server to client; Dispatcher branch `0x6C1F` extracts its command and calls `sub_AD9658` | optional single LogicCommand | generic server-command delivery; not yet proven battle specific |
| `14300/v9` | client to server; fallback branch in `sub_C90A4C` | nullable/int-like, tagged object, nullable/int-like | stage-3 preloading setup message; exact class unresolved |
| `10500/v9` | client to server; common-context branch in `sub_C90A4C` | i32 plus five tagged nested objects/containers | complex stage-3 preloading setup message; exact class unresolved |
| `19314/v9` | client to server; `sub_B19A80` and `sub_BB2468` | three VInts and one bool | integrity/feature report candidate emitted around battle activity |

For `16513`, `0x1051110` writes object `+0xA0` from the current logical tick and
`0x1051118` writes `+0xA4` from `sub_1062BD0`, the battle state value. The
encoder then emits the tick/state pair, list count, commands, and optional blob.
This confirms a renamed EndClientTurn structure, although the old reference's
fixed eight-checksum tail has been replaced by a variable optional blob.

For `18193`, `sub_105E268` allocates a base object and `sub_105E2A8` encodes:

```text
VInt +0xA0  battle state/checksum
VInt +0xA4  logical tick
optional command at +0x98
VInt +0xA8  integrity bits
```

`sub_C89778` fills `+0xA0` with `sub_1062C3C`, `+0xA4` with the current tick,
and `+0x98` with the command. `sub_778164` supplies `+0xA8` and logs
`IntegrityBits` as `tick=%d bits=0x%03x`. `sub_AD3A14` first checks the current
battle's HighRateUdpSocket at `+0x98`; when available it passes the packet to
`0x13329DC`, otherwise it falls back to the ordinary Piranha send queue.

The integrity value is not a conventional CRC:

- `sub_77805C` walks a registration map and builds a packed bit vector from
  entries whose byte at `+0x18` is false.
- `sub_778164` is a stateful 10-bit chunk reader. It emits `1023` when a run
  starts, emits packed 10-bit values while the vector remains, uses `1022` as
  the end marker, and restarts on the following call.
- `sub_C89778` places the current reader value in the packet. The exact meaning
  of the registered entries and the anti-tamper policy remain unresolved.

For `20941`, encoder `0x105E58C` and decoder `0x105E640` prove the field order.
The consumer is unusually specific. `sub_C89B20` accepts it only when battle
controller stage `+0x88` is one of the final two stages. `sub_CCE804` then:

```text
server_tick = 10 * message.+0x9C
state_value = message.+0x98
command list = message list at +0xA0
raw payload = message.+0xB0 / length +0xB8
```

It calls `sub_1064A08(battle, server_tick, state_value, message)`. That function
compares the state value at the same tick, queues commands, stores both value
streams, and can rewind battle execution when a later value for an existing
tick differs. It keeps bounded histories of 141 ticks and limits the blob to
900,000 bytes. This makes `20941` a high-confidence authoritative state/turn
batch with rollback support, not a UI notification.

For `13584`, `sub_C8DA38` invokes `sub_C89F28` when loading-state flags `+0x116`
and `+0x117` transition together. `sub_C89F28` sets the current tick and the
value returned by `sub_1062BD0`, then sends through the ordinary queue. Its
exact historical class name (`RequestSectorState`, `SectorHeartbeat`, or a
newer sector-state request) is still a hypothesis.

For `27679`, Dispatcher branch `0x6C1F` detaches the decoded command and calls
`sub_AD9658`, which either forwards it to the active battle controller or puts
it in the pending-command queue. Its shape supports a generic
`AvailableServerCommand` role, but no current field-key or battle-only caller
has closed that name.

Battle-state names also need control-flow support. `sub_CC3EB0` contains
`BattleStart`, `sub_BB1CC0` contains `PvpBattleStart`, `sub_BB2D08` contains
`BattleEnd`, `sub_EF0C7C` contains `BattleResult`, and `sub_DC79A4` contains
logic-battle strings. Those strings are search seeds because the binary uses
the same words in unrelated state machines and debug code.

### Current UDP envelope and transform work

The current high-rate packet builder provides a second same-build view of the
transport envelope. `0x122128C` writes:

```text
u8    object+0x98      type/flag
VInt  object+0x90      message/type ID
u8    object+0x99      fragment index
u8    object+0x9A      fragment count
VInt  object+0x94      payload length
bytes object+0xA0      payload
```

`0x13334F0` copies the pending 32-bit sequence list, appends queued application
messages, selects an optional 16-bit leading sequence, and invokes the datagram
writer `0x1332818`. Sequence values are clamped to `65000`; `0xFFFF` is a
sentinel. The builder uses `1456` bytes normally and `1448` bytes when its
configuration byte at `+324` is set.

`0x1336A5C` writes four special VInt markers in distinct nonzero-counter
branches:

```text
0xFFFFFFFE
0xFFFFFFFD
0xFFFFFFFC
0xFFFFFFFB
```

The counters at `+8`, `+12`, `+20`, and `+24` select which marker is written.
The object also carries a 32-bit sequence list and a vector of queued messages.
These values are structurally associated with continuation, retransmission,
acknowledgement, or slot selection, but their protocol names are not yet
proved.

The stream vtable `0x18BE1B8` resolves the ambiguous write widths used by that
builder:

```text
+0x50  0x1212BB8  u32 big-endian helper
+0x80  0x1212C84  u16 big-endian helper
+0x88  0x12134FC  VInt
+0x90  0x12139D0  additional VInt/stream helper
```

Current helper `0x1334E44` concatenates two key strings and implements the
standard RC4 key-scheduling algorithm over a 256-byte permutation. Its KSA
shape is confirmed. The separate `0x1335154` initializer copies two 32-byte
state regions, stores two integer parameters, fills a 256-byte buffer using
`0x121B188`, and calls `0x1335244`. Neither the exact place where this state is
applied to a datagram nor the existence and coverage of a MAC is established
yet. It is therefore accurate to say "RC4-shaped KSA in the transport module",
not "the complete UDP payload is RC4 encrypted".

The next byte-level checkpoint is to reproduce the output of `0x1336A5C` and
compare it with a captured datagram before and after the `0x1335244` transform.
That comparison will assign clear/encrypted/authenticated regions and close the
meaning of the four special markers.

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
2. Current-build message `16513` is an outbound EndClientTurn-like
   command-list envelope. The traced home-battle click sends command `824`
   inside it. Current `18193/v27` carries one real-time battle command and
   prefers the configured UDP socket; current `20941/v16` is the corresponding
   server-side state/turn batch with checksum-based rollback support.
3. Current-build message `21631/v27` creates and configures a separate UDP
   endpoint,
   and the successful path immediately sends empty message `16226` over it.
   This is the strongest current-build evidence for the sector/high-rate
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

1. Correlate `16513`, `18193`, `20941`, and `13584` with real captured
   datagrams. Prove which command classes use the general turn envelope and
   which use the UDP-preferred single-command envelope.
2. Identify the controlled server's expected response to a valid command-824
   `16513`. Trace current-build clientbound decode/dispatch until the first
   consumer that starts match-found, loading, arena setup, or high-rate UDP.
   The `21631/v27` codec has already been rechecked on this build.
3. Decode the integrity-bit registration map and prove what policy or feature
   state the `1022`/`1023` delimiters and 10-bit chunks attest.
4. Decode the high-rate UDP transport header from its packet builder and
   reconcile it with a captured datagram, including sequence, acknowledgement,
   fragmentation, message count, and encryption state.
5. Prove the semantic names of the `1062BD0`, `1062C3C`, and `F9E1A0` state
   values and identify the two loading-stage flags that trigger `13584`.
6. Inspect the visible tutorial overlay and the runtime checks around
   `tutorial_battles_completed`, `tutorial_state`, `matchmaking_start`,
   `BATTLE_TRAINING`, and `TUTORIAL_BATTLE_PREFER_OLD_TUTORIALS` before
   changing profile state. The exact force-gate or local-training decision has
   not been established.
7. On a fresh session, keep the probe passive and capture the normal
   clientbound sequence after the real `16513`/command-824 request. Resolve the
   `0x9954B4`/`0x11C2EFC` object-cleanup contract before any further synthetic
   battle-control injection.
8. Decode the captured `16513` command-824 frame and map every parsed field
   back to its same-build decoder and consumer before processing later socket
   or gameplay messages.
9. Correlate current LogicCommand type `2` with a live in-battle frame. The
   static actor/card placement evidence is strong, but no current wire frame
   has yet proved which of the two envelopes carries it during gameplay.
10. Stop before assigning card-command semantics to other command types until
    each command codec and live frame agree on the field order and widths.

## Rejected interpretations

- `16513` is not merely a status/UI-only message; the current build uses it as
  the outbound command-list envelope on the traced battle-click path.
- `29938` was the message that carried this click's command. The class exists,
  but it did not run in the decisive trace.
- The `BattleInit`/`Battling` state names at `0x10BBE60` do not identify the
  Clash Royale PvP protocol; that code is AutoChess.
- A displayed matchmaking screen is not evidence that a request reached the
  backend.
- AstralRoyale message IDs and field offsets cannot be copied directly into the
  legacy or current builds.
- Structural similarity alone does not establish a class, message, or command
  identity.
- `29613` is not a confirmed MatchFound or TeamFound consumer. Its nested object
  is an event configuration with field keys, event times, targeting, and
  presentation metadata; the match/team strings belong to a generic UI
  transition reached from that path.
- The earlier `94D728`/`C2FDBC` matchmaking-controller hypothesis is rejected.
  Those constructors and their immediate callers belong to Clan-War or
  Clan/Social presentation. `951B34`/`94D384` remain a broader specialized
  matchmaking UI family whose exact mode and network trigger are unresolved.
- An empty `26349/v16` is not a valid battle bootstrap. It produces the
  `11865/v27` reply in the tested instance, then reaches a reproducible
  loading/cleanup SIGTRAP.
- The local `Checksum` and `ChecksumReport` file-dump functions do not establish
  a network checksum format. `sub_11549D8` is a replay/JSON schema initializer,
  not the turn-checksum codec.
- `sub_DAA1D4` is a global configuration loader. Its
  `SECTOR_COMMAND_MESSAGE_INTERVAL_SECONDS` entry does not make the containing
  function a SectorCommandMessage codec.
- `sub_D1B804` is a field-map initializer for data involving `allianceId`,
  `players`, `mapItemId`, `battleId`, and `spectatorCount`; it is not yet a
  network message.

