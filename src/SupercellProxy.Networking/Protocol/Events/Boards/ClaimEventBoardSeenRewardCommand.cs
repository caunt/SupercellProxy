using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Events.Boards;

/// <summary>
/// Defines the Claim Event Board Seen Reward Command contract.
/// </summary>
/// <summary>
/// Defines the Event Id contract.
/// </summary>
public sealed record ClaimEventBoardSeenRewardCommand([property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventId) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.ClaimEventBoardSeenRewardCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static ClaimEventBoardSeenRewardCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new ClaimEventBoardSeenRewardCommand(stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(EventId);
    }
}
