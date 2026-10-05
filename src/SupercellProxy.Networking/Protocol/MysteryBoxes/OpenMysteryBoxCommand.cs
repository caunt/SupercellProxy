using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MysteryBoxes;

/// <summary>
/// Defines the Open Mystery Box Command contract.
/// </summary>
/// <summary>
/// Defines the Box Global Id contract.
/// </summary>
/// <summary>
/// Defines the Legacy Flag contract.
/// </summary>
public sealed record OpenMysteryBoxCommand([property: System.Text.Json.Serialization.JsonPropertyName("BoxGlobalId")] int BoxGlobalId, bool LegacyFlag) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.OpenMysteryBoxCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static OpenMysteryBoxCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int box = stream.ReadVarInt();
        bool flag = stream.ReadBoolean();

        return new OpenMysteryBoxCommand(box, flag);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(BoxGlobalId);
        stream.WriteBoolean(LegacyFlag);
    }
}
