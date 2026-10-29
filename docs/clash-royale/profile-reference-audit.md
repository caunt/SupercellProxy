# Clash Royale profile support in the SupercellProxy reference

Date: 2026-10-28

> 2026-10-29 update: a local-only server using the current runtime build
> delivered the 133,844-byte decrypted `23245` object body after the `30000`
> post-consent transition. The client entered the lobby and remained stable for
> more than six minutes. Earlier failures passed the 133,867-byte sealed frame
> as though it were plaintext, causing a second transport wrapping. See
> [startup-to-main-interface.md](startup-to-main-interface.md).

## Question

Does the SupercellProxy reference contain the profile and home model needed by
the analyzed current Clash Royale client?

The useful answer is split:

- Yes, it contains a mature account/home/profile model.
- No, that model is not a current Clash Royale codec and cannot be transferred
  by message ID or field order.

| Dimension | Current result |
| --- | --- |
| Profile/home architecture | Present and useful as a reference |
| Current Clash message IDs | No exact match for the reference profile/home entries checked |
| Current profile wire schema | Not established by the reference |
| Current business field names | Only current-SO/IPA evidence can establish them |
| Likely current bootstrap message | `23245`; high-confidence profile/game-state candidate |
| Loading-screen completion | Lobby reached and held with the supplied decrypted `23245` fixture; dynamic profile generation remains unresolved |

This note uses the current `libg` target whose SHA-256 is
`110AA2B5CAC391C498645E072B0D88729428C2C2845E7E2737CA8EE979059783`.
The reference snapshot is commit `388a6e5` on the fork's Clash Royale research
branch.
The corresponding local research job is `job 32`
(`Clash Royale profile/home bootstrap boundary`) in
`ida-ai/agent_reports/libg-protocol-20261023/libg.sqlite`.

## What the reference implements

`Release17286` is a Hay Day 1.72.86 table. Its relevant registrations are:

```text
21108  AvatarProfileMessage
12127  RequestAvatarProfileMessage
24180  OwnHomeDataMessage
24489  OtherHomeDataMessage
28302  SocialProfileMessage
25220  LoginOkMessage
```

The reference has more than a screen-name model:

- `OwnHomeDataMessage` carries a server timestamp, `ClientAvatar`, two
  compressed avatar documents, and a compressed home document.
- `ClientAvatar` identifies the account and home, and models inventory,
  social state, progression, purchase history, and other account systems.
- `AvatarProfileMessage` carries a player ID, linked-account IDs, SCID, and
  farm name.
- `SocialProfileMessage` carries a player ID, linked-account IDs, farm name,
  and four still-unnamed numbers.
- `ProtocolClient.ObserveAnonymousProfile` observes `OwnHomeDataMessage` and
  `OtherHomeDataMessage`. It reads the avatar name, level, and experience, then
  resolves cash and diamond balances through data-table references.

These are useful architectural examples. They show that a current home load
must carry both identity data and compressed domain snapshots, and that a
server should not fake profile progress from LoginOk alone.

## Why it cannot be copied into the current client

The current Clash Royale schema has 340 top-level messages. It contains none of
the following reference message IDs:

```text
12127
21108
24180
24489
28302
```

It also uses clientbound LoginOk `20963`, not the reference's `25220`. The
current LoginOk begins with two LongId values and has different optional
blocks, flags, URL lists, and trailing fields. Only fields with current-build
consumers are safe to name: account part 1, pass token, session count, session
ID, cached location, and the Zendesk group. The second LongId remains a Home ID
candidate.

The current schema does contain message `24222`, whose exact reference mapping
is `FishingDataMessage`. The current top-level wrapper and nested
own-home-like codec support that relationship at the architectural level, but
the compact current payload is not wire-compatible with the reference's full
`OwnHomeDataMessage`. The detailed comparison follows below.

## Current bootstrap candidate: message `23245`

The current Null client contains a large stateful clientbound message `23245`.
The Android package and the analyzed native library must be kept as separate
identities:

