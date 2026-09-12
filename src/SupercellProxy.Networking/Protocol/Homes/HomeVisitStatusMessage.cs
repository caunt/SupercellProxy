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
    [property: System.Text.Json.Serialization.JsonPropertyName("HomeOwnerId")] LongIdentifier HomeOwnerIdentifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("VisitorId")] LongIdentifier VisitorIdentifier,
    bool Flag
)
    : IMessage
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static HomeVisitStatusMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        HomeVisitStatusMessage result = new(container.Payload.ReadLongIdentifier(), container.Payload.ReadLongIdentifier(), container.Payload.ReadBoolean());

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "The visit-pair status has trailing data.")
            : result;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteLongIdentifier(HomeOwnerIdentifier);
        stream.WriteLongIdentifier(VisitorIdentifier);
        stream.WriteBoolean(Flag);

        return new MessageContainer(identifier, version, stream);
    }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(HomeVisitStatusMessage);
    }
}
