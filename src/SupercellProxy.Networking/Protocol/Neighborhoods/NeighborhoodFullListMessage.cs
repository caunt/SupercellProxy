using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Clientbound full neighborhood profiles for one directory response.</summary>
public sealed record NeighborhoodFullListMessage : CodecMessage<NeighborhoodFullListMessage>, ICodecProvider<NeighborhoodFullListMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodFullListMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.NullableArray(NeighborhoodHeaderEntry.Codec, CollectionCount.AtMost(maximum: 1024).WithStrictAbsence()),
            static message => message.Profiles,
            static profiles => new NeighborhoodFullListMessage
            {
                Profiles = profiles,
            }
        ),
        trailingDataMessage: "The full neighborhood list has trailing data."
    );

    /// <summary>Gets the optional ordered list of full profiles.</summary>
    public NeighborhoodHeaderEntry[]? Profiles { get; init; }

    /// <summary>Omits neighborhood names and ids from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(NeighborhoodFullListMessage);
    }
}
