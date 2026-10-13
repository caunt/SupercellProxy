using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Help;

/// <summary>Delivers the help entries of a farm; the client passes them to the neighborhood chat when it is visiting that farm.</summary>
/// <param name="Value">The value.</param>
/// <param name="OwnerId">The owner id.</param>
/// <param name="Entries">The entries.</param>
public sealed record NeighborhoodHelpEntriesMessage(int Value, LongId OwnerId, ReadOnlyMemory<NeighborhoodHelpEntry>? Entries) : CodecMessage<NeighborhoodHelpEntriesMessage>, ICodecProvider<NeighborhoodHelpEntriesMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodHelpEntriesMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.Value,
        Codecs.LongId,
        static message => message.OwnerId,
        Codecs.NullableMemory(NeighborhoodHelpEntry.Codec),
        static message => message.Entries,
        static (value, ownerId, entries) => new NeighborhoodHelpEntriesMessage(value, ownerId, entries)
    );
}
