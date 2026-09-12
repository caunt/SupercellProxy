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
public sealed record VisitedBoatHelpRequestServerCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("HomeOwnerId")] LongIdentifier HomeOwnerIdentifier,
    int CrateIndex,
    int RequestValue,
    int ServerCommandIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : ServerCommand(ServerCommandIdentifier, ExecutionPhaseCounter, DebugData0, DebugData1)
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
        LongIdentifier owner = stream.ReadLongIdentifier();
        int crate = stream.ReadVariableInt();
        int requestValue = stream.ReadVariableInt();
        (int serverCommandIdentifier, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields) = DecodeServerCommand(stream, environment);

        return new VisitedBoatHelpRequestServerCommand(
            owner,
            crate,
            requestValue,
            serverCommandIdentifier,
            commandFields.ExecutionPhaseCounter,
            commandFields.DebugData0,
            commandFields.DebugData1
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongIdentifier(HomeOwnerIdentifier);
        stream.WriteVariableInt(CrateIndex);
        stream.WriteVariableInt(RequestValue);
        EncodeServerCommand(stream, environment);
    }
}
