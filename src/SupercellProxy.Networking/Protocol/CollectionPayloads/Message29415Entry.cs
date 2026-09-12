namespace SupercellProxy.Networking.Protocol.CollectionPayloads;

/// Carries one wire entry from clientbound message 29415.
public sealed record Message29415Entry(
    LongIdentifier ResourceAssociation,
    string? PrimaryText,
    string? OptionalIndexedText,
    string? UnretainedText,
    string? RequiredText,
    int Value,
    int UnretainedValue,
    int EntryType,
    int UnretainedTrailingValue
);
