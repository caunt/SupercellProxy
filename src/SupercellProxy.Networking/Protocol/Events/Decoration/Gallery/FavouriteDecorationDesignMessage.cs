using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Gallery;

/// <summary>Adds a decoration design to the photo album favourites.</summary>
/// <param name="EventId">The event id.</param>
/// <param name="DesignId">The design id.</param>
/// <param name="VariantId">The variant id.</param>
public sealed record FavouriteDecorationDesignMessage(int EventId, DesignAvatarId? DesignId, int VariantId) : CodecMessage<FavouriteDecorationDesignMessage>, ICodecProvider<FavouriteDecorationDesignMessage>
{
    /// <inheritdoc />
    public static ICodec<FavouriteDecorationDesignMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.EventId,
        Codecs.Optional(DesignAvatarId.Codec),
        static message => message.DesignId,
        Codecs.VarInt,
        static message => message.VariantId,
        static (eventId, designId, variantId) => new FavouriteDecorationDesignMessage(eventId, designId, variantId)
    );
}
