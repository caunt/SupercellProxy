# `23245` three-binary joint analysis

Date: 2026-10-30

## Scope

This note compares three independent artifacts around the startup/profile
object graph:

| Artifact | Path | SHA-256 |
| --- | --- | --- |
| IPA main executable | `D:\Clash_Royale` | `D0F2C8E55A90D1856B240A6DB16654DE99D8BBD64607613A4BA92DDC947C14B5` |
| Null runtime `libg.so` | `D:\IDA Scripts\clash-royale-supply-chain\artifacts\extracted\15.535.13-android-a6693e925c082a46\aarch64\libg.so` | `110AA2B5CAC391C498645E072B0D88729428C2C2845E7E2737CA8EE979059783` |
| Older official `libg.so` analysis snapshot | Retained DB/IDB under `D:\IDA Scripts\ida-ai\agent_reports\libg-protocol-20261023`; the source `D:\libg_fixed.so` is currently unavailable, so this study did not reopen the source file | `B1CAF6D94F7EFFAFEC4900A78F5D96DEFD98364B082A3CF5E560622A6858993C` |

The IPA is the main Mach-O executable, not its frameworks or resource
bundles. Android and iOS absolute addresses, offsets, and type-dependent
layouts remain build-specific.

The recovered wire split for the captured Android startup package remains:

```text
23245 / version 10
  Object A: 133694 bytes, Home/game-state-family strong candidate
  Object B:    137 bytes, ClientAvatar-family strong prefix candidate
  outer:         5 VarInt32 fields
```

`23245` is still an OwnHomeData-family candidate. The exact current C++ class
name is not proven.

## What each binary contributes

### Null runtime `libg.so`

The live capture supplies the actual bytes and decoder execution order. It
proves:

- Object A is a `0x570`-byte state object.
- Object B is a `0x208`-byte object.
- Object B starts with three LogicLong values and the bounded display-name
  string `CodexAITest`.
- Object B byte `+0x1E` is a VarInt registry selector for nested object type
  `0x36`, not a normal count or a transplanted Arena/name-change flag.
- Object B contains nine collection handles. Their element objects are
  `0x20` bytes with a fixed record/handle and a VarInt value.
- The 79 IPA bindings normalize to 74 unique HomeState member names and 39
  decode helpers. All 74 names have exact diagnostics in the current SO and
  are reachable from the current HomeState decode closure; 48 of them were
  observed directly in the live field trace.

### Older official `libg.so`

The older Android build supplies independent control-flow evidence:

- Object A is initialized and decoded with size `0x570`.
- Object B is initialized and decoded with size `0x208`.
- The Null and official Object B decoders are structurally equivalent after
  address normalization.
- The Object B vtable, constructor, encoder, and decoder establish storage
  offsets independent of the live capture.
- The outer decoder reads five VarInt32 values after the two nested objects.

### IPA main executable

The IPA supplies a third serialization implementation and, importantly,
independent symbolic/diagnostic evidence.

The startup/profile path is:

```text
0x100374BC8 profile-response handler
  -> loc_100461F9C or 0x1004620B4
     -> 0x100749244
```

`0x100749244` reads version literal `11`, allocates exactly `1376 = 0x560`
bytes, constructs the Home-state-family object through
`0x1006A6AFC`, and calls its decoder at `0x1006A8B00`.

The IPA ClientAvatar-family object is exactly `0x208` bytes:

```text
constructor   0x10046FC30
vtable        0x1014532E0
encode        0x100470058
decode        0x10047094C
```

The verified vtable slots are:

| Slot | Function | Meaning |
| --- | --- | --- |
| `+0x00` | `0x100476A58` | destructor |
| `+0x08` | `0x100476AAC` | deleting destructor |
| `+0x10` | `0x10046FE00` | release owned members |
| `+0x18` | `0x100470058` | encode |
| `+0x20` | `0x10047094C` | decode |
| `+0x28` | `0x10046FB34` | set owner/context |
| `+0x30` | `0x1004757CC` | return byte at `+0x19C` |
| `+0x38` | `0x10046FB44` | decode/encode round-trip probe |

