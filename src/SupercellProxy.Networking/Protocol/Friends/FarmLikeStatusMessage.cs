using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Reports whether a particular visitor has liked a farm.</summary>
public sealed record FarmLikeStatusMessage(
    [property: JsonPropertyName("HomeOwnerId")] LongId HomeOwnerId,
    [property: JsonPropertyName("VisitorId")] LongId VisitorId,
    bool HasLiked
) : CodecMessage<FarmLikeStatusMessage>, ICodecProvider<FarmLikeStatusMessage>
{
    /// <inheritdoc />
    public static ICodec<FarmLikeStatusMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.LongId,
            static message => message.HomeOwnerId,
            Codecs.LongId,
            static message => message.VisitorId,
            Codecs.Boolean,
            static message => message.HasLiked,
            static (homeOwnerId, visitorId, hasLiked) => new FarmLikeStatusMessage(homeOwnerId, visitorId, hasLiked)
        ),
        trailingDataMessage: "The farm-like status has trailing data."
    );

    /// <summary>
    /// Returns only the type name, keeping player ids out of logs.
    /// </summary>
    public override string ToString()
    {
        return nameof(FarmLikeStatusMessage);
    }
}
