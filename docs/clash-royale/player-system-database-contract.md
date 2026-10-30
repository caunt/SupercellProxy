# `23245` player-system database contract

Date: 2026-10-30

## Scope

This note turns the current `23245/v10` startup findings into requirements for
the Null's Royale server's player-system persistence. It covers only the
startup/home/profile boundary. It does not define battle, matchmaking, clan, or
post-login gameplay schema.

Authoritative current-runtime identity:

- `libg.so`: `110AA2B5CAC391C498645E072B0D88729428C2C2845E7E2737CA8EE979059783`
- decrypted `23245` plaintext:
  `16D983559A639552ABDBEB43814EA64FD29976086A2AAC6D55255107362BBD2E`
- external comparison:
  `D:\IDA Scripts\clash-royale-supply-chain\work\null-libg-analysis\profile23245-real-field-trace-20261029\external-reference-comparison.json`
  (`07985BF562A64741C8D7F0285F321C95DDE520FFAAC1165A9DCF987C1FF9562F`)
- Job VCS: `libg-null-runtime-110aa2b5`, jobs 1/2; use `job-resume` for the
  current revision after the three-binary refresh

## Field meanings that are safe to use

`23245/v10` is an OwnHomeData-family startup candidate. Its exact current
message class is still unproven.

| Wire location | Safe meaning | Boundary |
| --- | --- | --- |
| Object A, `0..133694` | Large Home/account-wide game-state object | Family-level match; current prefix and detailed layout are build-specific |
| Object B, `0..137` | Small player/profile object | Strong `ClientAvatar`-family prefix match |
| Object B `+0..+14` | Three repeated LogicLongs `(0, 71458820)` | Current logic-ID component roles are unproven |
| Object B `+15..+29` | `u32be` length 11 followed by UTF-8 `CodexAITest` | Current player display name |
| Object B `+0x1E..` | Current-build-specific profile fields | Selector at `+0x1E` uses object type `0x36`, resolves table class 54 (`arenas.csv`), and selects `Arena2`; storage is `+0x160`, with a derived neighbor at `+0x168`. Astral `ClientAvatar` continuity ends before this selector |
| Outer `+0xAC` and `+0xB8` | Timer base and end timestamp | Store both source values; treat the interval as derived |
| Outer `+0xB0` | Quickmatch cooldown disabled/expired sentinel candidate | Signed `-1`; UI consumer supports this meaning |
| Outer `+0xA8`, `+0xB4` | Unknown | Do not assign business names or constraints |

Object A contains fixed `u32be` values `26000000`, `26000001`, `26000005`,
`28000001`, `28000000`, and `26000003` in its observed head/deck region. They
belong to the card-content SCID domain.

Object B has a separate collection structure. Wire `+47` is read and discarded;
the decoder then iterates a fixed nine collection handles. Each handle has its
own count. The current capture's counts are `2,0,0,0,1,0,0,0,0`, and each wire
item is a fixed `u32be` GlobalID plus a VarInt typed value. The current
15.535.13 resources table resolves the observed records as:

```text
5000001 -> Resources[1] Gold,            value 500
5000002 -> Resources[2] ChestOrderIndex, value 10
5000008 -> Resources[8] CardCount,       value 8
```

GlobalID class 5 maps to `csv_logic/resources.csv`. Persist the GlobalID and its
resource value as distinct typed fields. Do not call every value a generic
count; `ChestOrderIndex` is an ordered/tier-like value. The consumer routing for
the two nonempty resource groups is still unresolved.

## Required persistence model

### 1. Preserve evidence and protocol snapshots separately

Keep these independent:

- raw decrypted profile snapshot;
- ordered decoded field observations;
- typed player/home domain state;
- protocol schema identity;
- analysis confidence and unresolved status.

Every imported startup snapshot should record:

- `libg_sha256`;
- `message_id` and `message_version`;
- plaintext SHA-256 and byte length;
- schema/external-comparison artifact hash;
- import time and idempotency key;
- parser/generator version.

Do not make mutable analysis snapshots a code-generation dependency.

### 2. Model LogicLong without inventing roles

Store LogicLongs as two unsigned 32-bit VarInt components, for example
`logic_id_hi` and `logic_id_lo`. A convenience composite value may be derived,
but it must not replace the two source components.

The three repeated Object B values use the same pair. AstralRoyale names the
components `highID` and `lowID`, but the current build does not yet prove
whether each occurrence is an account ID, player/avatar ID, home ID, or an
ownership reference. Preserve occurrence order and a provisional relation
label such as `avatar_identity_0/1/2` until consumers prove the roles.

Do not:

- reduce the ID to a signed 64-bit value where sign behavior becomes ambiguous;
- label the two components account ID and home ID by assumption;
- use display name as a primary or unique player key.

### 3. Separate content IDs from instance IDs

Use distinct domain types and columns for:

