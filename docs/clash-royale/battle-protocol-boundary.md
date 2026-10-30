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

### Current match-control and event-control candidates

Three current-build message classes were inspected. `26349` and `11865` remain
match-control candidates. `29613` is not one: its nested object is now
identified as event configuration and must be kept out of the match-found
candidate set.

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

The current Dispatcher at `0xAD3DA8` has a `26349` branch. One accepted path
reaches `0xAD6EA4`, where it calls `sub_AD9070`. That function reads a virtual
boolean at incoming-object `+0x150`, sets the application state at `+8` to
either `9` or `4`, performs presentation cleanup, allocates a `0x90`-byte
object, constructs outbound message `11865`, and inserts it with
`sub_AD3AC0`. This proves a conditional `26349 -> 11865` queue relation. It
does not prove that every `26349` produces an acknowledgement or that either
message is named `MatchFound`.

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

This also corrects a proximity assumption: `26349` does not directly call
`sub_BDB444`, and `match_found_start` is not a direct consequence of the
`26349 -> 11865` branch.

### Battle-entry string and structure candidates

Current-build strings and their local xrefs provide the next battle-search
frontier. These are navigation clues until their codecs and callers agree:

| Search term | Address | Current function / role candidate |
| --- | --- | --- |
| `CommandTick` | `0x3120E7` | `sub_D2F37C`, the confirmed LogicCommand scheduler |
| `ServerTick` | `0x31A87B` | `sub_D2F37C` |
| `CurrentTick` | `0x32C009` | `sub_D2F37C` |
| `ExecuteCommand` | `0x33CF64` | `sub_D2F37C` |
| `SectorCommandMessage` | `0x31F25F` | source in `sub_DAA1D4`; sector message candidate |
| `Sectormanager` | `0x32BDB3` | references `0xC88238`, `0xC89B58`, `0xC8AB3C` |
| `Checksums` | `0x2F1A35` | `sub_11549D8`; turn/checksum candidate |
| `Checksum` | `0x324746` | `sub_EC4CD0`, `sub_EC51EC`; local file-report path |
| `ChecksumReport` | `0x338B2C` | `sub_C9942C`; creates `checksum_report.csv`, not wire proof |
| `battleId` | embedded in `sub_D1B804` | field-map constructor with `allianceId`, `players`, `mapItemId`, `battleId`, `spectatorCount` |

`sub_11549D8` is the stronger network-turn checksum lead. The
`Checksum`/`ChecksumReport` file-map functions at `0xEC4CD0`, `0xEC51EC`, and
`0xC9942C` are local diagnostics and are excluded from the wire schema.
`sub_D1B804` is a strong battle/replay object candidate, but its constructor
and xrefs still need to connect it to a current message codec. `sub_C8845C`
prints battle, spectate, replay, and `battle_id` diagnostic data; it is useful
for confirming object relationships, not for naming wire fields by itself.

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
2. Current-build message `16513` is an outbound command-list envelope. The
   traced home-battle click sends command `824` inside it; its queue semantics
   and required server response remain to be established.
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

1. Follow the execute-with-flags-3 observer at `0xD2FE00` from command 824 into
   `16513`, then identify every command scheduler and queue mutation that
   decides why some `16513` encodes have an empty list and the battle click
   produces exactly one command.
2. Identify the controlled server's expected response to a valid command-824
   `16513`. Trace current-build clientbound decode/dispatch until the first
   consumer that starts match-found, loading, arena setup, or high-rate UDP.
   Re-run the `21631` codec comparison on that exact build before transferring
   field names or IDs.
3. Decode the high-rate UDP transport header from its packet builder and
   reconcile it with a captured datagram, including sequence, acknowledgement,
   fragmentation, message count, and encryption state.
4. Search command codecs for the EndClientTurn header shape: tick/checksum,
   bounded command count, then command-type plus payload.
5. Search sector codecs by structure: connection endpoint/port pair, battle
   identity, participant descriptors, tick/checksum, and bounded command arrays.
6. Inspect the visible tutorial overlay and the runtime checks around
   `tutorial_battles_completed`, `tutorial_state`, `matchmaking_start`,
   `BATTLE_TRAINING`, and `TUTORIAL_BATTLE_PREFER_OLD_TUTORIALS` before
   changing profile state. The exact force-gate or local-training decision has
   not been established.
7. Decode the captured `16513` command-824 frame and map every parsed field
   back to its same-build decoder and consumer before processing later socket
   or gameplay messages.
8. Stop before assigning card-command semantics until a command codec and a
   live command agree on the field order and widths.

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
- The local `Checksum` and `ChecksumReport` file-dump functions do not establish
  a network checksum format. `sub_11549D8` is the current wire-side lead that
  still needs encoder/decoder correlation.

