using System.Collections.Immutable;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>
/// Clientbound newspaper edition: the roadside shop advertisements and the stories shown on its pages.
/// </summary>
public sealed record NewspaperDataMessage : CodecMessage<NewspaperDataMessage>, ICodecProvider<NewspaperDataMessage>
{
    /// <inheritdoc />
    public static ICodec<NewspaperDataMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.NullableArray(NewspaperStoryEntry.Codec, CollectionCount.PerElement(minimumElementSize: 24)),
            static message => message.Stories,
            Codecs.NullableArray(NewspaperPlacement.Codec, CollectionCount.PerElement(minimumElementSize: 14)),
            static message => message.Placements,
            static (stories, placements) => new NewspaperDataMessage
            {
                Stories = stories,
                Placements = placements,
            }),
        trailingDataMessage: "The newspaper data has trailing bytes.");

    /// <summary>
    /// Gets or sets the farms advertising their roadside shops in this edition, each with up to four advertised items; <see langword="null"/> when absent.
    /// </summary>
    /// <value>The second list of the message, in the order the server sent it; its count is checked against the remaining payload before reading.</value>
    public ImmutableArray<NewspaperPlacement>? Placements { get; init; }

    /// <summary>
    /// Gets or sets the stories printed in this edition; <see langword="null"/> when absent.
    /// </summary>
    /// <value>The first list of the message, in the order the server sent it; its count is checked against the remaining payload before reading.</value>
    public ImmutableArray<NewspaperStoryEntry>? Stories { get; init; }
}
