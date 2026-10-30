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

The strongest new bridge is the signed profile key namespace. Null, the older
official Android build, and the IPA all expose the same 19 logical keys
(`accountID.hi`, `accountID.lo`, `name`, `arena`, `scr`, `expLevel`, and the
other entries listed below). This lets the stored ABI offsets be joined to a
semantic key before the positional Object B decoder supplies the exact wire
range. Offset-only searches were retained as discovery leads, but are not
treated as field proof because unrelated objects and structures reuse the same
small offsets.

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
- The keyed profile decoder at `0xCE1DA4` binds 15 currently present fields to
  exact wire ranges after the positional decoder fixes the wire cursor.
- The same decoder selects four conditional clan fields when the wire `+84`
  flag is true. The current capture has the flag false, so those fields have
  known key/ABI bindings but no observed values.
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
- The official build contains the same 19-key profile namespace at
  `0xCF4240`, including the 15 active and four conditional field keys.
- Its compact profile namespace at `0xD16E6C` and persistence-key initializer
  at `0xFB13EC` independently preserve the cross-version string families.
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

The IPA also contains the positional Object B encoder and decoder at
`0x100471AD0` and `0x100471C80`, the full decoder at `0x10047094C`, the
19-key initializer at `0x100476AF8`, the compact profile namespace at
`0x10048EF80`, and the persistence-key initializer at `0x1006B41C8`. These
addresses corroborate semantics through independently compiled code while
remaining build-specific.

The `0x560` IPA Home-state object and `0x570` Android Home-state object are a
strong family-level bridge. Their size difference and different build ABI
forbid copying offsets or complete types between them.

The IPA LoginOK ID `24742` is not a startup `23245` alias. Android LoginOK is
`20963`. The `24742` value helps identify the login boundary but does not
establish an IPA startup-message identity corresponding to Android `23245`.

### IDA type-layout correction

The first local type draft declared `NullLibgMessageStringStorage` as three
32-bit values plus an 8-byte pointer. IDA therefore padded it to 24 bytes.
Current ABI stores prove that this helper is 16 bytes: two leading `u32`
values followed by an 8-byte pointer. That pointer's high half was previously
misread as a fourth `u32`.

After correcting the helper and reapplying the four startup function
prototypes, the IDA-reported sizes are:

```text
NullLibgMessageStringStorage                 0x10
NullLibgMessage_23245_ClientAvatarCandidate  0x208
NullLibgMessage_23245_HomeState              0x570
NullLibgMessage_23245                        0xC0
```

Hex-Rays now emits `scr_0088`, `expLevel_00e8`, `npc_019c`, and
`birth_date_01a0` at their matching instruction stores. This correction matters:
using the original 24-byte helper would silently shift most fields after the
display name or HomeState string by eight bytes. The read-only IDA type-layout
check is retained locally at
`type-layout-rerun-ida.log`, SHA-256
`7588B6E44B798F731FCA0F0A5C160AD11929BD9ED8B12C0098D2D6F25FAEE49A`.

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

### Closed keyed-profile ranges

The current capture closes these 15 keyed values. The `wire range` is relative
to the 137-byte Object B segment, while `ABI` is relative to the current Android
Object B.

| Wire range | Key | ABI | Type | Captured value | Interpretation boundary |
| --- | --- | ---: | --- | ---: | --- |
| `+5..+6` | `accountID.hi` | `+0x20` | VarInt32 | `0` | Key label and storage are proven |
| `+6..+10` | `accountID.lo` | `+0x24` | VarInt32 | `71458820` | Key label and storage are proven |
| `+15..+30` | `name` | `+0x78` | length-prefixed string | `CodexAITest` | Current display-name field |
| `+30` | `arena` | `+0x160` | registry VarInt | `2` | Resolves `Arena2`; derived neighbor at `+0x168` |
| `+31` | `scr` | `+0x88` | u32 | `0` | Keyed name proven; gameplay expansion unresolved |
| `+79` | `expPoints` | `+0xEC` | u32 | `0` | Keyed name proven |
| `+80` | `totalExpPoints` | `+0xF0` | u32 | `0` | Keyed name proven |
| `+83` | `expLevel` | `+0xE8` | u32 | `1` | Keyed name proven |
| `+96..+100` | `spi` | `+0x194` | u32be | `0` | Keyed name proven; expansion unresolved |
| `+100..+102` | `lti` | `+0x58` | VarInt32 | `6047` | Keyed name proven; expansion unresolved |
| `+102` | `npc` | `+0x19C` | bool | `false` | Keyed name and boolean read proven |
| `+103..+108` | `birth_date` | `+0x1A0` | VarInt32 Unix seconds | `946674000` | UTC `1999-12-31T21:00:00Z` |
| `+123` | `rnk_sc` | `+0x1D0` | VarInt32 | `0` | Keyed name proven; expansion unresolved |
| `+125` | `rnk_ps` | `+0x1D8` | VarInt32 | `0` | Keyed name proven; expansion unresolved |
| `+136` | `spt` | `+0x198` | VarInt32 | `0` | Read last although it precedes several ABI fields; wire order is authoritative |

Wire `+31` is therefore `scr` at `+0x88`; wire `+32` is a separate conditional
scalar stored at `+0x158`. The previous reverse mapping was wrong.

When wire `+84` is true, the keyed decoder reads `clan_id` into `+0x108`,
`badge` into `+0x110`, `clan_name` into `+0x120`, and `welo` into `+0x130`.
The current capture's `+84` flag is false, so the key order and ABI offsets are
known but no wire values are present to verify.

`spi > 0 && spi == lti` is a code-proven predicate. In this capture the
predicate is false (`0 != 6047`); its UI consequence remains unproven.
The `birth_date` value is consistent with a new-account sentinel, but no current
consumer proves that interpretation.

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
\object-b-storage-map-v2.json
\profile-field-crosswalk.json
\homestate-member-map.json
```

The final storage map preserves all trace fields, fixed-width gaps, callsites,
capture values, proven storage paths, confidence, and unresolved status. Hashes:

```text
object-b-storage-map-v2.json
6623EB91EA9171A5CD2FBB618498C718C8E41690C57EC8ADC70EE05A4B6A411A

profile-field-crosswalk.json
7502A0DCB439E5EE3CAD2500BA6B71B63EFBC7AA2FFD2A97C8893C1075A29009

homestate-member-map.json
9A7FA9B82E1461FEAB0E2B4743393EC0AEF1DF724F8F8A66454E8B4122BD26EB
```

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
8. The keyed codec closes 15 current fields and four conditional keys, but it
   does not expand ambiguous abbreviations such as `scr`, `spi`, `spt`, `lti`,
   `welo`, `rnk_sc`, or `rnk_ps` into safer gameplay labels.
9. Persistence strings such as `accepted_tos_version`, `new_user`, and
   `tutorial_state` are real cross-binary key literals. They are not yet mapped
   to Object B offsets or proven to trigger a specific startup UI.
10. Offset-only consumer scans are insufficient for this object. A candidate
    must preserve receiver identity or be corroborated by a keyed codec.

The next useful capture changes one controlled profile dimension, such as the
display name or one resource value, while keeping the same binary and message
version. A byte-by-byte diff should be used to resolve occurrence roles and
collection consumers.