- `CardContentScid`: immutable content/card definition;
- `CardInstanceId`: a concrete player-owned card occurrence.

Even when an instance later resolves to a content SCID, retain the original
instance ID. A database foreign key must not be added until a current-build
mapping producer or decoder proves it.

### 4. Store the known profile prefix as typed state

The initial player/profile model should support:

- player identity components;
- display name;
- the three repeated startup identity references, preserving order;
- current profile schema/revision;
- timestamps required by the session;
- source snapshot reference.

Do not promote `B+0x1E=02` to a name-change flag or trophy count. Current code
and table registration prove an arena-family selector resolving `Arena2`;
that does not prove every later object field. Those old meanings end at the
confirmed 30-byte prefix.

### 5. Preserve unknown fields losslessly

Persist the nine Object B fixed collection handles and their item boundaries
as ordered observations. Each item keeps the GlobalID and VarInt value in
separate columns or typed components. Persist the remaining unknown regions as
ordered, offset-addressed observations containing:

- object (`A` or `B`);
- relative offset and length;
- encoding/wire type;
- raw bytes or canonical scalar value;
- provisional label;
- evidence status and confidence;
- schema/evidence reference.

The raw plaintext snapshot remains the lossless source of truth. An exact
raw-profile replay/round-trip check is required before semantic re-encoding is
introduced.

### 6. Normalize only proven collections

The Object B outer vector and item boundaries are proven: a discarded `+47`
value, nine fixed collection handles, per-handle counts, then GlobalID plus
VarInt value for each item. Class 5 is proven to be the resources table.
Normalize the structure, but model each resource independently because
`Gold` and `ChestOrderIndex` have different semantics.

Object A has current-SO helper-scope names for cards, decks, inventory, events,
rewards, banners, badges, trails, and seasonal progress. All 74 unique
IPA-derived names have exact current-SO diagnostics and reachable decoder
paths, but the number of uniquely proven wire positions is still zero. A helper
can encode several sibling arrays, so helper names do not yet prove unique
parent offsets.

Create typed child tables only after a count and element boundary are verified.
Before then, retain repeated records in the offset-addressed observation layer.
Do not duplicate one repeated helper execution into hundreds of parent fields.

### 7. Use UTC integer timestamps

Store `+0xAC` and `+0xB8` as `int64` UTC Unix seconds from the wire. Preserve
both source values. The observed difference `1,272,831` seconds is derived
presentation state and should be recalculated rather than treated as another
wire field.

### 8. Keep transactions and revisions explicit

- One transaction per accepted state-changing command.
- Version player/profile/home rows and migrate forward.
- Keep account/economy audit records append-only.
- Make startup-profile import idempotent.
- Reject malformed, truncated, or over-limit fields without dropping raw
  evidence.
- Keep transport/session state separate from persistent player state.

## Minimum conceptual tables

Names are implementation choices, but the responsibilities must remain
separate:

| Responsibility | Required fields or relationships |
| --- | --- |
| Account | internal stable key; authentication linkage; current player relationship |
| Player/profile | internal key; LogicLong components; display name; profile revision |
| Startup profile snapshot | libg/schema identity; message version; raw plaintext hash/bytes; import key; corrected Object B storage-map hash |
| Decoded field observation | snapshot key; object/offset/encoding/value/status/confidence |
| Home state | player key; schema revision; timers; versioned aggregate state |
| Card content | immutable card-content SCID and content revision |
| Card instance | player key; dynamic instance ID; content-SCID mapping kept nullable |
| Decks | only after exact boundaries are proven; ordered slots with instance references |
| Resource | GlobalID; class/table identity; resource name; typed protocol value |
| Inventory/events/banners/rewards | normalize incrementally from proven collection boundaries |

## Acceptance requirements for the server work

The first player-database slice is acceptable when it can:

1. import the captured `23245/v10` plaintext without losing unknown bytes;
2. expose player name and all three LogicLong components as typed data;
3. expose both timer source values;
4. expose Gold, ChestOrderIndex, and CardCount through the resources GlobalID
   namespace while retaining their values as typed protocol data;
5. keep card-content SCIDs and dynamic instance IDs in different types/tables;
6. reproduce the original raw snapshot exactly;
7. show schema/evidence hashes and import provenance;
8. fail closed on malformed fields rather than guessing;
9. avoid claiming a complete dynamic profile or a proven exact
   `OwnHomeDataMessage`/`ClientAvatar` class until current consumers establish
   the remaining boundaries.

The next evidence-producing experiment is a second valid startup package from
the same build with a changed display name or one changed resource value.
That diff should drive the first constrained typed update instead of guesswork.

## Related evidence

- [Field and external comparison](profile-23245-frame-schema.md)
- [Runtime dispatch](profile-23245-runtime-evidence.md)
- [Startup to main interface](startup-to-main-interface.md)
