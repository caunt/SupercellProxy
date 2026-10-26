# Clash Royale `libg` analytics semantics

This note records static-analysis observations for the Clash Royale `libg`
client against the protocol message registered here as `AnalyticsBatchMessage`
(message id `38000`). It describes the analyzed client build only. The
`Release17286` and `Release17381` tables in this repository remain the
authority for message and field layouts.

## Wire-level semantics

| Field | Observed meaning |
| --- | --- |
| `AnalyticsBatchMessage.Time0` | Unix time in seconds when the session began. |
| `AnalyticsBatchMessage.Time1` | Total session duration in milliseconds, including paused intervals. |
| `AnalyticsBatchMessage.Time2` | Active session duration in milliseconds, excluding paused intervals. Equals `Time1` when the session never paused. |
| `AnalyticsContextValues.Value0` | Integer battery level captured at session start. |
| `AnalyticsContextValues.Value1` | Integer battery level captured at session end. |
| `AnalyticsContextValues.Flag` | Charging state captured for the session. |
| `AnalyticsContextTimestamp.Time` | Active session duration in milliseconds, excluding paused intervals. |
| `AnalyticsEvent.Time0` | Segment start relative to the session's absolute start, in milliseconds. |
| `AnalyticsEvent.Time1` | Total segment wall-clock duration in milliseconds. |
| `AnalyticsEvent.Time2` | Active segment duration in milliseconds, excluding paused intervals. |

The session property list contains `session_name` and then the named properties
associated with that session. The analyzed normal producer initializes each
event's property list empty; event measurements are carried by the event
context.

## Context sections

The encoded context order is:

```text
Values
FirstStatistics
SecondStatistics
Counters
Timestamp
ThirdStatistics
```

This differs from collector-kind order:

```text
1 Values
2 FirstStatistics
3 Timestamp
4 SecondStatistics
5 Counters
6 ThirdStatistics
```

The analyzed normal producer requests collector kinds `2, 4, 5, 1`; the initial
launch session can additionally request kind `3`. Kind `6` has wire support and
a registered collector implementation, but no active producer was observed in
the analyzed build.

## Statistics payload

All three statistics sections use the same ten-slot 64-bit payload order:

```text
0  duringSessionMin
1  duringSessionMax
2  atSessionStart
3  atSessionEnd
4  p1
5  p5
6  p50
7  p95
8  p99
9  sampleCount
```

At least one statistics stream receives frame intervals in milliseconds. The
current binary does not establish which serialized collector receives that
stream, nor the complete business metric taxonomy of `FirstStatistics` and
`SecondStatistics`. Older asset strings mentioning memory or network metrics
are historical clues and do not establish the current mapping.

## Thermal counters

`AnalyticsContextCounters` uses this order:

```text
Value0  statusAtSessionStart
Value1  statusAtSessionEnd
Value2  minStatusDuringSession
Value3  maxStatusDuringSession
```

The encoded status values are:

```text
0  notSet
1  nominal
2  warning
3  serious
4  critical
```

## Timestamp subtype

`AnalyticsContextTimestamp.Value` is a release-specific timestamp collector
subtype. It is not the context section's collector kind. The analyzed initial
launch path uses `1`; an older capture used `2` for a Farm_load operation. The
complete subtype mapping is not established and should not be generalized
across game releases.

## Remaining questions

- Which concrete metrics feed collector kinds `2` and `4`?
- Which statistics collector receives the frame-interval stream?
- Does another component add collector kind `6` in a runtime path not covered
  by static analysis?
- Why does the analyzed message reset omit `Time2`?

These observations are semantic annotations. They do not change the wire
layout or the public CLR names used by the protocol contracts.
