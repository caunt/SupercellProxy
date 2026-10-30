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

A second narrow current-build trace exercised a different Arena state and
closed the direct-command branch:

```text
clicked object == ArenaPresenter+0x130 mode-manager+0x28 (slot 5)
ArenaPresenter pending byte +0x428: 0 -> 1
deferred action presented to BD9528: action=1, value=0, flag=-1
BD5A80 arguments: state=0, selected_id=-1, flag=0
824 execute: flags=1
824 execute: flags=3
empty 16513 heartbeat
one-command 16513 containing the same 824 pointer
local "searching for opponent" UI
later 16513 frames: command count 0
```

The `BD5A80` call initialized the observed 824 instance with `+0x2C=0`,
`+0x30=-1`, and `+0x34=0`. Both execute passes retained those values while the
base `+0x0C` counter advanced from `-1` to `19910`; `+0x10` was `19910` in both
passes. This directly demonstrates that 798 is not a mandatory predecessor:
the earlier v2 run emitted 798 action `41` before 824 when its selection state
differed, while this v3 run constructed 824 directly.

The stable normalized v3 artifact is
`battle-798-direct-824-runtime-v3-20261031.json`, SHA-256
`2033BDAB13E5226D990C6A033FEA248B3E27A14F9599C6F9230800DF10012DF5`.
The run instrumented only command 824 and message 16513, so it does not prove
the absence of other traffic. It also captured no natural clientbound response
that reached match-found, loading, or high-rate UDP. Command 824 is therefore
the PlayArena battle-entry request candidate, while the selected queue and
the server acknowledgement remain unresolved.

A later v4 pass instrumented the current-build Dispatcher, the `23592/v10`
decoder, the controller setters, registry selection, active-match float setter,
the Arena and match controller transitions, match-found presentation, and the
`16513` encoder. One natural transition again produced:

```text
BD9528 return address libg+0xBD975C
BD5A80(state=0, selected_id=-1, flag=0)
controller state: -1 -> 0, started=1, float_0x2BC=-1
16513 tick=29735, state=524288, command count=0
16513 tick=29736, state=524288, command count=1
  command 824
    +0x0C=29735, +0x10=29735
    +0x14=0, +0x28=0, +0x2C=0, +0x30=-1, +0x34=0
```

The request was followed by periodic empty `16513` frames through tick 30400.
Within the bounded v4 capture there was no hooked `23592/v10`, `21631/v27`,
`20941/v16`, `13584/v9`, `16226/v1`, UDP bootstrap, or natural match-found
presentation. These messages are therefore absent from this observed session
window, not proved absent from the client or server.

The tap was delivered within the visible red "cancel" control bounds, yet the
instrumented native path executed another `824` request. That control mapping
must remain unresolved: screen coordinates alone do not prove which native
action the app associated with the press.

The frozen normalized v4 artifact is
`battle-clientbound-v4-observation-20261031.json`, SHA-256
`B34493A21F8B5B408988A29A9AF7CA4AC05F85C13D8CE7F22A91B4C88693F071`.
Its raw JSONL, Frida script, runner, and screenshot hashes are pinned inside the
artifact.

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
queue callback at offset `+0x20` of the installed callback object after a
successful command. Its
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

### LogicCommand routing and queue checkpoint

The current `C893D0(controller, &command, flag)` path has two distinct network
outcomes. This corrects the earlier shorthand that every type-2 command is
unconditionally sent as `18193`.

When the command has already been executed while the battle context is in the
requisite state, the function appends it to the pending array at
`controller+32` and returns. On the normal path it obtains the current tick from
`battle_context+96`, applies a minimum correction of one, lets the command
initialize its tick when `command+20` is empty, and sets the command tick
through virtual slot `+64`.

The subsequent route at `1062F6C(battle_context)` is split:

```text
1062F6C(battle_context) == false
  execute the command as needed
  C89778(controller, command, tick, state)
  construct and send 18193/v27

1062F6C(battle_context) == true
  execute the command as needed
  D2EEF0(battle_context+56, &command)
  append the command to the battle command queue
```

`D2EEF0(queue, &command)` is an append operation, not a sender. It destroys the
incoming command when `battle_context+36 == 2`; otherwise it appends to the
array at `queue+8` and increments the count at `queue+20`.

`D2F37C` consumes that queue by tick. It compares the current and command ticks,
adjusts commands that are at least 21 ticks away, and executes a command at the
20-tick boundary through virtual slot `+24`. Its callback behavior depends on
the active Sector substate:

```text
Sector mode false:
  execute local queued commands
  no controller-listener append

Sector mode true, battle_context+168 -> state == 3:
  execute the authored/replayed commands
  invoke installed callback vtable +0x20 (nullstub)

Sector mode true, other substate:
  execute local queued commands
  invoke installed callback vtable +0x18
  -> 7F5250(listener, command)
  -> append to controller+136 command list
```

The final branch is explicit at `D2FE34`-`D2FE48`: it loads
`queue+0x18`, read from `[queue] + 0x18`, and calls it with the successfully
executed command. The `D2F8A0` path seen earlier uses `[queue] + 0x20` and is a
different branch. The outer logical-tick path is also current-build fact:

```text
C890BC(controller, dt)
  -> 10620A4(controller+144)
  -> 1062274(battle_context)
  -> D2F37C(...)
```

`C86930` constructs the controller-side listener object with
`7F5098`, stores it at `controller+136`, and attaches it to the battle command
queue with:

```text
D2F330(controller+144 + 56, controller+136)
```

Within `D2F330`, the queue's callback object at `+24` is replaced by this
listener. This closes the former "producer unresolved" boundary at the object
level: the listener is both held by the controller and installed on the
`battle_context+56` queue.

The same routing function adds 20 ticks for command types `2`, `64`, `86`, and
`95` when `battle_context+168` is nonzero:

```text
if battle_context+168 != 0
and command_type in {2, 64, 86, 95}
    command_tick = current_tick + 20
```

The adjustment is a code fact. Whether it represents deployment latency,
interpolation, schedulable lead time, or execution ordering remains a
hypothesis.

`C88BBC` is the controller tick that flushes `controller+32`, invokes `C893D0`
for each pending command, and clears the pending count. With a live battle
object, it also constructs an optional periodic `18193` state update. There is
therefore a second `18193` path with no command payload.

`C890BC` also schedules the `16513` drain: when `controller+460` is set, it calls
`C89890` after the battle tick and clears the flag. `C89890` constructs the
message only when controller state is one of `1..4`, loading flag `+116` is
false, and the current tick exceeds `controller+192`. It writes the current
tick and state, detaches the listener's accumulated list with
`7F52D4(&snapshot, controller+136)`, passes that snapshot to `16513`, optionally
attaches the tail from `controller+232`, sends through the ordinary queue, and
advances `controller+192`.

The listener method `7F5250` appends an item to the listener's dynamic list.
The current-build vtable read at `0x17F3008` maps as follows:

```text
vtable +0x00  7F50D4
vtable +0x08  7F50E4
vtable +0x10  7F50E8
vtable +0x18  7F5250   append(listener, item)
vtable +0x20  7F5324   nullsub_336 (single RET)
```

The authoritative/replay `D2F37C` branch uses the empty callback:

```text
D2F8A0  LDR X0, [X19,#0x18]   ; installed queue callback object
D2F8A8  LDR X8, [X0]          ; callback vtable
D2F8AC  MOV X1, X28
D2F8B0  LDR X8, [X8,#0x20]    ; callback slot
D2F8B4  BLR X8
```

The live local-command branch uses the other callback:

```text
D2FE34  LDR X0, [X19,#0x18]   ; installed queue callback object
D2FE38  STR X21, [...]
D2FE40  LDR X8, [X0]          ; callback vtable
D2FE44  LDR X8, [X8,#0x18]    ; append slot
D2FE48  BLR X8                ; 7F5250(listener, command)
```

This closes the producer that was previously open. `C89890` detaches and
encodes the list held by `controller+136`; `D2F37C` appends successful live
local commands to that list. The active battle path can therefore coalesce
commands for `16513` rather than sending each one as `18193`.

The periodic `18193` call has independent support. Global configuration
function `DAA1D4` reads
`SECTOR_COMMAND_MESSAGE_INTERVAL_SECONDS` into configuration `+576`, and
`C88BBC` compares the current tick against
`controller+360 + 20 * configuration+576`. When the interval has elapsed, it
calls `C89778` with a null command at `C88D64`. Thus `C89778` is used for both:

- an immediate single-command `18193` on the `1062F6C == false` path;
- a periodic Sector state update with a null command on the active battle path.

For the live type-2 gameplay question, two statements are now separated:

- `18193/v27` is a single-command or periodic Sector-state envelope. In the
  active Sector queue path it is not the carrier of every executed local
  action.
