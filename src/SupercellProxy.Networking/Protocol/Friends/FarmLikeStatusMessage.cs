using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Reports whether a particular visitor has liked a farm.</summary>
/// <param name="HomeOwnerId">Home id of the farm whose like status is reported.</param>
/// <param name="VisitorId">Home id of the visiting player the status is about.</param>
/// <param name="HasLiked"><see langword="true"/> if that visitor has already liked the farm; otherwise, <see langword="false"/>.</param>
public sealed record FarmLikeStatusMessage([property: JsonPropertyName("HomeOwnerId")] LongId HomeOwnerId,
    [property: JsonPropertyName("VisitorId")] LongId VisitorId,
    bool HasLiked) : CodecMessage<FarmLikeStatusMessage>, ICodecProvider<FarmLikeStatusMessage>
{
    /// <inheritdoc />
    public static ICodec<FarmLikeStatusMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.LongId,
            static message => message.HomeOwnerId,
            Codecs.LongId,
            static message => message.VisitorId,
            Codecs.Boolean,
            static message => message.HasLiked,
            static (homeOwnerId, visitorId, hasLiked) => new FarmLikeStatusMessage(homeOwnerId, visitorId, hasLiked)),
        trailingDataMessage: "The farm-like status has trailing data.");

    /// <summary>
    /// Returns only the type name, keeping player ids out of logs.
    /// </summary>
    /// <returns>Always <c>FarmLikeStatusMessage</c>, without either home id or the like flag.</returns>
    public override string ToString()
    {
        return nameof(FarmLikeStatusMessage);
    }
}