| Artifact | SHA-256 |
| --- | --- |
| Current Null runtime `libg.so` | `110AA2B5CAC391C498645E072B0D88729428C2C2845E7E2737CA8EE979059783` |
| Older official-server analysis `libg` | `B1CAF6D94F7EFFAFEC4900A78F5D96DEFD98364B082A3CF5E560622A6858993C` |

These are two different game versions derived from the same official lineage.
The older build contains the accumulated historical analysis; the current Null
runtime adds the distributor's public-key and server-URL changes and must be
analyzed in its own database. Absolute addresses and type-dependent views must
not be copied between them.

The following is the recovered current-runtime path. It must not be mixed with
addresses from the earlier official-library snapshot:

- `0x1026898` forwards the decoded frame's message ID to the mode-0 factory
  `0x1026E64`.
- The `0x5ACD` (`23245`) case allocates a `0xC0`-byte message object and calls
  constructor `0x10529C4`.
- Its vtable is `0x18A96B0`; decoder `0x1052A98`, encoder `0x1052A18`, message-ID
  getter `0x1052BFC`, and message-version getter `0x1052C04` are also confirmed
  by both vtable slots and runtime calls.
- The decoder allocates object A at message `+0x98` with size `0x570`, runs
  `0xF948A4` and `0xF96F48`, allocates object B at `+0xA0` with size `0x208`,
  runs `0xCDF238` and vtable slot `+0x20` decoder `0xCE0394`, then reads five VarInt32
  values into `+0xA8`, `+0xAC`, `+0xB0`, `+0xB4`, and `+0xB8`.
- Clientbound dispatch `0xAD3DA8` routes the real `0x5ACD` value to the
  `23245` branch at `0xAD5FCC`. The branch moves A and B out of the message,
  passes B's `+0x78` string to the object reached from `GameRoot+0x510`,
  passes the message's `+0xB0` value to `0xBDB09C`, sets `GameRoot+0x8 = 3`,
  and calls state installer `0xC8E054`.
- `0xC8E054` stores A at `GameRoot+0x40` and B at `+0x48`, stores constants
  `0` and `1` at `+0x50/+0x54`, stores message `+0xAC` at `+0x58`, a computed
  timestamp at `+0x60`, message `+0xA8` at `+0x70`, message `+0xB4` at
  `+0x68`, and message `+0xB8` at `+0x6C`.

A controlled established-encryption frame with message ID `23245`, version
`10`, and a 4096-byte all-zero body reached all of those runtime functions:
factory, constructor, decoder, clientbound dispatch, the `23245` branch,
object-A/object-B extraction, and `0xC8E054`. This separates "the message is
recognized and routed" from "the nested profile body is semantically valid."

A cursor trace of the all-zero representation consumed exactly 605 bytes:
object A consumed bytes `0..508`, object B consumed `508..600`, and the five
outer VarInt32 values consumed `600..605`. A second run with an exact 605-byte
body reached the same dispatcher and state installer. A later repeat of the same
exact representation did not crash: the client sent established messages
`14300` and `38000`, the local server ignored both as unsupported, and the
connection then ended. The original null-callback crash remains observed but
not reproducible, so the evidence does not show that the zero values alone
caused rejection. Extra trailing bytes are ruled out as the decoder length
error; semantic validity remains open.

The earlier addresses `0x105EA74`, `0xADBC1C`, and `0xC9805C`, and the reported
Frida observation at `0xADD134`, belong to a different library snapshot and
must not be used to explain the Null client. They remain useful only as
unverified historical evidence for that other artifact.

The IPA/current-SO correlation supplies 79 exact field bindings across 39
decode codecs in this message, representing 74 unique member names. They cover
cards, decks, card drafts, inventory items and spells, rewards, offer state,
masteries, tasks, season progress, badges, banners, trails, skins, consumables,
and event/deep-link state. The presence of this entire account-level state
inside object A is the main reason to classify `23245` as a startup/player
state package.

This is stronger than a field-name coincidence, but it is still not enough to
assign the business class name `OwnHomeDataMessage`:

