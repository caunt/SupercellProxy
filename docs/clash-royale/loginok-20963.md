# Current Clash Royale `libg`: LoginOk `20963`

This note records current-build evidence for the clientbound LoginOk message.
It applies only to the analyzed `libg.so`:

```text
SHA-256
110AA2B5CAC391C498645E072B0D88729428C2C2845E7E2737CA8EE979059783
```

Message IDs, object offsets, and optional-block shapes must be rechecked before
using this note for another release.

## Relationship to the repository contract

The current message is not the `Release17286` LoginOk registered at `25220`.
It is clientbound `20963` (`0x51E3`), version 4 in the controlled live path.
The messages have different wire order, trailing blocks, and field meanings.
`LoginOkMessage.cs` remains valid for the `Release17286` table; it must not be
selected as the current Clash Royale codec by assuming that all Supercell games
or releases share one LoginOk layout.

The generic `PlayerPermissionSet` shape remains structurally useful:

```text
VarInt state
VarInt entry count
entry count x (Boolean enabled, VarInt restriction value)
Boolean final flag
```

The business meaning of each current permission entry is not established.

## Binary identity

```text
factory/construct  0x102E8D0
decode             0x102EE18
encode             0x102E978
ID getter          0x102F994
vtable             0x18A4C70
object size        0x1B0
```

The factory constructs message `20963` in mode 0. The runtime decoder entered
`0x102EE18`, dispatch accepted the object, and encoder `0x102E978` agreed on the
same operation order.

## Wire order

The core block is:

```text
LongId                 account ID part 1                  object +0x90
LongId                 account ID part 2                  object +0x98
String?                unknown A0                         object +0xA0
String?                unknown A8                         object +0xA8
String?                unknown B0                         object +0xB0
VarInt                 unknown C0                         object +0xC0
VarInt                 unknown C4                         object +0xC4
VarInt                 unknown C8                         object +0xC8
VarInt                 unknown CC                         object +0xCC
String?                unknown D0                         object +0xD0
VarInt                 unknown D8                         object +0xD8
VarInt                 unknown DC                         object +0xDC
VarInt                 unknown E0                         object +0xE0
String?                unknown B8                         object +0xB8
String?                 server time text                  object +0xF0
String?                unknown F8                         object +0xF8
VarInt                 unknown 128                        object +0x128
```

The server-time consumer parses `+0xF0` with `atoll(...) / 1000`, so captured
values are Unix milliseconds in text. The account-ID consumer formats
`+0x90` and `+0x98` as `%d-%d`.

When payload bytes remain after the core, the client reads the optional
resource block:

```text
String?                +0x100
String?                +0x108
String?                +0x110
String?                +0x118
VarInt                 +0x12C
VarInt                 +0x120
VarInt                 +0x124
String?[]              content URLs        +0x130
String?[]              Chronos URLs        +0x138
```

Both arrays use a signed VarInt count. `-1` is null; `0..99` is accepted by the
native decoder, and a count of 100 or more aborts. Each element is a nullable
string.

When bytes still remain, the session-detail block is:

```text
Bytes?                 +0x140
Boolean                +0x148, decoded as the inverse of the wire bit
Boolean                +0x149
Boolean                +0x14A
VarInt                 +0x14C
String                 required, maximum 900000 bytes, object +0x150
String?                +0xE8
```

The final compatibility area has two parts around the permission object:

```text
String? +0x160
String? +0x168
String? +0x170
String? +0x178
String? +0x180
PlayerPermissionSet? +0x1A8
String? +0x188
String? +0x190
String? +0x198
String? +0x1A0
```

Every nullable-field presence is a packed primary Boolean bit. The permission
object itself has a separate presence bit. The `+0x180` field is easy to miss
because it appears between the first group of four flagged strings and the
permission object.

## Empty and complete bodies

An empty body is accepted and enters the LoginOk dispatch case. This is a
compatibility path, not proof that every field is absent from a normal server
login response.

A 338-byte controlled marker populated every block and every field, including
two content URL lists, five leading optional strings, nine permission entries,
and four trailing optional strings. Its SHA-256 was:

```text
C258D63BAD8815DC456E9F23E9ECB7E19681460787A7A18BCD9688FE4581C124
```

The current client decoded the marker through dispatch and sent an established,
authenticated `17537` analytics event.

The D test server then generated a complete anonymous default body with zero
account IDs, no tokens or URLs, no permissions, and all compatibility blocks
present. The live sequence was:

```text
10100 -> 20100 -> encrypted 10101
-> encrypted clientbound 20963
-> established 17537 accepted
-> established 10108 accepted
```

This closes the protocol-transition path. It does not prove the business value
or UI behavior of the anonymous profile; account, Home, content, permission,
and token composition remain separate semantic work.

## Corrections to older reference assumptions

- Current Clash Royale LoginOk is `20963`, not `25220`.
- The current core begins with two `LongId` values; the `Release17286` codec
  begins with VarInts.
- The current account ID and server-time consumers establish those meanings,
  but most current fields still have explicit offset-based unknown names.
- Current content resources have separate normal and Chronos URL lists.
- Current trailing data has five strings, an optional permission object, then
  four more strings. It is not the reference three-field session block followed
  by four uniform strings.
- The current captured Login used no 508-byte zero pad.
- Current communication uses HChaCha10 (five double rounds), ChaCha8 (four
  double rounds), and Poly1305. The `Release17286` 17/8-double-round values must
  not be transferred to this build.

## Evidence boundary

This note retains no live SharedKey, private key, nonce, login token, or
reusable session state. The implementation reference is the private-server
workspace:

```text
nulls-royale-server/source/nrserver/protocol/login_ok_20963.d
nulls-royale-server/docs/evidence/loginok-20963-20261028.md
```
