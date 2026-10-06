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
    [property: System.Text.Json.Serialization.JsonPropertyName("RequestId")] string RequestId,
    int OrderIndex,
    int TicketIndex,
    [property: System.Text.Json.Serialization.JsonPropertyName("ObjectGlobalId")] int ObjectGlobalId,
    [property: System.Text.Json.Serialization.JsonPropertyName("DataGlobalId")] int DataGlobalId,
    int SlotIndex
) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.ActivateMovieTicketCommandType;

    internal static Version ReorderedFieldsVersion { get; } = new(major: 1, minor: 73, build: 81);

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static ActivateMovieTicketCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        string requestId = stream.ReadString();

        if (stream.GameVersion >= ReorderedFieldsVersion)
        {
            int dataGlobalId = stream.ReadVarInt();
            int orderIndex = stream.ReadVarInt();
            int slotIndex = stream.ReadVarInt();
            int objectGlobalId = stream.ReadVarInt();

            return new ActivateMovieTicketCommand(requestId, orderIndex, stream.ReadVarInt(), objectGlobalId, dataGlobalId, slotIndex);
        }

        return new ActivateMovieTicketCommand(requestId, stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteString(RequestId);

        if (stream.GameVersion >= ReorderedFieldsVersion)
        {
            stream.WriteVarInt(DataGlobalId);
            stream.WriteVarInt(OrderIndex);
            stream.WriteVarInt(SlotIndex);
            stream.WriteVarInt(ObjectGlobalId);
            stream.WriteVarInt(TicketIndex);

            return;
        }

        stream.WriteVarInt(OrderIndex);
        stream.WriteVarInt(TicketIndex);
        stream.WriteVarInt(ObjectGlobalId);
        stream.WriteVarInt(DataGlobalId);
        stream.WriteVarInt(SlotIndex);
    }
}
