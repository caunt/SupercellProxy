using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Defines the Clientbound26668 Message contract.
/// </summary>
/// <summary>
/// Defines the Home Owner Id contract.
/// </summary>
/// <summary>
/// Defines the Visitor Id contract.
/// </summary>
/// <summary>
/// Defines the Flag contract.
/// </summary>
public sealed record HomeVisitStatusMessage(
    [property: System.Text.Json.Serialization.JsonPropertyName("HomeOwnerId")] LongId HomeOwnerId,
    [property: System.Text.Json.Serialization.JsonPropertyName("VisitorId")] LongId VisitorId,
    bool Flag
)
    : IMessage
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static HomeVisitStatusMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        HomeVisitStatusMessage result = new(stream.ReadLongId(), stream.ReadLongId(), stream.ReadBoolean());

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The visit-pair status has trailing data.")
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
        stream.WriteBoolean(Flag);
    }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(HomeVisitStatusMessage);
    }
}