- the message ID differs from the reference's `24180`;
- the top-level wire shape is two immediately structured objects followed by
  five varints, rather than timestamp, `ClientAvatar`, and three optional
  document blobs;
- no current consumer proves that object A or B has the reference's account,
  home, avatar-document, or home-document roles;
- several early object fields have stable widths and positions, but neither
  their semantic names nor an exact correspondence to the reference avatar
  fields has been proven.

The defensible description is: **a high-confidence initial profile/game-state
or start-configuration candidate whose architecture is analogous to a home
bootstrap**. The exact business name and mapping to `OwnHomeDataMessage`
remain hypotheses.

### Loading-chain correction

`23245` sets `GameRoot+0x8 = 3`. This is the high-level game-root object
returned by `0xC965C0`; it is not the resource-state word
`LoadingScreen+0xC` used by `LoadingScreen::updateLoadState`.

The message does affect objects read by the loading screen:

- `C9805C` installs object A and object B into `GameRoot+0x40/+0x48`;
- `LoadingScreen::updateLoadState` consults those objects while processing
  startup state;
- `LoadingScreen::updateProgressUi` obtains the active progress provider from
  `GameRoot+0x20`.

The isolated 2026-10-29 test partially supersedes this open boundary. With the
133,844-byte decrypted `23245` object body sent after the `30000`
post-consent transition, the client rendered the lobby and remained stable in
the tested session. The byte count must not be confused with the 133,867-byte
sealed frame captured on the wire; the transport seals the object body itself.

### Current candidate: message `27663`

Message `27663` is profile-shaped but not an initial own-home response.
`0xCA57E8` forwards its `+0xB0` payload through the tournament/profile UI
pipeline. Downstream `0xA94DC8` updates databind properties named
`tournament_info`, `tournament_host`, `host_name`, `host_clan`, and
`clan_badge`.

Its current field hints are:

```text
m_trailKeys
m_visibleBadges
m_seasonResults
m_trailProgress
m_trailGlobalIds
m_badgesShownInProfile
```

That combination fits a tournament/profile-results view. It should not be
promoted as the initial home payload without contrary evidence.

## Current `24222` versus reference `FishingDataMessage`

The reference release maps exact ID `24222` to `FishingDataMessage`, whose
payload is an `OwnHomeDataMessage`. This is the one exact profile/home-related
ID shared by both builds.

The current message also has a two-stage shape: a presence boolean followed by
an own-home-like nested payload. Its current payload codec is only `0x78`
bytes and exposes an optional u32 pair, a clan array, two owner-backed
collections, and an optional result container. The current handler
`0x8AC47C` stores that payload on a game object rather than running the
`23245` bootstrap path.

The exact ID and wrapper-like shape support the inherited name
`FishingDataMessage`. They do not establish that the current payload has the
reference's `OwnHomeDataMessage` wire layout: the current payload is much
smaller, has different component sizes, and has only one directly named field,
`m_clans`. Treat `24222` as a useful fishing/home-related anchor, not as the
normal initial-account bootstrap.

## IPA field grouping for message `23245`

The following groups are the 79 field-to-codec bindings. A repeated member can
appear in more than one nested decoder.

