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
| Loading-screen completion | Not proven by the current LoginOk/keep-alive trace |

This note uses the current `libg` target whose SHA-256 is
`110AA2B5CAC391C498645E072B0D88729428C2C2845E7E2737CA8EE979059783`.
The reference snapshot is commit `388a6e5` on the fork's Clash Royale research
branch.

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

The current schema does contain message `24222`, but its assigned name is
`fishing_data`, copied from the reference table. One nested call is
structurally named `own_home_data_encoded_object`. That nested name records
reuse of a similar codec shape; it does not prove that the current message is
an own-home response. The contradictory labels are a good example of why exact
ID and structural similarity must remain separate confidence dimensions.

The current binary also contains strong field-name hints for two large profile
related message classes that are absent from the Hay Day reference:

```text
23245  own-state-like message
27663  profile/results-like message
```

The IPA-derived hints on `23245` include inventory, cards, decks, badges,
banners, rewards, season-pass state, trails, consumables, emote IDs, and
`m_badgesShownInProfile`. The hints on `27663` include
`m_badgesShownInProfile`, `m_seasonResults`, `m_visibleBadges`, and trail
state. The SO supplies wire boundaries and object offsets for these messages,
but neither class has a proven business name or complete type layout. The hints
are useful search seeds, not schema truth.

A normalized name check over the reference's avatar, home, and social models
found no exact public-property match for these profile-specific hints. Terms
such as `Badge`, `Profile`, and `Result` overlap only as generic concepts.
Consequently the reference contributes no safe field-name backfill for
`23245` or `27663`.

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
2. Determine which current message class carries the initial own-profile/home
   state. Treat `23245` as a candidate and prove it through dispatch, native
   consumers, and a controlled response.
3. Use the reference codecs to generate hypotheses about common concepts such
   as account/home identity and compressed subdocuments, then verify every
   current field against SO evidence.
4. Keep message-ID matches, wire-shape matches, nested-codec similarity, and
   business-semantic matches as separate scores. A copied class name is not
   enough to promote any one of them to a current-build fact.
5. Only after the initial home/profile payload is accepted should the server
   attempt a minimum nonempty profile fixture for the loading screen.

## Material correction to the prior comparison

The Hay Day reference is more useful for invariants and implementation patterns
than for Clash Royale field recovery. In particular, exact-message-ID overlap
must not override the current binary's dispatch and consumer evidence. The
existing `24222` assignment demonstrates the risk: a reference name
`fishing_data` and a shape-derived nested name `own_home_data_encoded_object`
currently coexist without a current-build semantic resolution.
