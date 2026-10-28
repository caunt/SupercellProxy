# Clash Royale profile support in the SupercellProxy reference

Date: 2026-10-28

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
| Loading-screen completion | Not proven; `23245` is not yet tied to loading completion at runtime |

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

The current binary contains a large stateful clientbound message `23245` that
is absent from the Hay Day reference. It is now a strong candidate for the
initial player/game-state bootstrap:

- `0x105EA74` decodes two object fields first:
  - object A at message `+0x98` has size `0x570`; its decoder is
    `0xFA2878` (`LibgMessage_23245::NestedCodec_126::decode`);
  - object B at message `+0xA0` has size `0x208` and uses a polymorphic
    decoder through its vtable.
- Five scalar fields follow at `+0xA8`, `+0xAC`, `+0xB0`, `+0xB4`, and
  `+0xB8`.
- The dispatcher `0xADBC1C` moves both objects out of the message. It changes
  the high-level `GameRoot` returned by `0xC965C0`, installs both objects and
  the scalar values through `0xC9805C`, copies object B's `+0x78` string into
  the object reached through `GameRoot+0x510`, and records launch-timing
  metrics through `0xBE2E18` and `0xBE2E40`.
- `0xC9805C` stores object A at `GameRoot+0x40`, object B at `+0x48`, constants
  `0` and `1` at `+0x50/+0x54`, message field `+0xA8` at `+0x58`, a current
  timestamp at `+0x60`, `+0xAC` at `+0x68`, `+0xB4` at `+0x6C`, and `+0xB8`
  at `+0x70`.

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

However, no current evidence shows that receiving `23245` alone completes the
LoadingScreen state machine. The existing live trace still proves only
`LoginOk 20963 -> 17537 -> 10108`; it does not prove that the client entered
the game or completed `InitFlow` state 12.

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
It also cannot decide which values move the loading state machine forward.

## Loading-state boundary

The verified current live path is:

```text
10100 -> 20100 -> encrypted 10101
-> clientbound 20963
-> established 17537 analytic event
-> established 10108 keep-alive
```

`17537` is an analytics event and `10108` is an empty keep-alive. Neither is a
home/profile payload. Therefore this trace proves protocol and crypto
continuity after LoginOk, but it does not prove that the client loaded a
profile or passed the loading screen.

The current loading analysis separately establishes that the UI progress bar
and login initialization are independent. A complete resource bar can coexist
with `LOADING_SCREEN_PENDING_LOGIN`, and `InitFlowImpl` can still wait through
age, consent, SCID, marketing SDK, ATT, backfill, or server login states.

## Next work

1. Identify the first authenticated client request after `10108`, then bind it
   to the current `InitFlowImpl` server-login state and loading-screen waiter.
2. Prove or reject the hypothesis that `23245` carries the initial
   own-profile/home state through a controlled response or runtime breakpoint
   at `0xADBC1C`/`0xC9805C`.
3. Use the reference codecs to generate hypotheses about common concepts such
   as account/home identity and compressed subdocuments, then verify every
   current field against SO evidence.
4. Keep message-ID matches, wire-shape matches, nested-codec similarity, and
   business-semantic matches as separate scores. A copied class name is not
   enough to promote any one of them to a current-build fact.
5. Only after the initial home/profile payload is accepted should the server
   attempt a minimum nonempty profile fixture for the loading screen.

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