- `16513/v9` is the batched carrier for commands accumulated by the
  controller listener. `C893D0` sets `controller+460` when a live battle object
  exists, `C890BC` observes that flag, and `C89890` drains the listener into
  `16513`.
- `20941/v16` is the server-to-client counterpart that places authoritative
  commands into the same `battle_context+56` queue.

This still needs one runtime frame to prove the nominated first live action
makes the predicted `16513` rather than the alternate `18193` path. The static
control flow now favors the former for the stage-4 local Sector queue.

### Loading gate handshake around `29303` and `20129`

Two same-build empty clientbound messages now connect the stage-3 loading
controller to the pending `13584` send.

`29303/v9` is allocated as a `0x90`-byte object by factory case `0x7277`.
Its ID and version getters are `0x1053FE4` and `0x1053FEC`; its codec slots are
empty. The current Dispatcher branch is:

```text
message ID == 29303
  -> C8C5B8()                       ; loading-controller singleton
  -> C8E1BC(controller, 1)
       controller+0x74 = 1
       controller+0x75 = 1
```

The stage-3 preloading function `C90A4C` follows a different setup path:

```text
C8E1D0(loading_controller, 1)
  loading_controller+0x74 = 1
  loading_controller+0x75 = 0
  -> construct and enqueue 10500/v9 or 14300/v9
```

`C8DA38` later requires both bytes. When both are nonzero, it clears `+0x75`,
obtains the active battle controller, and calls `C89F28` to construct
`13584/v9`. This yields a strongly supported normal-flow hypothesis:

```text
stage-3 preloading
  -> initialize +0x74 and clear +0x75
  -> send 10500/v9 or 14300/v9
  -> receive 29303/v9
  -> set +0x74 and +0x75
  -> next loading tick sends 13584/v9
```

`29303/v9` is therefore a loading-stage acknowledgement/readiness candidate.
The current evidence does not prove its historical class name, and it must not
be named `MatchFound` or `StartBattle`.

`20129/v7` is another empty `0x90`-byte message, allocated by factory case
`0x4EA1`. Its ID and version getters are `0x1050B40` and `0x1050B48`.
Its Dispatcher branch handles matchmaking cancellation and invokes
`C8E1BC(loading_controller, 0)`. That clears `+0x74` while leaving `+0x75`
unchanged; the conjunction in `C8DA38` then cannot send `13584`. The branch
also resets the matchmaking controller through `BD978C`. `20129/v7` is thus a
matchmaking-cancel/reset notification candidate, while the same-build codec
does not expose a historical class name.

These two IDs are current-build facts and must not be copied into another libg
version. The exact relationship between `29303` and the server's processing of
`10500`/`14300` still requires a matching runtime frame.

### Matchmaking state 16 boundary

`BD5C38` writes the matchmaking controller tuple:

```text
controller+656 = state
controller+652 = started
controller+660 = matched_id
controller+957 = 0
```

`BD9A00` calls it with state `16` after a matching object's id agrees with the
global selection. `BDB444` can also reach state `16` from a nested type-20
object. This establishes state `16` as the post-match-object controller state
and a local `matchmaking_start` / `team_found_start` UI transition. It does not
by itself prove that state `16` is the loading screen or the first formal battle
state.

`BD978C` is a cancel/reset path. It clears the started flag, hides or tears down
matchmaking UI, and sets state and matched id to `-1`. It must not be used as
evidence for a loading transition.

Replay handling at `C87620` and `1061B1C` reads replay JSON, extracts the battle
object and command array, and feeds commands through `D2EEF0`. It confirms that
`D2EEF0` is a general command queue, but it is not evidence for the online
matchmaking or server-command startup path.

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

The command factory path is also closed in the current build:

```text
sub_D2C0F4(type, out)
  type > 999 -> out = 0
  type <= 999 -> sub_D2C138(type, context, out)

sub_D2C138
  type == 2 -> allocate 0x38 bytes
            -> sub_D32974(object, context)
```

`sub_10CD0F4` is a same-build creation path that reaches this factory for type
`2` and then passes the object into the battle/game object at `107E098`.
`sub_D2FE78` reaches the same factory while decoding the JSON key `ct`, then
invokes the object's JSON codec through virtual slot `+0x60` on key `c`.
Those paths prove factory registration and the JSON integration. They still do
not prove whether the resulting gameplay command is carried by `16513` or
`18193` at issue time.

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

Three current-build message classes were inspected. The message objects for
`26349` and `11865` both have empty encode/decode methods, but that describes
only their ordinary Piranha codec surface. The `26349` dispatcher handler reads
two bytes manually from the embedded ByteStream, so an empty codec must not be
misread as an empty dispatcher payload. `11865` remains an empty reply object.
Neither class is yet proved to be `MatchFound`. `29613` is not one: its nested
object is now identified as event configuration and must be kept out of the
match-found candidate set.

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
checks the controller byte at `+0x298`, a match/context pointer at `+0x1A8`,
and the `sub_11FF54C` readiness predicate. When those checks select the
existing-state path, control goes through the active object's virtual slot at
`+0x1C0` or `sub_BDBB2C` and does not call `sub_AD9070`. The loader call is
reached only when the combined readiness condition is false. If the delayed
condition path is selected instead, the dispatcher stores the message at
`+0xA8` and resets its one-second timer at `+0xB0`.

The delayed path is drained by `sub_AD97C4`, which is called from the main
network tick `sub_ADCB10` whenever the connection state is not `2` or `3`.
`sub_AD97C4` accumulates frame time in `+0xB0`; only after the accumulator is
at least `1.0` and the `sub_11FF54C` predicate succeeds does it call
`sub_AD9070`, retire the stored message, and clear `+0xA8`. Thus `26349` can be
handled immediately or delayed, but the delayed branch has an explicit
one-second minimum.

Once called, `sub_AD9070` obtains the incoming message's embedded ByteStream
with `sub_121BD78(message)`, which returns `message + 8`, and resets that stream
with `sub_121220C`. The stream vtable is `0x18BE1B8`; slot `+0x150` resolves to
`sub_1213054`, which reads one U8 from buffer pointer `+0x38` at signed cursor
`+0x14`, advances the cursor by one, and returns that byte. `sub_AD9070` calls
this slot twice:

```text
reset stream
byte[0] = read_u8()              ; discarded by this handler
byte[1] = read_u8()
if byte[1] == 1:
    app_state = 9
    pending_stage = 9 with parameter 1
else:
    app_state = 4
    pending_stage = 4 with parameter 0
apply pending stage
reset stream
construct and queue 11865/v27
```

The first byte's semantic is not established, so it must not yet be called a
protocol version or a match type. The second byte is a proven binary selector
only: value `1` selects stage `9`; every other value selects stage `4`.
`sub_121220C` also clears ByteStream fields `+0x1C` and `+0x20`; the final reset
means the dispatcher does not intentionally expose its two-byte read position
to the next consumer. The ordinary codec's empty encoder does not write these
bytes. A test producer must supply a valid ByteStream body and satisfy the
surrounding controller state rather than assuming that an empty `26349`
instance is a complete bootstrap.

The controller field read on the `26349` path at `+0x298` is sourced from the
configuration key `CACHE_MAX_EXTRA_SC_FILES`; it is not a battle-state field.
Together with the loading/SectorManager calls, this places `26349` on the
loading/application-controller branch. The class name `MatchFound` and the
payload field name for `byte[1]` remain unsupported.

### Current global-controller stage map

`sub_C8C900(old_stage, source+0x28, source+0x38)` is the central child-stage
factory. Its current-build mapping is:

| Old stage | Construction | Current meaning |
| --- | --- | --- |
| `1` | `sub_C8E920`, `0xB8` bytes | Startup/loading root controller |
| `2` | `sub_C93730`, `0x90` bytes | Unresolved |
| `3` | `sub_C85C88(mode=0)`, `0x330` bytes | AutoChess preload/UI controller candidate |
| `4` | `sub_C85C88(mode=1)`, `0x330` bytes | Non-AutoChess live-battle controller candidate |
| `5` | no allocation | No object is created by this implementation |
| `6` | `sub_C951DC`, `0xB8` bytes | Supercell ID login controller |
| `7` | `sub_C8276C`, `0x18` bytes | Unresolved |
| `8` | `sub_C96934(mode=2)`, `0x360` bytes | AutoChess root controller |
| `9` | `sub_C85C88(mode=2, parameter=1)`, `0x330` bytes | AutoChess live-battle controller candidate |

The stage factory therefore does not describe one homogeneous loading layer.
Stage `6` is excluded from the battle path by its SCID callbacks: `sub_C95970`
selects the explicit strings `with_scid` and `without_scid`, and the
`absent_scid` branch in `sub_BA7DFC` schedules pending stage `1`. Stage `8` is
excluded by the AutoChess configuration, augment, game-mode, location, and bot
strings reached through `sub_C96934`; `sub_C96D50` also schedules stage `8`.

