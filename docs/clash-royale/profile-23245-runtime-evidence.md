# `23245` runtime dispatch evidence

Date: 2026-10-28

## Scope

This note resolves a version-mixing error in the earlier profile research. It
uses only the Null client library and its modified APK. It does not treat
addresses from the earlier official-library snapshot as equivalent.

| Artifact | SHA-256 |
| --- | --- |
| Null source `libg.so` | `110AA2B5CAC391C498645E072B0D88729428C2C2845E7E2737CA8EE979059783` |
| Modified APK | `84C7FF879034A579133ABA1BB12AC2C5320C9AFB035C10CDBCEA2F8CEF5E0EB6` |
| APK `libg.so` | `B1CAF6D94F7EFFAFEC4900A78F5D96DEFD98364B082A3CF5E560622A6858993C` |

The APK library differs from the Null source in only two URL/TLS data ranges:

- `0x300418..0x30042F`
- `0x340440..0x3404C0`

## Recovered message path

The current Null static path is:

```text
0x1026898  frame-to-factory forwarding
0x1026E64  mode-0 factory
0x10529C4  23245 constructor
0x1052A98  23245 decoder
0xAD3DA8   clientbound dispatch
0xAD5FCC   message-23245 branch
0xC8E054   game-state installation
```

The factory allocates `0xC0` bytes for ID `0x5ACD`. The object's vtable is
`0x18A96B0`:

| Slot | Function | Observed role |
| --- | --- | --- |
| `+0x10` | `0x1052A18` | encode |
| `+0x18` | `0x1052A98` | decode |
| `+0x20` | `0x1052C04` | returns version `10` |
| `+0x28` | `0x1052BFC` | returns message ID `23245` |

The decoder creates:

| Message field | Allocation | Decoder |
| --- | --- | --- |
| `+0x98` object A | `0x570` bytes | `0xF948A4`, then `0xF96F48` |
| `+0xA0` object B | `0x208` bytes | `0xCDF238`, then vtable slot `+0x20` / `0xCE0394` |

It then reads five VarInt32 values into `+0xA8`, `+0xAC`, `+0xB0`, `+0xB4`,
and `+0xB8`.

The `0xAD5FCC` branch:

1. Extracts A and B with `0x1052B68` and `0x1052B78`.
2. If B exists, passes `B+0x78` to the object reached from `GameRoot+0x510`.
3. Passes `message+0xB0` to `0xBDB09C`.
4. Writes `3` to `GameRoot+0x8`.
5. Calls `0xC8E054` with A, B, constants `0/1`, and the four other VarInt32
   values.

## Controlled run

The local D server completed login, established per-direction encryption,
accepted the known `17537` analytic event, and sent a `23245` version-10 frame
with a 4096-byte all-zero plaintext body after the first established `10108`
keep-alive.

Frida observed these calls on the same thread:

```text
0x1026898 message_id=0x5ACD
0x1026E64 message_id=0x5ACD
0x10529C4 constructor
0x1052A98 decoder
0xAD3DA8 dispatch, vtable=0x18A96B0, getter returns 23245
0xAD5FCC message-23245 branch
0x1052B68 object-A extraction
0x1052B78 object-B extraction
0xC8E054 game-state installation
```

The object entering dispatch had non-null object-A and object-B pointers and
the expected vtable. The client crashed after this path in a later null
callback. The zero body therefore proves routing and allocation, but does not
prove that it forms an acceptable profile.

A second trace used an exact 605-byte all-zero body. It shows the complete
all-zero representation:

```text
object A: stream offsets   0..508
object B: stream offsets 508..600
five outer VarInt32s:    600..605
```

That run reached the same dispatch branch and installer, then crashed at
`libg+0xC7C4C8` while executing a null `BLR X8` from `libg+0xC7C480`.

A later controlled repeat of the same exact representation did not reproduce
the crash. The client remained alive after dispatch and sent two established
messages: `14300` and `38000`. Therefore connection closure alone does not prove
that the zero body was rejected. The earlier crash remains an observed failure,
but its causal field and repeatability are unresolved.

## Follow-on `14300`

The same repeat captured the serverbound `14300` version-0 plaintext as:

```text
7f 01 12 a8 80 7f
```

The Null factory has an explicit `14300` case that allocates an `0xA8`-byte
object and calls constructor `0x1053A04`. Its vtable is `0x18A98E0`:

```text
0x1053AA0  encode
0x1053AF0  decode
0x1053B54  version getter -> 9
0x1053B4C  message-ID getter -> 14300
```

The encoder writes a signed VarInt32 from object `+0xA4`, serializes the
length-bearing object at `+0x98` with `0x1161B10`, and writes a signed VarInt32
from `+0xA0`. The captured bytes decode as:

```text
-1
u32be 0x112A880 = 18,000,000
-1
```

The middle value is carried as four bytes by the stream serializer; it is not
a VarInt. The producer, consumer, and business meaning of `14300` are still
unresolved. In particular, the evidence does not yet justify naming it as an
asset, download, cache, or synchronization request.

An inverse controlled probe sent the identical six-byte payload as a
clientbound `14300` frame after login. The client did not call constructor
`0x1053A04`, decoder `0x1053AF0`, or clientbound dispatcher `0xAD3DA8` for that
frame, and the connection ended after the following keep-alive. This confirms
that the observed direction is effectively one-way in the current client; it
does not yet prove a business purpose.

