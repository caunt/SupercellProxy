using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>
/// Defines the Activate Movie Ticket Command contract.
/// </summary>
/// <summary>
/// Defines the Request Id contract.
/// </summary>
/// <summary>
/// Defines the Order Index contract.
/// </summary>
/// <summary>
/// Defines the Ticket Index contract.
/// </summary>
/// <summary>
/// Defines the Object Global Id contract.
/// </summary>
/// <summary>
/// Defines the Data Global Id contract.
/// </summary>
/// <summary>
/// Defines the Slot Index contract.
/// </summary>
public sealed record ActivateMovieTicketCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("RequestId")] string RequestIdentifier,
    int OrderIndex,
    int TicketIndex,
    [property: System.Text.Json.Serialization.JsonPropertyName("ObjectGlobalId")] int ObjectGlobalIdentifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("DataGlobalId")] int DataGlobalIdentifier,
    int SlotIndex,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.ActivateMovieTicketCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static ActivateMovieTicketCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new ActivateMovieTicketCommand(
            stream.ReadString(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
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
        stream.WriteString(RequestIdentifier);
        stream.WriteVariableInt(OrderIndex);
        stream.WriteVariableInt(TicketIndex);
        stream.WriteVariableInt(ObjectGlobalIdentifier);
        stream.WriteVariableInt(DataGlobalIdentifier);
        stream.WriteVariableInt(SlotIndex);
    }
}
