using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Clientbound full neighborhood profiles for one directory response.</summary>
public sealed record NeighborhoodFullListMessage : CodecMessage<NeighborhoodFullListMessage>, ICodecProvider<NeighborhoodFullListMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodFullListMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.NullableArray(NeighborhoodHeaderEntry.Codec, CollectionCount.AtMost(maximum: 1024).WithStrictAbsence()),
            static message => message.Profiles,
            static profiles => new NeighborhoodFullListMessage
            {
                Profiles = profiles,
            }),
        trailingDataMessage: "The full neighborhood list has trailing data.");

    /// <summary>Gets or sets the optional ordered list of full profiles.</summary>
    /// <value>Up to 1024 neighbourhood profiles in the order the server sent them, or <see langword="null"/> when the server wrote the list as absent.</value>
    public NeighborhoodHeaderEntry[]? Profiles { get; init; }

    /// <summary>Omits neighborhood names and ids from diagnostic text.</summary>
    /// <returns>Always the type name <c>NeighborhoodFullListMessage</c>, so logs never show the profiles.</returns>
    public override string ToString()
    {
        return nameof(NeighborhoodFullListMessage);
    }
}
