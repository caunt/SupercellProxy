using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;

/// <summary>The fields native <c>StreamEntry::decode</c> reads before every neighborhood stream entry's own fields.</summary>
/// <param name="PostedTimestamp">The time the entry was posted, in seconds.</param>
/// <param name="AgeSeconds">The entry's age in seconds.</param>
/// <param name="New">Whether the entry is new.</param>
/// <param name="Removed">Whether the entry was removed; NeighborhoodStreamEntryRemovedMessage sets it on a received entry.</param>
/// <param name="EntryId">The entry id.</param>
/// <param name="SenderHomeId">The sender's home.</param>
/// <param name="SenderFarmName">The sender's farm name.</param>
/// <param name="SenderLevel">The sender's level.</param>
/// <param name="SenderRole">The sender's neighborhood role.</param>
/// <param name="Value">The value native reads after the sender's role.</param>
/// <param name="Reactions">The entry's reactions, when present.</param>
public sealed record StreamEntryHeader(
    int PostedTimestamp,
    int AgeSeconds,
    bool New,
    bool Removed,
    LongId EntryId,
    LongId SenderHomeId,
    string? SenderFarmName,
    int SenderLevel,
    int SenderRole,
    int Value,
    ReadOnlyMemory<StreamEntryReaction>? Reactions
) : ICodecProvider<StreamEntryHeader>
{
    /// <inheritdoc />
    public static ICodec<StreamEntryHeader> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static header => header.PostedTimestamp,
        Codecs.VarInt,
        static header => header.AgeSeconds,
        Codecs.Boolean,
        static header => header.New,
        Codecs.Boolean,
        static header => header.Removed,
        Codecs.LongId,
        static header => header.EntryId,
        Codecs.LongId,
        static header => header.SenderHomeId,
        Codecs.OptionalString,
        static header => header.SenderFarmName,
        Codecs.VarInt,
        static header => header.SenderLevel,
        Codecs.VarInt,
        static header => header.SenderRole,
        Codecs.VarInt,
        static header => header.Value,
        Codecs.OptionalValue(Codecs.Memory(StreamEntryReaction.Codec)),
        static header => header.Reactions,
        static (postedTimestamp, ageSeconds, isNew, removed, entryId, senderHomeId, senderFarmName, senderLevel, senderRole, value, reactions) => new StreamEntryHeader(postedTimestamp, ageSeconds, isNew, removed, entryId, senderHomeId, senderFarmName, senderLevel, senderRole, value, reactions)
    );
}