The three `sub_C85C88` modes are distinguished below the stage constructor.
`sub_C86930`, which initializes the battle simulation/controller layer,
switches on `object+0x88`:

- mode `0` calls `sub_BC9D88`, which loads AutoChess UI assets and effects;
- mode `1` skips that AutoChess asset path and constructs the live battle
  simulation plus its controller-side wrapper;
- mode `2` also constructs the live battle simulation, and when `object+0x480`
  is `1` calls `sub_B5579C`, which loads AutoChess merge/effect resources.

Later in `sub_C87620`, the scene-construction branch has the same split:
mode `2` uses the AutoChess-specific path `sub_BB8F04`; other non-zero modes use
`sub_BAEBE8`; mode `0` sets a `"MENU"` transition and constructs
`sub_BBBD78`. This is strong same-build evidence that stage `9` is an
AutoChess battle variant and that stage `4` is the non-AutoChess live-battle
candidate. Stage `3` remains a preload/UI candidate until its gameplay entry is
observed; it must not be folded into the ordinary PvP path.

The stage-4 construction path is now resolved at the object level:

```text
C8DD84(global_controller)
  stage     = controller+0x34
  context   = controller+0x28
  parameter = controller+0x38
  child     = C8C900(stage, context, parameter)
  call child->vtable+0x40
  controller+0x20 = child
  controller+0x34 = 0
  controller+0x38 = 0
  controller+0x30 = previous stage
```

The stage object created by `C85C88` stores its mode at `+0x58` (`88`), external
context at `+0xD8` (`216`), and constructor parameter at `+0x1E0` (`480`).
For stage `4`, those values are mode `1`, the supplied context, and parameter
`0`.

`C86930` then constructs the live simulation in all three modes, but the
resulting ownership differs:

```text
mode 0:
  1060BA8(mode=0, ...)
  controller+144 = simulation
  no SectorManager

mode 1:
  1060BA8(mode=1, ..., parameter=0)
  controller+144 = simulation
  CCE5A8(simulation)
  controller+128 = SectorManager

mode 2:
  1060BA8(mode=1, ..., parameter)
  controller+144 = simulation
  CCE5A8(simulation)
  controller+128 = SectorManager
  parameter == 1 additionally loads AutoChess merge/effect resources
```

`CCE5A8` is the small SectorManager constructor. It stores the simulation at
`+8`, initializes count/state fields at `+0x28`, and installs vtable
`off_1873398`. `C86930` also constructs the command-list listener `7F5098`,
stores it at controller `+136`, and attaches it to `simulation+0x38`
(`+56`) through `D2F330`.

After loading reaches phase `7`, the stage-4 mode-1 branch constructs
`BAEBE8`, whose scene object later becomes controller `+80`. `BAEBE8` reads
the simulation directly from controller `+144`; its initializer takes
`simulation+168`, reaches a nested object at `simulation+168 -> +432`, and
creates the scene-local `855F28` object stored at scene `+752`. The exact
network callback and first battle message still have to be resolved below
these objects.

The same-build tick also exposes the normal loading fork in `sub_C90A4C`.
When `sub_C8C5C4(global_controller)` is non-null, `CE65CC(context)` says
context `+0x14C` (`332`) is zero, and both `CD6E0C` and `CD6E24` return false,
the function:

```text
C8DFD0(controller, 4, 0)
C8E2C4(controller)
C8E1D0(controller, 1)
construct and enqueue 10500/v9 or 14300/v9
```

Otherwise it selects pending stage `3`. This proves that this build has a
static path into stage `4`; it does not yet prove that the selected instance is
ordinary ranked/1v1 PvP. Friendly matches, challenges, events, replays, and
other non-AutoChess battle modes can still share the same stage and simulation
objects until their context values and runtime responses are separated.

### Battle scene activation and first command boundary

The stage-4 handoff continues through a scene and controller tick layer. This is
the narrowest same-build static path currently identified between loading and
the LogicCommand scheduler:

```text
loading stage 4
  -> BAEBE8 constructs the battle scene
  -> scene vtable slot +0x9E8 = BB2D08
  -> BB2D08 calls C89A7C(controller, active)
  -> C89A7C attempts C89890(controller) on a false-to-true transition
  -> C89890 conditionally drains a command list as 16513/v9
```

`BB2D08` is the scene-local gameplay tick rather than a message dispatcher. It
contains two `C89A7C` call sites, at `0xBB3A54` and `0xBB4060`. The second call
is guarded by a battle state in `{6, 15, 17}`, a true battle-object predicate,
and an empty result at scene `+0x310`. These checks establish an internal object
lifecycle boundary; they do not prove that this is the ranked/1v1 branch.

`C89A7C(controller, active)` only calls `C89890` when active differs from the
stored byte at controller `+396`, and only when the new value is true. The
`16513` drain is itself conditional:

```text
simulation state in 1..4
and loading_controller +116 == 0
and current_tick > controller +192
```

When those conditions pass, `C89890` detaches the current listener snapshot,
constructs `16513`, optionally appends the tail from controller `+232`, sends
through `AD3AC0`, and advances controller `+192`. Scene activation therefore
contains a real `16513` attempt, but it is not an unconditional first packet.

The controller vtable supplies two relevant ticks:

```text
controller vtable +0x58 -> C88BBC  main controller tick
controller vtable +0x68 -> C899E0  alternate transition tick
```

`C88BBC` passes a time delta to `C890BC`, drains pending commands through
`C893D0`, clears the pending count, and can construct `18193` state updates.
`C899E0` calls `C890BC` and the scene tick only while controller `+396` is
zero. In the currently inspected call sites, `C88BBC` is the branch that keeps
the command queue moving during the active controller lifetime.

`C893D0(controller, command, flags)` has two command transports:

```text
simulation condition false:
  execute command as needed
  C89778(controller, command, tick, state)
  send 18193/v27

simulation condition true:
  execute command as needed
  D2EEF0(simulation +56, command)
  enqueue the ordered battle command
```

The ordered queue is consumed by `D2F37C`, which adjusts commands that are more
than 20 ticks late, executes commands at the due tick, and can invoke a queue
callback. The callback virtual slot `+0x20` resolves to a nullstub in this
build. This rejects the earlier interpretation that every successful queued
command reaches `7F5250` and produces another `16513`.

The server-authoritative branch remains separate:

```text
message 20941/v16
  -> Dispatcher branch 0x51CD
  -> C89B20
  -> controller mode in {1, 2}
  -> CCE804
  -> 1064A08
```

`1064A08` applies state and commands at ten times the wire tick and supports
rewinding when a later state value conflicts for an already seen tick. This is
the server-to-client state/turn path, not evidence for the client's first
outbound packet order.

The currently recovered LogicCommand type `2` can now be tied to its producer
and codec without assigning cross-version field names:

```text
construct from actor  D329A8
execute               D32A08
type getter           D32E94 -> 2
wire codec            D32EA0 / D32F10
JSON codec            D32D98 / D32E1C
```

The actor-copy constructor loads:

```text
command +0x28 <- actor +0x08
command +0x2C <- *(actor +72) -> +64
command +0x30 <- actor +0x98
```

The wire body emits base fields, then subclass `+0x30`, `+0x28`, `+0x34`, and
`+0x2C`. The JSON key `cgid` is proved for `+0x28`; the other subclass offsets
remain structural. `8A92B0` can construct this type on one branch, but it also
has a separate type-`1548` action-command branch described below. The available
code does not yet separate every player input, AI/bot, and replay/test producer.

### Current command factory and choice commands

The ordinary command-list reader `D2C0F4` only forwards a command type when it
is at most `999`; it then calls `D2C138`. That constructor switch is a roughly
10 KB compressed dispatch over types `2..1548`:

```text
jump table          0x341B38
case relative base  0xD2C174
distinct targets    261
non-default 2..999  259
default in 2..1548  739
```

The full type range is reachable by the low-level constructor switch, but type
`1548` has no ordinary `D2C0F4` path because of the `<= 999` guard. This explains
both sides of the prior observation: `1548` is constructible, while the checked
`16513` and `18193` command-list readers do not instantiate it.

Selected same-build constructor entries are:

| Type | Constructor | Allocation |
| ---: | --- | ---: |
| `2` | `D32974` | `0x38` |
| `64` | `D31330` | `0x30` |
| `86` | `D309A8` | `0x58` |
| `95` | `D31B00` | `0x30` |
| `798` | `D50D6C` | `0x38` |
| `824` | `D5C6D0` | `0x38` |
| `1548` | `D32FD4` | `0x58` |

### Type `798` client-state or selection command

