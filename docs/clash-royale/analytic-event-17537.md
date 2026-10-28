# Current Clash Royale `libg`: message 17537

This note records a version-specific observation from the analyzed current
client build:

```text
libg.so SHA-256
110AA2B5CAC391C498645E072B0D88729428C2C2845E7E2737CA8EE979059783
```

The numeric IDs in this note belong to that build. They must not be copied to
another release without checking its message factory.

## Identification

Message `17537` (`0x4481`) is the current client's `AnalyticEventMessage`
equivalent. It is not `RequestOwnHomeMessage`; the two messages use different
classes, schemas, and numeric IDs.

The identification is based on these independent observations:

- The message factory branch at `0x1028964` compares against `0x4481`,
  allocates a `0x98`-byte object, and calls the constructor at `0x103EC30`.
- The object installs vtable `0x18A6940`. Its encode, decode, and ID slots are
  `0x103ECB0`, `0x103ED08`, and `0x103ED90`; the ID getter returns `17537`.
- The decoder allocates an inner object with constructor `0xCD74EC`. That
  constructor installs vtable `0x1874388` and parses a JSON string.
- The inner encoder and decoder at `0xCD76B4` and `0xCD7728` write or read a
  required event-type string followed by a nullable JSON string.
- A controlled login capture produced a 110-byte application payload with
  event type `billing` and a parsed JSON object.
- After clientbound LoginOk `20963` is accepted in the same controlled session,
  the client sends `17537` before any other application message. This is a
  scheduling observation only; it does not make `17537` a home request. The
  full current-build LoginOk layout is recorded in
  [`loginok-20963.md`](loginok-20963.md).

A longer controlled capture kept the connection open after `17537` and then
received repeated 16-byte `10108` frames on the same connection. A later
current-client run with a complete anonymous LoginOk `20963` again sent
`10108`, and a strict empty-plaintext decoder accepted it. This verifies the
current transport behavior and payload shape; the current binary's class-level
`10108` identity has not yet been separately reconstructed. The sequence shows
that `17537` is a nonblocking analytics event interleaved with connection
traffic; it is not the sole post-login request that a server must answer before
the session can advance.

## Wire layout

The recovered logical schema is:

```text
AnalyticEventMessage
  HasEvent : Boolean bit
  if HasEvent:
    Type : String
    Json : nullable String
```

In the analyzed encoding:

- `HasEvent` uses the message stream's packed primary bitstream.
- `Type` is a 32-bit big-endian byte length followed by UTF-8 bytes.
- `Json` is a nullable 32-bit big-endian byte length followed by UTF-8 bytes;
  `0xFFFFFFFF` represents absent data.

The current decoder limits `Type` to 1024 bytes and `Json` to 900000 bytes.

The recovered 110-byte plaintext from the controlled billing event was:

```text
01
00000007 62696c6c696e67
0000005e 7b226576656e7450726f64756374223a227061796d656e7473756e617661696c61626c65222c2270726f647563744964223a22222c2250434368616e6765223a302c2273746f7265436f756e747279223a22222c22636f756e74223a307d
```

Decoded:

```text
HasEvent = true
Type     = "billing"
Json     = {"eventProduct":"paymentsunavailable","productId":"","PCChange":0,"storeCountry":"","count":0}
```

The JSON member names are preserved from the document. Their observed values
support these meanings:

| JSON member | Observed meaning |
| --- | --- |
| `eventProduct` | Billing event subtype. The captured client used `paymentsunavailable`. |
| `productId` | Store product identifier. Empty in the captured event. |
| `storeCountry` | Store country associated with the event. Empty in the captured event. |
| `PCChange` | Numeric field observed as `0`; its business meaning is not established. |
| `count` | Numeric field observed as `0`; its business meaning is not established. |

`Type` selects the broader analytics event family, while `Json` carries the
family-specific document. This is the same role split as the repository's
`AnalyticEventMessage(Type, Json)` contract. The current build differs from the
`Release17286` contract in two important ways:

- `Release17286` registers `AnalyticEventMessage` as message `10906`; the
  analyzed client uses `17537`.
- `Release17286` models both strings as independently optional. The analyzed
  client first writes an optional-event bit; when the event is present, `Type`
  is required and only `Json` is nullable.

The shared class contract therefore helps locate and name the message, but the
current binary remains authoritative for its wire layout.

## Evidence

The retained runtime artifact is
`probe-17537-analytic-event-plaintext.json` in the analysis workspace. It
contains the encode trace and 110-byte stream buffer. No key material or nonce
values are included in this note.
