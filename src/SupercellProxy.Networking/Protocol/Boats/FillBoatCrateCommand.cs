using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Defines the Fill Boat Crate Command contract.
/// </summary>
/// <summary>
/// Defines the Client Presentation contract.
/// </summary>
/// <summary>
/// Defines the Crate Index contract.
/// </summary>
/// <summary>
/// Defines the Confirmation contract.
/// </summary>
public sealed record FillBoatCrateCommand(
    bool ClientPresentation,
    int CrateIndex,
    bool Confirmation,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.FillBoatCrateCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static FillBoatCrateCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new FillBoatCrateCommand(
            stream.ReadBoolean(),
            stream.ReadVariableInt(),
            stream.ReadBoolean(),
            fields.ExecutionPhaseCounter,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteBoolean(ClientPresentation);
        stream.WriteVariableInt(CrateIndex);
        stream.WriteBoolean(Confirmation);
    }
}