Type `798` is not a direct matchmaking message. The same-build constructor
`D50D6C` installs vtable `0x187A218`; the type getter `D510A8` returns `798`.
Encoder `D510B4` and decoder `D51124` agree on this order after the common
LogicCommand fields:

```text
VInt object+0x34
VInt object+0x28
VInt object+0x2C
VInt object+0x30
```

The executor `D50DA0` provides bounded meaning for those offsets:

| Offset | Recovered role |
| --- | --- |
| `+0x28` | numeric value used by action `5` |
| `+0x2C` | selected entity or list-entry id |
| `+0x30` | action discriminator |
| `+0x34` | boolean-like auxiliary value used by actions `15`, `61`, and `65` |

The recovered action branches modify current-player or list state:

```text
5   set player+0xB0 from command+0x28
15  add selected id to the unique-int vector at player+0xE0, then update a controller
18  set player+0xBC from selected id
20  set player+0xB8 from selected id
27  update the bounded player collection at +0xC0
41  set player+0xB4 from selected id
61  set a boolean field on one listed object selected by id
62  insert selected id into the bounded collection at +0xC0
65  set another listed object's boolean field selected by id
```

On the home battle path, `BD9528` compares a selected object's id with
current-player `+0xB4`. When they differ, it creates type `798` with action
`41`, enqueues it immediately, and then constructs type `824`. A clean current
build v2 trace reproduced this state-dependent observed sequence:

```text
type 798: +0x28=-1, +0x2C=-1, +0x30=41, +0x34=-1
type 824: next one-item 16513 command
```

The trace remained on the local search presentation and sent empty `16513`
frames afterward. No `18193`, `20941`, `13584`, `21631`, or high-rate UDP hook
ran. The screen also stated that the training camp must be completed to unlock
online multiplayer arena. This proves the outbound click sequence and the
profile-gate boundary, not a server match or battle start.

A later v3 trace from a synchronized selection state reached 824 directly:
`BD5A80(0,-1,0)` constructed the command without type 798. Therefore
"798 action 41 then 824" describes the mismatch branch, while "824 without
798" describes the branch observed after selection state had converged.

The current business identity of player `+0xB4` is unresolved. Type `798`
therefore remains numbered: AstralRoyale's older `LogicStartMatchmakeCommand`
has a different subclass shape and cannot supply a class name. The current
best-supported label is a generic client-state or selection submission
command.

The normal enqueue function `D2EEF0` appends to the pointer vector at object
`+8`, whose count is at `+20`, and grows through `7F5328`. It destroys the
command instead of appending it when `1062BC8(command_context)` returns `2`.
`D2F37C` consumes the resulting commands by tick. Neither function proves that
every accepted command is emitted unchanged in either envelope.

Type `64` is best described provisionally as an indexed-choice command. Its
producer `8B2BA8` obtains a 4- or 8-entry capacity, uses `D07150` to find the
first empty index, and checks `D05060(container) == 1`. Passing selection
objects equal to controller `+224`, `+232`, or `+240` encodes choice `0`, `1`,
or `2`. The producer constructs `D31330`, calls `D31360`, immediately sends
through `C893D0(..., 1)`, and then updates the local presentation through
`8B1278`.

```text
type 64
  execute           D31368
  type getter       D31700 -> 64
  JSON encode       D31568
  JSON decode       D3163C
  object +0x28      idx
  object +0x2C      choice
```

The executor validates the index and choice for the 4/8-entry shape before
calling `D073BC`. The JSON keys prove `idx` and `choice`, but the exact game
screen or choice category is still unresolved.

Type `95` has stronger semantic evidence and is provisionally a draft/card
choice command. Producer `8BF670` checks that controller `+360 == 1`,
controller `+400 == 0`, `1065AE8(..., 95)` is false, and `D0E0B8` permits the
choice. The selected object must belong to the pointer vector at controller
`+304`, with count at `+316`; the current selection bitmap is at `+320`. The
already-selected list occupies `+280/+288`.

```text
type 95
  construct          D31B00
  set index          D31B30  -> object +0x28
  execute            D31B38
  type getter        D31ED4 -> 95
```

After construction, `D2BCA0` copies the base LogicCommand fields and
`C893D0(..., 1)` sends the command immediately. The local handler `8BF7EC`
finds `"time_bar"`, applies `"fade"`, displays `TID_CHOOSE_A_CARD`, hides
`TID_TIME_LEFT` and `draft_time`, applies the local card-choice presentation,
sets a `2.0` timer, and updates controller `+400` to prevent a second choice.
The executor checks battle mode `5` and applies the selection through
`D0DDF8`.

This is direct static UI evidence for a draw/card-choice command, not a live
frame observation. One earlier JSON-key note called the type-`95` key `spell`;
that key is retained as a discrepancy rather than promoted to the complete
class identity. A natural draft screen and captured command are still needed
to prove the runtime envelope and final semantic name.

### HUD ability controls and the `1548` action-command family

The battle HUD contains two different control families that were previously
conflated:

```text
sub_8D2694
  four 0x3A8 card views
  HUD +0x138, +0x140, +0x148, +0x150
  each constructed through sub_8D8EF0

sub_8D39D8
  two 0x1F8 ability controls
  HUD +0x180 and +0x188
  each constructed through sub_8A8F54
```

`8D2694` loops exactly four times, allocates a `0x3A8` object, calls
`8D8EF0`, and stores it at `HUD + 0x138 + index * 8`. It is therefore the card
view chain. `8D39D8` destroys any previous objects in HUD `+0x180/+0x188`,
allocates two `0x1F8` objects through `8A8F54`, stores them, and calls
`8D2254`.

`8D2254` supplies the decisive identities. When the child object at
`HUD+0x180` has no name at child `+0x38`, it assigns `"ability_right"`.
The same operation on `HUD+0x188` assigns `"ability_left"`:

```text
HUD +0x180 -> 0x1F8 child -> name "ability_right"
HUD +0x188 -> 0x1F8 child -> name "ability_left"
```

These are ability controls, not card slots. `8A8F54` initializes them with
vtable `off_17FFD88`; the exact primary-vtable slot map remains open, so the
constructor address alone is not used to assign method names.

The release path enters `8A92B0` after the pointer-up ownership and target
rectangle checks in `8A91E8`. The delayed path enters the same function from
`8A9684` when control byte `+0x154` is set. `8A92B0` then has two command
constructors:

```text
normal ownership path
  allocate 0x38
  D329A8(type-2 actor/card command)
  C893D0(context, command, 1)

ability/action path
  allocate 0x58
  D32F88(command, context, 194)
  command+0x2C = selected_list[0]+0x08
  C893D0(context, command, 1)
```

The second value passed to `D32F88` is not a command type. That constructor
creates LogicCommand type `1548`, reads the current vtable at `0x1877BB0`, and
stores its third argument at object `+0x28`. The vtable slot at `+0x20`
returns `0x60C` (`1548`); the encoder is at slot `+0x28` (`D3E910`) and the
decoder at slot `+0x30` (`D3EA84`).

The `1548` object has a bounded `0x58`-byte layout:

| Offset | Structural field |
| --- | --- |
| `+0x28` | `actionIdx` |
| `+0x2C` | `intParameter1` |
| `+0x30` | `intParameter2` |
| `+0x34` | `intParameter3` |
| `+0x38` | string parameter |
| `+0x48` | optional integer-array parameter 1 |
| `+0x50` | optional integer-array parameter 2 |

The earlier IDA display `s__9` was an unresolved string placeholder. The raw
instruction at `0xD3DF30` is `ADRL X1, loc_3163CC`, and the original bytes at
`0x3163CC` spell `"intParameter1"`. This closes the field name without relying
on decompiler text.

The reciprocal codec order is:

```text
base LogicCommand codec
optional vector at +0x48
string at +0x38
int at +0x2C
int at +0x30
optional vector at +0x50
int at +0x34
actionIdx at +0x28
```

Type `1548` is a shared action-command family, not a single ability command.
Same-build `D32F88` call sites already show at least these producers:

| Producer | `actionIdx` source |
| --- | --- |
| `8A92B0` | `194`, ability-control path |
| `BB2668` | `62` |
| `800C44` | caller-supplied argument |
| `81E7F8` | `69` |
| `81E8C8` | `336` |
| `81F5F0` | `81`, first-NPC battle unit-placement hint |
| `A1E278` | `212`, piggy-bank/IAP path |

Action `194` is therefore best described as an ability-activation candidate
inside LogicCommand `1548`; it would be incorrect to call it command type
`194`. The static producer path is strong, but a natural same-build ability
release frame is still required before promoting action `194` from a candidate
to a runtime-confirmed action.

### Battle action dispatch and input checkpoint

The type-`1548` executor at `D33018` has a complete static action dispatcher.
It rejects `actionIdx > 427`, then indexes 428 signed 16-bit relative entries
based at `D3311C`. The full current-build table resolves to 322 distinct target
addresses. The following actions encountered in the producer pass map as
follows:

