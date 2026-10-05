using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Defines the Visited Boat Departure Server Command contract.
/// </summary>
/// <summary>
/// Defines the Home Owner Id contract.
/// </summary>
public sealed record VisitedBoatDepartureServerCommand([property: System.Text.Json.Serialization.JsonPropertyName("HomeOwnerId")] LongId HomeOwnerId) : ServerCommand
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.VisitedBoatDepartureServerCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static VisitedBoatDepartureServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongId owner = stream.ReadLongId();


        return new VisitedBoatDepartureServerCommand(owner);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongId(HomeOwnerId);
    }
}
