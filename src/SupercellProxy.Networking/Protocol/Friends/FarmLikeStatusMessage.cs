using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Reports whether a particular visitor has liked a farm.</summary>
public sealed record FarmLikeStatusMessage(
    [property: System.Text.Json.Serialization.JsonPropertyName("HomeOwnerId")] LongId HomeOwnerId,
    [property: System.Text.Json.Serialization.JsonPropertyName("VisitorId")] LongId VisitorId,
    bool HasLiked
)
    : IMessage
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static FarmLikeStatusMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        FarmLikeStatusMessage result = new(stream.ReadLongId(), stream.ReadLongId(), stream.ReadBoolean());

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The farm-like status has trailing data.")
            : result;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteLongId(HomeOwnerId);
        stream.WriteLongId(VisitorId);
        stream.WriteBoolean(HasLiked);
    }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(FarmLikeStatusMessage);
    }
}