| `actionIdx` | Dispatch target |
| --- | --- |
| `62` | `D378CC` |
| `69` | `D39160` |
| `81` | `D38C88` |
| `194` | `D384B8` |
| `212` | `D36CF4` |
| `336` | `D36B60` |

Presence in this table proves static dispatch. It does not prove that an action
is enabled in the current game mode or that a natural client event reaches it.

The `194` target resolves both an acting and a target battle member:

```text
D384B8
  D2BCD8(command)                   -> base command logic-long
  CF46E4(member_table, owner, slot) -> acting member
  F2D944(member)                    -> acting battle object
  F254A8(member_table, +0x2C)       -> target object
  F3A518(acting_object, target)
```

`F3A518` adjusts counters at object `+0x340/+0x2F8`, optionally calls
`F11F0C`, resolves the battle context, and tail-dispatches virtual slot
`+0x728` with the target. The type-`2` actor/card execute path also reaches
`F3A518`. These observations make it a shared target-application phase, not
proof that action `194` is specifically an attack, heal, spell, or named
ability.

The action-`62` path is also bounded by arguments rather than a guessed name:

```text
D378CC
  +0x48 vector
  +0x30/+0x34 coordinate-like integers
  F380A4(acting_object, vector, x, y, 0)
    -> F3816C grid/target checks
    -> F38A68 battle-state application
```

The producer `BB2668` creates action `62` only after `a2`, `a4`, and `a5` pass
the branch guards and `F3B480(context)` is true. Its caller `8DA5F0` receives
`a2` through `W1`. The periodic `8DA030` call supplies `0`, so it cannot be the
action-`62` emission branch. The vtable region at `0x1802370` binds an input
controller family whose `8DB1BC` and `8DB254` paths supply `W1=1`;
`8DB044` stores the incoming float pair at controller `+0x2D4/+0x2D8` and
tail-calls `8DA5F0` with the same flag. This establishes pointer-state/release
plumbing, while the exact UI gesture and gameplay meaning of action `62`
remain a hypothesis.

When the action-`62` branch is not taken, the same producer constructs a
different command through `D309A8`. That object's getter returns command type
`0x56` (`86`), not `2`. Its executor reaches
`TID_INVALID_SPELL_POSITION` on a rejected placement, so this alternative is a
spell-placement command. It should not be collapsed into LogicCommand type `2`
or into the type-`1548` action family.

The checked client command-list readers also impose a direction boundary.
`D2C0F4` rejects types above `999`, `D2C138` is the concrete constructor switch,
and `D2F214` delegates to them. Both the current `16513/v9` and `18193/v27`
decoders call `D2F214` for listed commands. Those paths therefore do not
instantiate type `1548`. Type `1548` is proved as a client-issued action
command class; whether another protocol path consumes it on the client remains
open.

The current evidence therefore supports this bounded model:

- `16513/v9` is the command-list drain used around battle-controller
  activation and other controller states `1..4`. In the active local Sector
  branch, successful queued commands are appended through listener slot
  `+0x18` and drained here.
- `18193/v27` is emitted by `C89778` either for an immediate single command
  when the Sector queue condition is false or as the periodic Sector state
  update controlled by `SECTOR_COMMAND_MESSAGE_INTERVAL_SECONDS`; it prefers
  the configured UDP socket.
- `20941/v16` is the authoritative server state/turn batch consumed by
  `C89B20 -> CCE804 -> 1064A08`, then queued into `battle_context+56`.
- LogicCommand type `2` is a high-confidence actor/card operation, but neither
  its first live carrier nor its exact first-battle trigger is proved by a
  runtime frame. The static active-Sector path predicts `16513`.

The remaining boundary is runtime closure. The controlled Null session reached
98% and then showed the local privacy prompt; accepting it produced
`Connection interrupted`, so no natural battle frame was captured in this pass.
The first observed frame order must therefore remain a hypothesis until a
same-build session records the transition from match/loading traffic through
the first `16513`, `18193`, and `20941` envelopes.

### `23592/v10` is the current match controller parameter-notification candidate

The strongest current-build clientbound matchmaking candidate is `23592/v10`.
It is a real same-build message class with a reciprocal codec and a Dispatcher
consumer; it is not an old message ID copied from a reference implementation.

```text
23592 / v10
  factory case      0x5C28 in 0x1026E64
  allocation        0xA0 bytes
  constructor       0x105252C
  vtable            0x18A95C0
  encode / decode   0x1052570 / 0x10525C8
  ID / version      0x1052624 / 0x105262C
```

Both codec directions operate on the same three unsigned 32-bit fields. Their
wire order is proved by the matching encoder and decoder:

```text
object +0x98
object +0x90
object +0x94
```

The constructor initializes `+0x90` and `+0x98` to `0xFFFFFFFF`. The Dispatcher
case at `0xAD6218` first obtains a controller through `0xBBAEC0`, then follows
two state-dependent paths:

```text
controller not ready
  -> BDA93C(controller)

controller ready
  -> BDA918(controller, message +0x90)
  -> BDA930(controller, message +0x98, message +0x94)

current match controller present
  -> 952B88(match_controller, message +0x90)
```

The consumers write:

| Consumer | Controller effect |
| --- | --- |
| `BDA918` | `+0x2B0 = float(+0x90)`, `+0x2B4 = float(+0x90)`, `+0x2BC = -1` |
| `BDA930` | `+0x29C = +0x98`, `+0x2A0 = +0x94` |
| `952B88` | `+0x154 = float(+0x90)` |

The not-ready path `BDA93C` scans entries rooted at controller `+0x220` through
`FF51F8`. For a matching entry it reaches `C8E1F0`, calls `BC0290(..., 1)`,
then invokes the general presentation transition
`BD5C38(controller, 20, matched_id, ...)`. State `20` is associated with local
match/team-found presentation in `BDB444`, but that shared state does not prove
that `23592` itself is the server's final MatchFound message.

The state-20 transition itself is now bounded further:

```text
BD5C38(controller, 20, matched_id, ...)
  controller +0x290 <- 20
  controller +0x28C <- started
  controller +0x294 <- matched_id
  controller +0x2B8 <- 0
```

The controller tick then decides whether the matched object is ready:

```text
BDB444 / BD9A00
  state 20 + matching object available
  -> BD5C38(controller, 16, matched_id, ...)
  -> team_found_start

BDB5F4
  no matching object
  -> idle / player_waiting
  -> possible BD978C cancellation/reset
```

`BD978C` clears the started bit, resets state to `-1`, and resets the associated
identifiers and timers. Therefore state `20` means that a match controller has
been started and is waiting for or presenting a matched result. It is not, by
itself, proof that the client has entered the battle loading screen or the
active sector.

The defensible label is therefore "matchmaking/battle controller parameter
notification candidate." It is currently stronger than the old `27552`, the
conditional `26349`, and the Clan-War-only `10711`/`15458` pair. The three
payload integers now have the following evidence-ranked interpretations:

| Wire field | Controller effect | Current interpretation | Confidence |
| --- | --- | --- | --- |
| `+0x98` | `controller+0x29C`; compares to zero and is reduced by delta time in `sub_8D9AE8` | countdown/deadline candidate | 0.72 |
| `+0x90` | `controller+0x2B0` and `+0x2B4` as floats; another counter increases by delta time and is compared with configured limits | elapsed/wait-time candidate | 0.60 |
| `+0x94` | `controller+0x2A0`; later compared with small integer modes | mode/selector candidate | 0.45 |

These are semantic hypotheses, not field names. In particular, the same
numeric offset in another controller object must not be used to name `+0x94`.

Two adjacent same-build messages are empty match-control objects rather than
ordinary PvP MatchFound payloads:

```text
10711 / v7   constructor 0x1050B84
15458 / v11  constructor 0xD25C24
```

The Clan-War controller method `0x94D5C8` selects `10711` for mode 0 or `15458`
for mode 1 before enqueueing through `AD3AC0`. That is not a complete ownership
statement. `BD55B8` independently selects `10711` when controller flags
`+676`, `+677`, and `+678` are all clear. Therefore `10711` is a shared
control object with at least two producers, while `15458` remains established
only on the Clan path. Neither message's empty codec proves a MatchFound role.

### Current matchmaking UI is still a presentation boundary

The ordinary-looking match controller presentation path is now closed through
its object and vtable layers:

```text
C32468
  -> allocate 0x190-byte object
  -> 94D384
  -> 951B34(..., 1, 0)
  -> primary vtable 0x180E548
  -> secondary vtable 0x180E720
```

