using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>
/// Defines the Collect Gift Command contract.
/// </summary>
/// <summary>
/// Defines the Gift Global Id contract.
/// </summary>
public sealed record CollectGiftCommand([property: System.Text.Json.Serialization.JsonPropertyName("GiftGlobalId")] int GiftGlobalId) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.CollectGiftCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CollectGiftCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int gift = stream.ReadVarInt();

        return new CollectGiftCommand(gift);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(GiftGlobalId);
    }
}
