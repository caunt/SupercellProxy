using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Joining;

/// <summary>Delivers one full neighborhood entry to the neighborhood manager.</summary>
/// <param name="Entry">The neighbourhood with its profile and members.</param>
public sealed record NeighborhoodFullEntryMessage(NeighborhoodFullEntry Entry) : CodecMessage<NeighborhoodFullEntryMessage>, ICodecProvider<NeighborhoodFullEntryMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodFullEntryMessage> Codec { get; } = Codecs.Composite(
        SupercellProxy.Networking.Protocol.Neighborhoods.Joining.NeighborhoodFullEntry.Codec,
        static message => message.Entry,
        static entry => new NeighborhoodFullEntryMessage(entry)
    );
}
