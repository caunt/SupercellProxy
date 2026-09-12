using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>
/// Defines the Record Event Seen Command contract.
/// </summary>
/// <summary>
/// Defines the Event Id contract.
/// </summary>
/// <summary>
/// Defines the Context0 contract.
/// </summary>
/// <summary>
/// Defines the Context1 contract.
/// </summary>
public sealed record RecordEventSeenCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventIdentifier,
    int Context0,
    int Context1,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.RecordEventSeenCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RecordEventSeenCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int identifier = stream.ReadInt32();
        int context0 = stream.ReadInt32();
        int context1 = stream.ReadInt32();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new RecordEventSeenCommand(identifier, context0, context1, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteInt32(EventIdentifier);
        stream.WriteInt32(Context0);
        stream.WriteInt32(Context1);
        EncodeCommand(stream, environment);
    }
}