`94D384` records the profile/matched identifiers at `+0x168..+0x188`.
`951B34` contains the ladder, 2v2, Clan-War shield, `play_arena`, and
matchmaking-failure UI strings. The secondary table slot `0x180E708` resolves
to `94D678`, which calls the local presentation transition `952A68`. That
transition plays `players_found_start`, `bangs_test`, and
`match_found_start` and writes presentation state `2` at object `+0xE4`.
`BDBB2C` is a neighboring presentation-only path that plays
`players_found_start` or `team_found_start` and clears controller flags.

This is stronger evidence for what the screen does, but still does not show
which profile mode created the overlay or which Piranha message invoked its
network action. The direct native reference to `94F870` is only the
eight-byte thunk `94F914`; the vtable data reference alone does not identify a
runtime event. `C95970` has a `without_scid` branch that can reach
`BA7DFC` and then `C8DFD0(..., 1, ...)`, but the state-16 or ordinary-ladder
caller that selects that branch remains unproved.

The same controller method `BD55B8` emits one of three empty messages after
setting controller `+676`:

```text
controller +677 nonzero -> 11278/v9
else +678 nonzero        -> 10363/v11
else                     -> 10711/v7
```

The constructors install vtables `0x18A6FD0`, `0x18A6C10`, and `0x18A9110`
respectively. All three objects have the `0x90`-byte Piranha-message base
shape, and all are enqueued through `AD3AC0`. Static callers of `BD55B8` are
`BD3C08`, `BD5F34`, and `BDB144`. No getter, encoder, decoder, or consumer in
these three classes contains a field codec, so their wire bodies are empty in
this build. Their business names remain unknown; they are useful control-flow
observations, not MatchFound facts.

### `26915/v9` battle-result carrier

The current Dispatcher case `26915` now has a complete same-build factory,
message, codec, and consumer chain:

```text
26915 / v9
  factory case 0x6923 in 1026E64
  -> allocate 0x110 bytes
  -> constructor 103FB24
  -> vtable 18A6B70
  -> +0x20 version getter 103FF70 returns 9
  -> +0x28 ID getter 103FF68 returns 26915
  -> encoder 103FBBC
  -> decoder 103FD64
  -> Dispatcher AD3DA8 case
  -> BB145C(controller, message)
  -> 8B443C result UI
```

The reciprocal codec establishes the field order below. Offsets are the
decoded message-object offsets, and the length-bearing slots are intentionally
not named as strings or arrays without their concrete element types.

```text
u32/vint +188
u32/vint +172
length-bearing object +224
u32/vint +256
length-bearing object +248
u32/vint +204
u32/vint +260
u32/vint +176
u32/vint +168
u32/vint +196
u32/vint +152
bool     +216
u32/vint +264
u32/vint +192
u32/vint +180
u32/vint +184
length-bearing object +240
length-bearing object +208
u32/vint +268
length-bearing object +232
u32/vint +200
length-prefixed raw blob {ptr +160, len +156}, max 900000 bytes
```

The final blob is the decisive consumer field. `103FB7C` detaches the pointer
at `+160`, while `BB145C` reads its length from `+156`. It wraps the bytes in a
stream, consumes a first byte, and chooses `CCEAAC` when that byte is `1` or
`CCEBC8` otherwise. It then constructs the `0x468`-byte UI object `8B443C`,
which selects resources including `pvp_battle_end_bo3`,
`pve_battle_end_victory`, `pve_battle_end_defeat`, `pvp_battle_end_2v2`,
`pvp_battle_end_new`, `pvp_battle_end_tournament_new`, and
`pvp_battle_end_2v2_ladder`. `8B4884` additionally consults decoded `+168`
for the PvE victory/defeat choice.

This proves that `26915/v9` is the current battle-result carrier and that its
tail contains result/replay data. It does not prove the historical class name
or the meaning of the other 21 fields. `BattleResultMessage` is therefore a
high-confidence naming hypothesis, not yet a copied class identity.

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
for this test instance. Both message classes' ordinary codecs are empty, so the
experiment did not encode a valid body. It also demonstrates that, for that
test instance, the response was not only queued: the client sent it
immediately.

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
identify the missing member. The handler's two ByteStream reads were not
represented by the empty synthetic body, so this is also not evidence that a
validly encoded and state-compatible `26349` body fails.

The next runtime pass must stay passive on a fresh client session and capture
the normal messages and object setup after the real command-824 request. Until
that sequence exists, do not name `26349` or `11865` `MatchFound`, and do not
treat the synthetic exchange as a complete handshake.

This also corrects a proximity assumption: `26349` does not directly call
`sub_BDB444`, and `match_found_start` is not a direct consequence of the
`26349 -> 11865` branch.

### Child-stage controller graph and preloading envelopes

The loading UI is driven through an object hierarchy, so "stage 3" must not be
used as a synonym for the function `sub_C90A4C`. The main loop reaches the
global controller through `sub_C8C5B8`, then calls `sub_C8DA38(controller, dt)`
on its child-stage tick. The decoded range at `0x759CE8..0x75A214` is valid
code in the current image but is not owned by an IDA function; these facts come
from range disassembly and must not be promoted into a fabricated function
boundary.

When `controller+0x34` is nonzero, `sub_C8DD84` moves that pending stage into
action through the stage map above:

```text
old_stage = controller+0x30
pending_stage = controller+0x34
controller+0x34 = 0
destroy previous child at controller+0x20
child = C8C900(old_stage, source+0x28, source+0x38)
child+0x08 = old_stage
controller+0x20 = child
call child->vtable+0x40

while child exists:
  call child->vtable+0x50
  call child->vtable+0x58
  call child->vtable+0x68(controller, dt)
```

Constructor `sub_C8E920` installs vtable `0x186F2A0` for the case-1 child used
by `sub_C8C900`; vtable slot `+0x68` resolves through `0x186F308` to
`sub_C90A4C`. Once application/login readiness succeeds, `sub_C90A4C` schedules
a stage-4 child through `sub_C8DFD0(controller, 4, 0)`, performs its application
transition helpers, and selects one of the two outbound setup messages below.

Known pending-stage setters in this snapshot are:

| Function | Stage value(s) |
| --- | --- |
| `sub_BA7DFC` | `1` |
| range code at `0x75A214` | `1` or `6` |
| `sub_8B8334` | `4` |
| `sub_AD9070` | `4` or `9` |
| `sub_BB2468`, `sub_B19A80` | `3` |
| `sub_C96D50` | `8` |
| `sub_C90A4C` | `4` |

The preload message branch in `sub_C90A4C` is:

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

### `21573/v1` is chat status, not loading

Current `21573/v1` is a useful rejection of one earlier startup assumption.
Its factory case is `0x5445`, allocation size is `0xA8`, constructor is
`0x102C794`, vtable is `0x18A4970`, and its codec/getter addresses are:

```text
encoder       0x102C7D0
decoder       0x102C828
ID getter     0x102C8A4
version getter 0x102C8AC
```

The codec shows the top-level structural order:

```text
i32 from +0x90
string from +0x94
i32 from +0x98
```

The same-build Dispatcher consumer supplies the semantics. Case `21573`
passes message fields `+0x90`, `+0x94`, and `+0x98` to `sub_C8E3D4`, which
converts the first field to a duration/time float at `controller+0x8C`, stores
the second as a status/code at `controller+0x90`, and stores the third string
through `controller+0x98`. `sub_C8E404` maps status `3` with duration at least
one to `TID_CHAT_FAIL_BANNED`, and status `4` with the same duration condition
to `TID_CHAT_UNAVAILABLE`; `sub_C8E594` and `sub_C8E5B0` test those statuses
independently.

This establishes a chat status/error notification for the current build. It
does not justify any loading-controller or battle-transition interpretation.

### Match timeout is not a battle transition

`sub_BD5C38` has a started guard at matchmaking-controller `+652`. On a new
transition it records:

```text
controller+656 = state
controller+652 = started
controller+660 = matched id
controller+957 = 0
controller+0x2B0 = 0.0
controller+0x2B4 = 0.0
controller+0x2BC = -1
```

The state-20 path is `sub_BDA93C -> sub_BD5C38(controller, 20, matched_id,
...)`; `sub_BDB444` and `sub_BD9A00` can also apply state `16` after the
matching object reaches its status. Separate from that path, `sub_BDB444`'s
timeout branch reaches `sub_AD9BD4 -> sub_13CFCC4 -> sub_13FC5C4`, whose body
initializes the string `"vibrateDevice"` and invokes the device vibration
service. The timeout is therefore a UI/device response and must not be used as
evidence for a battle or loading transition.

### UDP sequencing and packet budgets

The current high-rate UDP builder now has several joined structural facts:

```text
sub_13334F0  assembles one or more queued application messages
sub_1336D30  clamps sequence values:
               0xFFFF is a sentinel
               values >= 65000 become 65000
sub_1336D70  stores pending message objects
sub_1332818  serializes them through the packet writer
```

