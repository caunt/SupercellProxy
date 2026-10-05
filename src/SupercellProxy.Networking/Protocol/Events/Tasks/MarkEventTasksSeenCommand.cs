using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// Defines the Mark Event Tasks Seen Command contract.
/// </summary>
/// <summary>
/// Defines the Task Index contract.
/// </summary>
/// <summary>
/// Defines the Event Id contract.
/// </summary>
public sealed record MarkEventTasksSeenCommand(int TaskIndex, [property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventId) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.MarkEventTasksSeenCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MarkEventTasksSeenCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MarkEventTasksSeenCommand(stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(TaskIndex);
        stream.WriteVarInt(EventId);
    }
}
