using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Defines the Visited Boat Help Request Server Command contract.
/// </summary>
/// <summary>
/// Defines the Home Owner Id contract.
/// </summary>
/// <summary>
/// Defines the Crate Index contract.
/// </summary>
/// <summary>
/// Defines the Request Value contract.
/// </summary>
public sealed record VisitedBoatHelpRequestServerCommand([property: System.Text.Json.Serialization.JsonPropertyName("HomeOwnerId")] LongId HomeOwnerId, int CrateIndex, int RequestKind) : ServerCommand
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.VisitedBoatHelpRequestServerCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static VisitedBoatHelpRequestServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongId owner = stream.ReadLongId();
        int crate = stream.ReadVarInt();
        int requestKind = stream.ReadVarInt();


        return new VisitedBoatHelpRequestServerCommand(owner, crate, requestKind);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongId(HomeOwnerId);
        stream.WriteVarInt(CrateIndex);
        stream.WriteVarInt(RequestKind);
    }
}
