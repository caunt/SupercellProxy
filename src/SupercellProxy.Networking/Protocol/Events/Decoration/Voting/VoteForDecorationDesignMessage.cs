using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Voting;

/// <summary>Submits a vote for a decoration design.</summary>
/// <param name="EventId">The event id.</param>
/// <param name="VariantId">The variant id.</param>
/// <param name="Selection">The selection.</param>
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