| Nested decoder | Fields |
| --- | --- |
| `0xEB07D4` | `m_cardDrafts`, `m_chainOfferProgresses`, `m_shards`, `m_badges`, `m_cards`, `m_skinSets`, `m_consumables`, `m_decks`, `m_chests`, `m_randomCards`, `m_tradeTokens`, `m_liveRewards`, `m_banners`, `m_mysteryBoxes`, `m_chestsWithCardTheme` |
| `0xD2C43C` | `m_usedGiveSpellIds`, `m_usedGetSpellIdsRare`, `m_usedGetSpellIdsCommon`, `m_itemsGive`, `m_usedGetSpellIdsLegendary`, `m_itemsGet`, `m_usedGetSpellIdsEpic` |
| `0xFB88B4` | `m_consumableCounts`, `m_seenConsumables`, `m_newUnseenConsumables`, `m_consumableIds`, `m_maxedConsumables` |
| `0xFFA314` | `m_rewardsPurchasedAmounts`, `m_eventDecks`, `m_eventDecksHaveBeenEdited`, `m_rewardsPurchasedIndexes`, `m_deckEventIds` |
| `0xF8CAA8` | `m_newBanners`, `m_chronosBannerOverrides`, `m_ownedBanners`, `m_favoriteBanners` |
| `0xFEFB04` | `m_newRulerSkins`, `m_ownedRulerSkins`, `m_newRulerVFXs`, `m_ownedRulerVFXs` |
| `0xF8775C` | `m_pendingNotSeenUpgradedBadges`, `m_badgesShownInProfile`, `m_badges` |
| `0x1014524` | `m_collectedSeasonalRewardsLeagueDataIndices`, `m_collectedOneTimeRewardsLeagueDataIndices` |
| `0x1031388` | `m_unclaimedRewards`, `m_activeStates` |
| `0x116D60C` | `m_values`, `m_keys` |
| `0xFFCD44` | `m_openedDeepLinkEventEndTimestamps`, `m_openedDeepLinkEventIds` |
| `0x1002164` | `m_cachedBadges` |
| `0x1007ABC` | `m_trails` |
| `0x100A524` | `m_claimedRewards` |
| `0x1010F1C` | `m_masteriesProgress` |
| `0x10116C4` | `m_tasksProgress` |
| `0x1018548` | `m_purchasedOffers` |
| `0x101AB1C` | `m_chainOfferStates` |
| `0x101C4B8` | `m_dependencyAccumulators` |
| `0x102F0BC` | `m_rewards` |
| `0x1031EF4` | `m_unseenLosses` |
| `0x1113A6C` | `m_augments` |
| `0xD2D434` | `m_items` |
| `0xD2E09C` | `m_spells` |
| `0xD2E9CC` | `m_deckUsage` |
| `0xEB4650` | `m_emoteIds` |
| `0xEE214C` | `m_resolvedOverrideRewards` |
| `0xEE2488` | `m_rewards` |
| `0xF8213C` | `m_progress` |
| `0xF8375C` | `m_collectedSnippets` |
| `0xF92CB4` | `m_rewards` |
| `0xF93D6C` | `m_rewardList` |
| `0xF9B390` | `m_progress` |
| `0xF9B928` | `m_rewardClaimed` |
| `0xF9FB54` | `m_claimedStepIndices` |
| `0xFCB6EC` | `m_popupOpenedEventIds` |
| `0xFD6B74` | `m_progress` |
| `0xFE2A24` | `m_previousSeasonPassUncollectedNodes` |
| `0xFF2A00` | `m_loadedOverrideNodes` |

Many of these are league or event systems, but all are reached from the same
`23245` decode closure. That makes the bundle account-wide, rather than a
single match or tournament result.

## Remaining candidate summary

The current binary also contains strong field-name hints for two large profile
related message classes that are absent from the Hay Day reference:

```text
23245  initial profile/game-state candidate
27663  tournament/profile-results candidate
```

The IPA-derived hints on `23245` include inventory, cards, decks, badges,
banners, rewards, season-pass state, trails, consumables, emote IDs, and
`m_badgesShownInProfile`. The hints on `27663` include
`m_badgesShownInProfile`, `m_seasonResults`, `m_visibleBadges`, and trail
state. The SO supplies wire boundaries and object offsets for these messages,
but neither class has a proven business name or complete type layout. The hints
are useful search seeds, not schema truth.

A normalized name check found no exact public-property match among the 74
unique `23245` member names and the reference `OwnHomeDataMessage` /
`ClientAvatar` properties. Terms such as `Badge`, `Profile`, and `Result`
overlap only as generic concepts. Consequently the reference contributes no
safe field-name backfill for `23245` or `27663`.

## Comparison with the current LoginOk

The reference can support a current login implementation in the following
limited ways:

