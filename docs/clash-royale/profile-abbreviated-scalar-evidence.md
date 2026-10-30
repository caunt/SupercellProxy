# Clash Royale `libg` abbreviated profile scalar evidence

This note records a bounded, same-build follow-up for the abbreviated
ClientAvatar-family profile keys `scr`, `lti`, `spi`, `spt`, `welo`, and
`rnk_ps`. The target is the older official Android `libg` analysis snapshot
under `D:\IDA Scripts\ida-ai\agent_reports\libg-protocol-20261023`.

The rule used here is:

1. start from the exact keyed encoder/decoder binding;
2. follow a getter or setter that acts on the same profile object;
3. require an independent command, delivery, persistence, or UI consumer;
4. keep an abbreviation unresolved unless that consumer proves the meaning.

Job 36 in `libg.sqlite` stores the corresponding claims and evidence. It is a
sibling of job 35, which remains dedicated to `rnk_sc`/`rnk_ps`.

## Result summary

| Key | ABI offset | Proven structural behaviour | Defensible label | Still unresolved |
| --- | ---: | --- | --- | --- |
| `lti` | `+0x58` | Monotonic gate; a new value must be greater than the current value before progression advances | Monotonic progression index/value | Exact tutorial, trail, season, shop, or ladder domain |
| `spi` | `+0x194` | Receives the current `lti` value when a shop purchase records progression context | Latest progression snapshot | Expansion of the letters and unrelated uses of the field |
| `spt` | `+0x198` | Written immediately after `spi`; observed command result maps to a small enum value `1..3`; delivery restores or clears the pair | Enum-like tag paired with `spi` | The meaning of tag values `1`, `2`, and `3` |
| `scr` | `+0x88` | Profile UI copies it and displays it through either the `rating` or `trophies_autochess` node; a profile-list consumer takes the maximum against another computed score | Score/rating value | Original expansion of `scr` and whether one exclusive game-mode meaning exists |
| `welo` | `+0x130` | Keyed inside the optional clan block; copied to profile UI `+464`; three command classes and one reset path write it | Clan-adjacent profile state | Exact value semantics; no decoded UI consumer was found |
| `rnk_ps` | `+0x1D8` | Keyed name, width, and ABI offset are proven | Unresolved rank-related scalar | Any specific previous/peak/season meaning |

The table deliberately avoids names such as `lastTutorialIndex`, `seasonPoints`,
`warElo`, or `previousSeasonScore`. Their first letters or record adjacency do
not establish those meanings.

## `lti`

The profile getter is `0xCF26F8` and the setter is `0xCF2700`.

In command execute function `0xD74EA0`:

```c
requested = command->field_0x2C;
if (profile_lti(profile) >= requested)
    notify_listener_without_advancing();
else {
    old_scr = profile_scr(profile);
    changed = update_progression(profile, world, requested, ...);
    notify_listener(requested, old_scr, changed);
}
```

The bounded updater `0xD74F78` repeats the monotonic check:

```c
if (profile_lti(profile) >= requested)
    return 0;
```

On an accepted update it writes `lti`, uses `scr` as the prior score in
threshold and scaling calculations, can update `scr`, and then enters season
transition paths including `0xFF61B4`.

This proves a monotonic progression update. It does not prove what the index
counts.

## `spi` and `spt`

The getters/setters are:

| Key | Getter | Setter |
| --- | ---: | ---: |
| `spi` | `0xCEB7BC` | `0xCEB7B4` |
| `spt` | not recovered in this pass | `0xCEB7C4` |

In `LogicShopItemPurchaseCommand` execute path `0xD76E6C`, the profile is
updated as one logical pair:

```c
current_progression = profile_lti(profile);
profile_set_spi(profile, current_progression);
profile_set_spt(profile, enum_tag);       // observed values 1, 2, or 3
```

The enum value is derived from a command result in the range `261..263`, then
mapped as:

```text
261 -> 1
262 -> 2
263 -> 3
```

`LogicDeliverableUtils::deliver` at `0xEB5A04` independently reads adjacent
fields from a delivered record. It restores them through the `spi` and `spt`
setters only when the record's progression value equals profile `lti`; another
branch resets both profile fields to zero.

This is strong evidence that `spi/spt` form a progression snapshot plus tag
pair. It is not evidence that `spt` means season points or that `spi` means shop
purchase index.

## `scr`

The profile getter is `0xCECE30`.

The profile UI constructor `0xA8339C` copies:

```c
*(uint32_t *)((uint8_t *)ui + 352) = profile_scr(profile);
```

At UI method `0xA83C44`, UI `+352` is formatted as a signed decimal and routed
to one of two UI nodes depending on the mode branch:

```text
rating
trophies_autochess
```

The formatter at `0x13999D8` only converts a signed integer to text. The
independent profile-list consumer `0xC81108` is more useful: it calls profile
UI vtable slot `+496`, which resolves to the UI getter for `+352`, compares the
value with a computed score, and applies a maximum operation.

Together these observations support `score/rating value`. The exact original
expansion and a single exclusive mode meaning remain open.

## `welo`

The profile getter is `0xCEE064` and the setter is `0xCEE05C`.

The keyed decoder places `welo` in the optional clan block:

```text
wire +84 clan-present flag
  clan_id   -> +0x108
  badge     -> +0x110
  clan_name -> +0x120
  welo      -> +0x130
```

The profile UI constructor copies it into UI `+464`. Three LogicCommand classes
write it from command fields:

| Factory case | Constructor | Execute path |
| --- | ---: | ---: |
| `0x2BA` | `0xD6EB18` | `0xD6EB48` |
| `0x307` | `0xD73240` | `0xD7327C` |
| `0x317` | `0xD7367C` | `0xD736B0` |

`0xD736B0` also contains a reset path that writes `-1`.

The clan-block position is exact, but it is insufficient to name the value.
No decoded consumer for UI `+464` was found in this bounded pass.

## `rnk_ps`

The key and `u32` storage at `+0x1D8` remain proven. The reviewed progression,
profile-UI, purchase, and delivery paths provide no direct consumer for this
offset.

The official seasonal UI reads previous-season values at `+0x1EC/+0x1F0` and
best-season values at `+0x1FC/+0x200`, not `+0x1D8`. Therefore
`previousSeasonScore`, `bestSeasonScore`, and `peakLadderScore` remain rejected
as unsupported interpretations.

## Practical storage labels

Until a later consumer closes the exact business meanings, persistence and
codec models should use neutral structural names:

```text
profile_progression_index   <- lti/+0x58
profile_progression_snapshot<- spi/+0x194
profile_progression_tag     <- spt/+0x198
profile_score_rating        <- scr/+0x88
profile_clan_scalar         <- welo/+0x130
profile_ranked_scalar       <- rnk_ps/+0x1D8
```

These labels describe observed roles without replacing the wire keys.
