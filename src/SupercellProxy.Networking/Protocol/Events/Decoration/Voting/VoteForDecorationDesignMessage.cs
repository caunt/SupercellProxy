using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Voting;

/// <summary>Submits a vote for a decoration design.</summary>
/// <param name="EventId">Decoration event being voted in.</param>
/// <param name="VariantId">Variant of that event the player is in.</param>
/// <param name="Selection">Index of the design the player voted for among those offered.</param>
public sealed record VoteForDecorationDesignMessage(int EventId, int VariantId, int Selection) : CodecMessage<VoteForDecorationDesignMessage>, ICodecProvider<VoteForDecorationDesignMessage>
{
    /// <inheritdoc />
    public static ICodec<VoteForDecorationDesignMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.EventId,
        Codecs.VarInt,
        static message => message.VariantId,
        Codecs.VarInt,
        static message => message.Selection,
        static (eventId, variantId, selection) => new VoteForDecorationDesignMessage(eventId, variantId, selection)
    );
}
