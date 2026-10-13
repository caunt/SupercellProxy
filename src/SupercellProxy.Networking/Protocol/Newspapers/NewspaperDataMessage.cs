using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>
/// Defines the Newspaper Data Message contract.
/// </summary>
public sealed record NewspaperDataMessage : CodecMessage<NewspaperDataMessage>, ICodecProvider<NewspaperDataMessage>
{
    /// <inheritdoc />
    public static ICodec<NewspaperDataMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.NullableArray(NewspaperStoryEntry.Codec, CollectionCount.PerElement(minimumElementSize: 24)),
            static message => message.Stories,
            Codecs.NullableArray(NewspaperPlacement.Codec, CollectionCount.PerElement(minimumElementSize: 14)),
            static message => message.Placements,
            static (stories, placements) => new NewspaperDataMessage
            {
                Stories = stories,
                Placements = placements,
            }
        ),
        trailingDataMessage: "The newspaper data has trailing bytes."
    );

    /// <summary>
    /// Gets the Placements value.
    /// </summary>
    public NewspaperPlacement[]? Placements { get; init; }
    /// <summary>
    /// Gets the Stories value.
    /// </summary>
    public NewspaperStoryEntry[]? Stories { get; init; }
}