The `0x560` IPA Home-state object and `0x570` Android Home-state object are a
strong family-level bridge. Their size difference and different build ABI
forbid copying offsets or complete types between them.

The IPA LoginOK ID `24742` is not a startup `23245` alias. Android LoginOK is
`20963`. The `24742` value helps identify the login boundary but does not
establish an IPA startup-message identity corresponding to Android `23245`.

## Object B wire semantics

The current capture and both Android decoders agree on this prefix:

| B offset | Encoding | Meaning | Confidence |
| --- | --- | --- | ---: |
| `+0x00..+0x0E` | three `LogicLong` pairs | repeated `(0, 0x04426004)`; account/avatar/home roles unproven | 0.93 |
| `+0x0F..+0x1D` | `u32be` length + UTF-8 | display name `CodexAITest` | 0.94 |
| `+0x1E` | VarInt | arena-family registry selector, observed value `2`; object type `0x36` resolves class 54 (`arenas.csv`) and selects `Arena2` | 0.97 |

The selected arena row is stored at Object B `+0x160`; the decoder separately
derives the neighboring row at `+0x168`. The next wire segment begins with 16
initial VarInt observations. Their current storage and business semantics are
not all uniquely mapped. The final storage map therefore preserves every raw
field and callsite, and assigns a specific storage path only where the decoder
sequence and current evidence close the connection.

Nine collection groups follow. The current capture observes:

```text
wire +47: value 9, decoded and discarded
outer collection handles: fixed loop count 9
group 0 count: 2
  5000001 -> VarInt 500
  5000002 -> VarInt 10
groups 1, 2, 3: count 0
group 4 count: 1
  5000008 -> VarInt 8
groups 5, 6, 7, 8: count 0
```

The current 15.535.13 resources table resolves these GlobalIDs:

```text
5000001 -> Resources[1] = Gold, value 500
5000002 -> Resources[2] = ChestOrderIndex, value 10
5000008 -> Resources[8] = CardCount, value 8
```

GlobalID class decoding uses `id / 1,000,000` and item decoding uses
`id % 1,000,000`; class 5 is registered as `csv_logic/resources.csv`. The
captured value is the resource-typed protocol value for that row. It is not
safe to define every value as a generic "count": `ChestOrderIndex=10` is an
index/tier-like value.

The corrected 137-byte storage map and the HomeState member map are in the
local evidence workspace:

```text
D:\IDA Scripts\ida-ai\agent_reports\libg-profile-23245-joint-20261030
\object-b-storage-map.json
\homestate-member-map.json
```

The final storage map preserves all trace fields, fixed-width gaps, callsites,
capture values, proven storage paths, confidence, and unresolved status. Its
SHA-256 is
`DBEBEA6C408065E34A807F5739F604479EF8FED4D53A49AC99155E88994AFDD8`.
The member map SHA-256 is
`BB33BF3DDBEF4D63AD44CDAE40DD5DA57A882FAAA016EF4066ABF23AA7AAEA79`.
The older `object-b-ordinal.json` hash
`1FCF8483B1FF585D6944A1F8F04BF266F6EC51BDDA825AFBDC76B033D1606E5B`
is retained as history only and is not the corrected schema.

## Object A named helper groups

The current SO exposes exact diagnostics for all 74 unique IPA member names
across 39 current decode helpers. A multi-member helper provides group
evidence. It does not prove that each diagnostic name occupies a unique
parent-object offset. The verified count of unique wire positions is still
zero, so the final map is deliberately a reachability map rather than a
fabricated field-by-field wire schema.

The following parent offsets are stronger because the Home-state decoder calls
the named helper directly on the field at that offset:

| Object A offset | Current direct-call evidence |
| --- | --- |
| `+0x2C0` | `m_badgesShownInProfile`, `m_pendingNotSeenUpgradedBadges` |
| `+0x308` | inventory collection family: cards, decks, chests, banners, shards, consumables, and related collections |
| `+0x3E0` | `m_consumableCounts`, `m_consumableIds`, `m_maxedConsumables`, `m_newUnseenConsumables`, `m_seenConsumables` |
| `+0x3E8` | `m_masteriesProgress` |
| `+0x3F0` | `m_itemsGet`, `m_itemsGive`, and spell-use arrays |
| `+0x428` | `m_newBanners`, `m_chronosBannerOverrides`, `m_ownedBanners`, `m_favoriteBanners` |
| `+0x440` | `m_collectedOneTimeRewardsLeagueDataIndices`, `m_collectedSeasonalRewardsLeagueDataIndices` |
| `+0x448` | `m_eventDecks`, `m_eventDecksHaveBeenEdited`, `m_deckEventIds`, purchased reward arrays |
| `+0x450` | `m_keys`, `m_values` |
| `+0x458` | `m_previousSeasonPassUncollectedNodes` |
| `+0x490` | `m_chainOfferStates` |
| `+0x4A0` | `m_trails` |
| `+0x4E8` | `m_claimedStepIndices` |
| `+0x4F0` | `m_popupOpenedEventIds` |
| `+0x4F8` | opened deep-link event IDs and end timestamps |
| `+0x500` | `m_progress` |
| `+0x508` | `m_activeStates`, `m_unclaimedRewards` |
| `+0x530` | owned/new ruler skins and VFX arrays |
| `+0x538` | `m_augments` |

The partial Home-state types expose these as group boundaries with explicit
gap bytes. They intentionally do not invent a unique type for every helper
member.

## Important outer fields

| Message offset | Captured value | Current interpretation | Confidence |
| --- | ---: | --- | ---: |
| `+0xA8` | 0 | unknown | 0.30 |
| `+0xAC` | 1793252299 | timer base/session timestamp candidate | 0.92 |
| `+0xB0` | -1 | quickmatch cooldown disabled/expired sentinel | 0.88 |
| `+0xB4` | 0 | unknown | 0.30 |
| `+0xB8` | 1794525130 | timer end/season-end candidate | 0.86 |

`+0xB8 - +0xAC` is `1,272,831` seconds, approximately 14 days 17:33:51.
Keep both source values and derive the interval.

## IPA member-name evidence

The older schema recovery found 79 bindings and 74 unique names for message
`23245`:

```text
D:\IDA Scripts\ida-ai\agent_reports\libg-protocol-20261023
\ipa-recovery-20261025\ipa-schema-field-recovery-20261025.json
```

All 74 unique names have exact diagnostics in the current Null SO and are
reachable from its current HomeState decoder closure. The live field trace
directly hit 48 of them; reachability added the remaining 26 without turning
them into unique wire fields.

The helper paths retain two important ambiguities:

- `m_badges` occurs in both inventory and profile-badge contexts.
- `m_progress` and `m_rewards` each occur in multiple independent helpers.

Those names must not be collapsed into one offset or one database column.
Names remain helper-scope unless direct data flow maps a specific occurrence
to a parent offset.

## Current conclusions

1. Android official and Null Object B are structurally equivalent.
2. The IPA independently proves a ClientAvatar-family `0x208` object and a
   Home-state-family `0x560` object.
3. Object B's identity/name prefix is strong enough to use for player-profile
   parsing.
4. B+0x1E is an arena-family registry selector and must not be interpreted as
   an old ClientAvatar boolean or collection count.
5. `23245` has strong startup/Home-family evidence, but the exact
   `OwnHomeDataMessage` class identity remains unproven.
6. `500000x` is the `resources.csv` GlobalID namespace, not an unresolved
   dynamic-instance namespace.
7. Most post-prefix fields need controlled variant captures or consumer proofs
   before they receive business names or database constraints.

The next useful capture changes one controlled profile dimension, such as the
display name or one resource value, while keeping the same binary and message
version. A byte-by-byte diff should be used to resolve occurrence roles and
collection consumers.