When handler `+0x13C` is nonzero, sequence counters use `+0x148`, transmit at
most `64998`, then wrap. When handler `+0x145` is false, the builder uses
`0xFFFF` as the sentinel. The serializer `sub_1332818` passes a writer with
reserved message ID `-2` into `sub_1336A5C`, retries up to ten chunks, applies
the handler transform, and writes the socket.

Normal packet-size budgets are `1448` bytes when handler `+0x324` is set and
`1456` bytes otherwise. The builder also contains fallback budgets `1398`,
`1348`, `1248`, `1148`, `1048`, and `948`. These are allocation or payload
budgets, not yet proved datagram boundaries.

The exact clear, encrypted, and authenticated byte ranges still require a
captured current-build datagram that can be matched to this writer.

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
| `16513/v9` | client to server; `sub_C89890` constructs it and `sub_AD3AC0` queues it | tick, checksum, command count, command list, optional tail blob | general command-list envelope; structurally the current `EndClientTurn`-like form, the proven carrier for home command `824`, and the static active-Sector collector-to-wire path |
| `18193/v27` | client to server; `sub_C89778` constructs it and `sub_AD3A14` prefers the battle UDP socket | checksum, tick, optional single command, integrity bits | immediate single-command fallback and periodic Sector state envelope |
| `20941/v16` | server to client; Dispatcher branch `0x51CD` reaches `sub_C89B20` | server tick, checksum, command count, command list, optional raw payload | authoritative battle state/turn batch |
| `13584/v9` | client to server; `sub_C89F28` sends it from the loading-state controller | current tick, secondary battle counter, command count, command list | loading-state synchronization or sector-state request candidate |
| `27679/v9` | server to client; Dispatcher branch `0x6C1F` extracts its command and calls `sub_AD9658` | optional single LogicCommand | generic server-command delivery; not yet proven battle specific |
| `14300/v9` | client to server; fallback branch in the `sub_C90A4C` child tick | nullable/int-like, tagged object, nullable/int-like | preloading setup message; exact class and stage semantics unresolved |
| `10500/v9` | client to server; common-context branch in the `sub_C90A4C` child tick | i32 plus five tagged nested objects/containers | complex preloading setup message; exact class and stage semantics unresolved |
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

`C89778` has two callers/local shapes. `C893D0` invokes it with a command on the
`1062F6C == false` branch. `C88BBC` invokes it with a null command when the
current tick exceeds `controller+360 + 20 * configuration+576`, where
configuration `+576` is loaded from
`SECTOR_COMMAND_MESSAGE_INTERVAL_SECONDS`. The live Sector command-queue path
instead appends successful local commands to the `controller+136` listener,
which `C89890` drains into `16513`. This is why `18193` must not be described
as the unconditional carrier for every battle action.

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

The result search is no longer only string-driven. `26915/v9` has the
factory/codec/consumer chain above, and `8B443C` selects the battle-end UI
assets. By contrast, `BB1CC0` is a battle-start touch animation,
`CC3EB0` only logs `"Battle start"`, and `EF0C7C` parses a replay/JSON
`BattleResult` key and returns enum `7`. Those three must not be promoted to
the online `26915` wire consumer merely because their strings overlap.

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
shape is confirmed. The virtual method at slot `+0x18` is `0x13350E4`, the
standard RC4 PRGA: each input byte performs the state swap and XOR, so the same
method serves encryption and decryption.

The high-rate handler constructor `0x1332C6C` creates two independent RC4
states from the same key material and stores them at object `+0x280` and
`+0x288`. When the transform blob is at least 80 bytes, it also creates two
state objects from `0x1335154` and stores them at `+0x296` and `+0x304`, then
sets the mode byte at `+0x324`.

The queued-message transform is `0x1333B30`. That function:

```text
selects the RC4 state at handler+0x288
or the advanced transform state at handler+0x304 according to handler mode
obtains the transform's extra output size through vtable +0x20
allocates input_length + extra_length
invokes the transform through vtable +0x18
replaces the stream payload with the transformed bytes
queues the stream through 0x133594C
```

This upgrades the defensible statement to: the transport applies a
RC4-shaped transform to a queued high-rate application-message buffer in the
RC4 branch. It remains unproved that every byte of the final UDP datagram is
framed or encrypted under RC4, because sequence metadata, message lists, and
fragmentation are added later by `0x13334F0` and `0x1332818`.

`0x1335154` is the constructor for the alternate transform state, not a
standalone MAC. It copies two 32-byte inputs, records chunk parameters, fills a
256-byte working table from `0x121B188`, and invokes `0x1335244` to regenerate
duplicate-free chunks. Its vtable `0x18C42A8` dispatches through wrappers into
the `0x7F5...` cryptographic implementation. The constructor and random table
alone prove neither authentication coverage nor a MAC tag format.

The later wrapper `0x1A1A94C` allocates `input_length + 32`, clears a 32-byte
prefix, copies the input after that prefix, and calls the `0x7F5B20` /
`0x7FF918` / `0x7F5BCC` family. `0x7FF918` contains a 16-by-16 multiply and
38-modulus reduction, multiple mixing rounds, a 253-iteration loop, and a
32-byte state. The same target family is covered by the local NaCl-style
reconstruction as ChaCha/Poly1305-shaped code. That is a strong algorithm
family hypothesis, but the mapping of this 32-byte overhead and the surrounding
high-rate UDP composition still needs a same-session vector check.

The next byte-level checkpoint is to capture a datagram from both transform
modes and compare the pre-transform queued stream with the final datagram.
That comparison must identify the clear framing, ciphertext range, nonce or
counter inputs, and the 16- or 32-byte authentication material before any
complete UDP codec is implemented.

## Startup/profile checkpoint

The startup and profile pass is complete enough to separate the working labels
from the abbreviations that remain unresolved. Those findings belong to the
legacy official-server analysis snapshot identified above by
`B1CAF6D9...`; they are not current-build `110AA2B5...` addresses. The detailed
evidence remains in:

- `docs/clash-royale/profile-abbreviated-scalar-evidence.md`
- `docs/clash-royale/profile-three-binary-joint-analysis.md`
- `docs/clash-royale/profile-23245-frame-schema.md`

| Wire key | Defensible meaning | Boundary |
| --- | --- | --- |
| `rnk_sc` | Current ranked-ladder score | This is the strongest semantic match in the profile set |
| `scr` | Score or rating value | The UI has mode-specific consumers, so one exclusive subsystem meaning is not proved |
| `lti` | Monotonic progression index/value | An accepted update must advance beyond the stored value; the domain is still unresolved |
| `spi` | Latest progression snapshot | It is written together with `spt` during progression delivery |
| `spt` | Small enum-like tag paired with `spi` | Values `1..3` are observed; their business names are unresolved |
| `welo` | Clan-adjacent scalar | Codec and storage placement are proved; the business meaning is not |
| `rnk_ps` | Rank-related scalar | Previous, peak, and season interpretations are explicitly rejected |

For persistence and server models, retain structural keys and assign these
labels only where a consumer needs a readable field name. In particular, do not
expand the abbreviations beyond the table. Cross-profile observations from the
IPA or reference proxies can generate hypotheses, but acceptance still needs a
same-build codec/consumer pair or a controlled runtime mutation.

The transition marker to battle research is the verified home Arena click:
LogicCommand `824` is serialized inside message `16513`. The following sections
start from that traced request and separate ordinary PvP matchmaking,
Clan-War-specific controllers, clientbound response dispatch, and the UDP
transport bootstrap.

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
   inside it. In the active local Sector branch, `D2F37C` appends successful
   due commands through listener slot `+0x18`; `C89890` drains that list into
   `16513`. Current `18193/v27` instead has an immediate single-command path
   and a periodic Sector-state path controlled by
   `SECTOR_COMMAND_MESSAGE_INTERVAL_SECONDS`; both prefer the configured UDP
   socket. Current `20941/v16` is the corresponding server-side state/turn
   batch with checksum-based rollback support and feeds commands into the same
   Sector queue.
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
   The next static target is the exact state-16 event subscriber that reaches
   `sub_BA7DFC`, range code at `0x75A214`, or another
   `sub_C8DFD0(...,1/4,...)` pending-stage setter. The `21631/v27` codec has
   already been rechecked on this build.
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
10. Capture one natural `26915/v9` frame and decode the first byte of its raw
    tail into the `CCEAAC`/`CCEBC8` branches. Correlate its 21 preceding fields
    with `8B443C` and the replay/result controllers before assigning historical
    field names.
11. Stop before assigning card-command semantics to other command types until
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
- An empty `26349/v16` object is not a valid battle bootstrap. Its dispatcher
  handler manually reads two stream bytes even though the ordinary message
  codec is empty. The synthetic runtime instance produced the `11865/v27`
  reply, then reached a reproducible loading/cleanup SIGTRAP without proving a
  valid `26349` body was present.
