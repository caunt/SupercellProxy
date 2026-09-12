using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "Wire contracts expose the exact mutable arrays required for decoding, editing, and re-encoding payloads."
)]
[assembly: SuppressMessage(
    "Design",
    "CA1056:URI properties should not be strings",
    Justification = "Protocol URL fields must preserve arbitrary received strings byte-for-byte."
)]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "Named wire collection envelopes are protocol contracts, not general-purpose collection implementations."
)]
[assembly: SuppressMessage(
    "Naming",
    "CA1720:Identifier contains type name",
    Justification = "Wire field discriminators are named after their encoded primitive types."
)]
[assembly: SuppressMessage(
    "Design",
    "CA1008:Enums should have zero value",
    Justification = "Wire enums retain their established values; an unassigned zero is not a fabricated success value."
)]
[assembly: SuppressMessage(
    "Design",
    "CA1040:Avoid empty interfaces",
    Justification = "IEvent is the intentional constraint identifying typed protocol events.",
    Scope = "type",
    Target = "~T:SupercellProxy.Networking.Events.IEvent"
)]