`38000` is analytics telemetry in the recovered protocol structure. The
current local server logs it as unsupported and produces no response. A bounded
run is still needed to establish whether silence is sufficient after `14300`
has also been handled.

## Real-server profile frame

The 2026-10-29 real-server capture provides a non-fixture `23245` frame:

| Artifact | SHA-256 |
| --- | --- |
| Captured sealed frame, 133867 bytes | `49B65C687DA0AAFF73FBD5776BE996F35AD04599D4252C9B25A2185D1335CFCF` |
| Decrypted body, 133844 bytes | `16D983559A639552ABDBEB43814EA64FD29976086A2AAC6D55255107362BBD2E` |

The header is `5A CD / 02 0A E4 / 00 0A`: message ID 23245, a 133860-byte
payload length, and version 10. That payload length covers the 16-byte
authentication tag and the 133844-byte ciphertext. The plaintext then splits
into object A at `0..133694`, object B at `133694..133831`, and five outer
VarInt32 values at `133831..133844`.

The field trace executed 23 current-SO decode helpers and directly hit 48
member names. Cross-walking all 79 IPA bindings against the current SO then
found exact diagnostics and decoder-closure reachability for all 74 unique
names. That does not identify 74 unique wire positions; multi-member helpers
and repeated names such as `m_progress`, `m_rewards`, and `m_badges` remain
ambiguous. The trace also recovered `CodexAITest` from object B and tied outer
fields `+0xAC` and `+0xB8` to a countdown and `+0xB0=-1` to the
quickmatch-cooldown path. Field-by-field evidence, confidence, and unresolved
interpretations are in [the frame schema](profile-23245-frame-schema.md).

The sealed-frame hash is not a stable identifier for the plaintext. A previous
valid delivery of the same 133844-byte plaintext had sealed-frame hash
`43DB0A4FC8AEE02A1B8480F2DE3B2AD9E2170DA16711C11F7CC33F0F934AD01B`.
The current capture has a different sealed-frame hash while the plaintext hash
is unchanged, consistent with per-frame authenticated-encryption material and
nonce variation.

## Corrections

The following prior statement is withdrawn:

> Frida observed `23245` enter the dispatcher at `0xADD134`.

`0xADD134` came from a different `libg` snapshot. The Null entry is `0xAD3DA8`
and the message-specific branch is `0xAD5FCC`.

Likewise, `0x105EA74`, `0xADBC1C`, and `0xC9805C` are not Null addresses and
must not be used to name or trace the APK's `23245` object.

## Next bounded experiment

Run the exact-605 profile again with the server acknowledging or recording
`14300` and accepting `38000` as analytics without a reply. Stop when the next
distinct authenticated message appears, a local handler requires a response,
or the client performs a documented fallback. In parallel, capture the producer
call stack of `14300`; its six wire bytes alone do not identify the business
operation.

The local evidence directory is:

```text
D:\IDA Scripts\clash-royale-supply-chain\work\null-libg-analysis\profile23245-dispatch-20261028
```

Evidence hashes:

```text
frida.jsonl       929F5CE5CB0EF0D9FF1473062992EC0F64312F97A7F8EAB67DE48803D6672927
server.stdout.log 1931A08615DC7895F6458F5D9C03D25780284F4A7BB02B4F1D4C02BCEF39BCD0
23245 frame       6C53F3A4F3B40B578F1F7E3D685D03FA99C0575215C9822BA2E08EE3EBF24029
```

The exact-605-byte trace is stored in
`D:\IDA Scripts\clash-royale-supply-chain\work\null-libg-analysis\profile23245-stream-605-20261028`.
Its evidence hashes are:

```text
frida.jsonl       4B7BE04DC39D5D7FCBB62D59583A0484D1F0F38D26864E51F28E822AD2DC632B
server.stdout.log C0540DAA0BAB5A8709A59E08C164D2398C7A23AE0C3C115E40617E7B3C5FA421
23245 frame       E409F88618A56DF73E73B82CF94CB99A5E36C0FD22C9AAA857C8A200BDBC7E4F
```

The later callback trace is:

```text
D:\IDA Scripts\clash-royale-supply-chain\work\null-libg-analysis\profile23245-callback-605-20261028
frida.jsonl       91F26525470EDFEE7B7B52940422360AE698DEF7435B1D0780E369E85F48D28A
server.stdout.log 039AEEF997F421C93999A60A8D07093DCD1A8C9DB0E4772BC3E3CEACB4A9420A
```

The `14300` encode trace is:

```text
D:\IDA Scripts\clash-royale-supply-chain\work\null-libg-analysis\profile14300-encode-20261028
frida.jsonl       C07C78C74CD79BAD54EB06CDB9CD80741D2C16B25900CE6D4F23EDD75B446833
server.stdout.log B0C6FDE42FAA0E0619619ED1DAEB593EB9A108A2CDEE8F07C2470CCCF6A28DFD
```

The inverse clientbound probe is:

```text
D:\IDA Scripts\clash-royale-supply-chain\work\null-libg-analysis\profile14300-clientbound-20261028
frida.jsonl       0EB64D7C56BD7B1730E2C9406D83938C6510C6684742A239982756881898CF85
server.stdout.log 44840B4C16710ED05331B4C393E7193C5A480478441EE15E8038575157AE0AF8
```