- The local `Checksum` and `ChecksumReport` file-dump functions do not establish
  a network checksum format. `sub_11549D8` is a replay/JSON schema initializer,
  not the turn-checksum codec.
- `sub_DAA1D4` is a global configuration loader. Its
  `SECTOR_COMMAND_MESSAGE_INTERVAL_SECONDS` entry does not make the containing
  function a SectorCommandMessage codec.
- `sub_D1B804` is a field-map initializer for data involving `allianceId`,
  `players`, `mapItemId`, `battleId`, and `spectatorCount`; it is not yet a
  network message.
- `21573/v1` is not an unresolved loading branch. Its same-build Dispatcher
  consumer sets chat duration/status/text and maps status `3`/`4` to banned or
  unavailable chat errors.
- The `sub_BDB444` timeout is not a battle/loading transition. Its helper path
  reaches `sub_13FC5C4`, which initializes and invokes the device vibration
  service.
- `sub_C90A4C` is not itself a "stage-3 controller." It is the `vtable+0x68`
  tick of the child created by `sub_C8C900` case 1; this tick schedules pending
  stage `4` and sends the preloading envelope. Direct state-16-to-controller
  linkage remains unresolved.
- The child-stage layer is not uniformly a loading layer. Case `6` is the SCID
  login controller, case `8` is an AutoChess root controller, and cases `3`,
  `4`, and `9` invoke three different `sub_C85C88` modes.
- Stage `3` is not the ordinary PvP controller merely because it is the next
  child after stage `2`. Its `sub_BC9D88` branch loads AutoChess UI resources.
  Stage `4` is currently the non-AutoChess live-battle candidate.
- An empty ByteStream is not proof that `26349` is an empty logical payload.
  `sub_AD9070` reads two bytes from the embedded stream after resetting it.
- `10711` is not Clan-War-only. `94D5C8` selects it for Clan mode 0, but
  `BD55B8` also selects it on its default branch. The empty message remains an
  unidentified control object; `15458` is the Clan-specific sibling proved so
  far.
- `BB1CC0`, `CC3EB0`, and `EF0C7C` are not the online `26915` result consumer.
  They own a touch animation, a log-only start path, and replay/JSON parsing,
  respectively.

## Same-build boundary update: 17293, 27786, and 11418

Research snapshot: 2026-10-31.

This update isolates the current Null client build
`110AA2B5CAC391C498645E072B0D88729428C2C2845E7E2737CA8EE979059783`. The
supporting read-only artifact is:

```text
path      D:\IDA Scripts\ida-ai\.ida-ai-cache\battle-research-current-20261030-v2\battle-17293-27786-11418-boundary-20261031.json
size      934912 bytes
SHA-256   C100936E4C7BE38FB853F0A53BAE0D43BC79E83E07DB7D70104174558BD2F5EF
contents  47 bounded function views, reciprocal codec operations, callers,
          cross-references, and four raw vtable byte snapshots
```

The artifact was extracted from the isolated SQLite/IDB pair described under
"Build isolation". It is evidence for this build only. The addresses and
object offsets below must not be reused for another client release without a
fresh codec, factory, caller, and consumer match.

### Matchmaking notification 17293/v9

The factory, constructor, reciprocal codec, vtable, dispatcher consumer, and
id/version getters agree on the following identity:

| Item | Same-build value |
| --- | --- |
| Factory branch | `0x438D` in `0x1026E64` |
| Allocation size | `0xA0` |
| Constructor | `0x10408C0` |
| Encoder | `0x1040934` |
| Decoder | `0x10409B8` |
| ID getter | `0x1040A68` -> `17293` |
| Version getter | `0x1040A70` -> `9` |
| Vtable | `0x18A6EE0` |
| Dispatcher consumer | `0xAD3BA8` |

The encoder and decoder establish two independent optional `LogicLong` values:

```text
message +0x90  optional LogicLong
message +0x98  optional LogicLong
```

The consumer compares the value from `message+0x98` with the current identity
pair in `qword_19BC678`. Its finite-state effects are:

```text
identity match and controller object type == 20
  -> BD5C38(controller, state=16, matched_id, started=1, ...)
  -> C78474(...) clears the matching temporary IDs

identity mismatch and controller+0x58 != -1
  -> BD5C38(controller, state=20, id=-1, started=1, ...)

otherwise
  -> records a pending-match condition
```

This is a same-build fact: `17293/v9` is a two-optional-LogicLong clientbound
notification capable of driving the matchmaking controller to state 16 or 20.
It is not enough to assign business names to either `LogicLong`, state 16, or
state 20.

### 27786/v4 replay-related handler and 11418/v9 response

The current build confirms a second boundary:

| Item | Same-build value |
| --- | --- |
| Factory branch | `0x6C8A` in `0xAD3DA8` |
| Allocation size | `0xA8` |
| Constructor | `0x10424B4` |
| Encoder | `0x1042540` |
| Decoder | `0x10425C8` |
| ID getter | `0x104269C` -> `27786` |
| Version getter | `0x10426A4` -> `4` |
| Vtable | `0x18A7430` |

The reciprocal wire order is:

```text
packed signed int32
optional LogicLong
boolean
optional string/blob
```

The decoded object fields are:

```text
+0xA0  int32
+0x90  optional LogicLong
+0xA4  bool
+0x98  optional string/blob
```

The world dispatcher branch has two materially different outcomes:

```text
if message+0xA4 == false
  -> the UI text is exactly TID_REPLAY_NOT_AVAILABLE

if message+0xA4 == true
  -> allocate an 0x88-byte handler
  -> initialize it at 0x103B7E4
  -> handler+0x44 <- message+0xA0
  -> handler+0x48 <- message+0x90
  -> handler+0x58 <- message+0x98
  -> enqueue internal action 17 through BD6674
  -> on success, call BD6A68(world)
```

The handler's own identity is `16308/v11`, not the action number:

```text
ID getter       0x103BD50 -> 16308
version getter  0x103BD58 -> 11
vtable          0x18A62B8
```

`BD6A68` switch case 17 constructs an outbound `11418/v9` message:

| Item | Same-build value |
| --- | --- |
| Factory branch | `0x2C9A` in `0x1026E64` |
| Allocation size | `0xD0` |
| Constructor | `0x10518D0` |
| Encoder region | `0x105196C..0x1051AA0` |
| Decoder region | `0x1051AA0..0x1051C2C` |
| ID getter | `0x1051C2C` -> `11418` |
| Version getter | `0x1051C34` -> `9` |
| Vtable | `0x18A9430` |

Case 17 populates the response as follows:

```text
11418+0x98  <- handler+0x44 integer
11418+0xA0  <- handler+0x48 optional LogicLong
11418+0xBC  <- constant 4
queue       <- sub_AD3AC0
```

The reciprocal `11418` codec is fully bounded and writes, in object-offset
order, a length-delimited/tagged payload from `+0xC0`, `int32 +0xBC`, the
conditional object at `+0xA0`, booleans at `+0xB3`, `+0xC8`, `+0xB0`, `+0xB1`,
`int32 +0xB8`, `int32 +0x98`, boolean `+0xB2`, the conditional object at
`+0xA8`, and `int32 +0xB4`. Only the integer from `+0x98`, the object from
`+0xA0`, and request-kind constant `4` are tied to handler case 17 by the
producer; the remaining fields retain offset names.

The evidence supports these facts:

1. `27786/v4` is a clientbound notification with the proven wire shape above.
2. Its false branch is explicitly tied to `TID_REPLAY_NOT_AVAILABLE`.
3. Its true branch constructs a `16308/v11` handler and internal action 17.
4. A successful internal action 17 produces outbound `11418/v9` with
   request-kind `4`.

The following remain hypotheses or are rejected:

- It is still a hypothesis that `27786` is replay-related overall. The strongest
  current clue is the explicit replay-unavailable text on its false branch.
- It is rejected to label `27786` as a normal `MatchFound`, `StartBattle`, or
  generic challenge invitation from the present evidence.
- It is rejected to treat internal action `17` as a wire message ID. The
  observed IDs are `27786` clientbound and `11418` serverbound.
- It is not established that `27786 -> 17293`, that `11418 ->` battle start, or
  that command `824 ->` a guaranteed `17293`.
- `11418` must not yet be named `ReplayData`, `ReplayAvailable`, or
  `ChallengeResponse`; its codec is known, while most business field meanings
  remain unproved.

### Next battle boundary

The current direct runtime request remains a serverbound `16513/v9` command-list
envelope containing command `824`. The next bounded step is to give that
command a typed decode and record the first normal clientbound sequence produced
by an accepting server. Until that sequence is observed and decoded, do not
synthesize `MatchFound`, battle start, loading completion, or a UDP session from
`17293`, `27786`, or `11418`.