| Item | Reference value | Use for current build |
| --- | --- | --- |
| Account identity | Account ID plus Home ID | Account part is proven; Home ID remains a hypothesis |
| Pass token | Stored with the account for later login | Confirmed by current persistence consumers |
| Content delivery | URL lists in LoginOk | Current normal and Chronos URL lists share the idea, not the old layout |
| Permissions | Generic permission set | Current wire shape is useful; entry meanings are not established |
| Support configuration | No matching current Zendesk model | Current `+0x188..+0x1A0` group is Clash-specific evidence |
| Profile/home | Rich Hay Day home and avatar codecs | Architectural guidance only |

The reference profile observer cannot populate a current Clash Royale profile
until the client's post-login home/profile response is identified and decoded.
It also cannot decide which values move the loading state machine forward. The
2026-10-29 local-server result proves the boundary can be crossed with a captured
valid profile, not that the reference model supplies every required value.

## Loading-state boundary

The verified current live path is:

```text
10100 -> 20100 -> encrypted 10101
-> clientbound 20963
-> established 17537 analytic event
-> established 10108 keep-alive
-> clientbound 23245 captured profile fixture
-> main lobby
```

`17537` is an analytics event and `10108` is an empty keep-alive. Neither is a
home/profile payload. The later decrypted `23245` fixture has rendered the
lobby, and the corrected replay session remained stable through the tested
interval. Dynamic synthesis of the same profile is still unresolved.

The current loading analysis separately establishes that the UI progress bar
and login initialization are independent. A complete resource bar can coexist
with `LOADING_SCREEN_PENDING_LOGIN`, and `InitFlowImpl` can still wait through
age, consent, SCID, marketing SDK, ATT, backfill, or server login states.

## Next work

1. Identify the producer and required consumer of serverbound `14300`. Its
   captured plaintext is `7f0112a8807f`, which decodes as signed VarInt32 `-1`,
   big-endian u32 `0x112A880`, and signed VarInt32 `-1`; the business meaning is
   still unknown.
2. Accept `38000` as analytics telemetry without a response and run the
   exact-605 profile again. Stop at the next distinct authenticated message or a
   specific handler requirement.
3. Recover the minimum required field shape of object A (`0xF96F48`) and object
   B (`0xCE0394`), then test bounded substitutions after the `14300`/`38000`
   boundary.
3. Use the reference codecs to generate hypotheses about common concepts such
   as account/home identity and compressed subdocuments, then verify every
   current field against SO evidence.
4. Keep message-ID matches, wire-shape matches, nested-codec similarity, and
   business-semantic matches as separate scores. A copied class name is not
   enough to promote any one of them to a current-build fact.
5. Only after the initial home/profile payload is accepted should the server
   attempt a minimum nonempty profile fixture for the loading screen.

The controlled evidence for this correction is stored locally at
`D:\IDA Scripts\clash-royale-supply-chain\work\null-libg-analysis\profile23245-dispatch-20261028`.
The main logs are `frida.jsonl` (SHA-256
`929F5CE5CB0EF0D9FF1473062992EC0F64312F97A7F8EAB67DE48803D6672927`)
and `server.stdout.log` (SHA-256
`1931A08615DC7895F6458F5D9C03D25780284F4A7BB02B4F1D4C02BCEF39BCD0`).
These are local controlled-test artifacts and are not part of the public
reference implementation.

## Material correction to the prior comparison

The Hay Day reference is useful at three separate confidence levels:

- exact ID overlap is demonstrated for `24222`;
- architectural overlap is plausible for a `FishingData` wrapper containing an
  own-home-like payload;
- wire and business-field equivalence are not demonstrated for the compact
  current `24222` payload.

Neither reference `OwnHomeDataMessage` nor the smaller current `24222` payload
explains the much larger current message `23245`. Conversely, `23245` now has
stronger current-SO bootstrap evidence than either `24222` or `27663`, but must
remain a current-build hypothesis until a runtime consumer or controlled
response proves its business role.
