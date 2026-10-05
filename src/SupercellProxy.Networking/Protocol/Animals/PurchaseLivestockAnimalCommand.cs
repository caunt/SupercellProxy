using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Animals;

/// Command 641 places one purchased livestock animal in an existing habitat.
public sealed record PurchaseLivestockAnimalCommand(
    bool Mirrored,
    int PositionY,
    [property: System.Text.Json.Serialization.JsonPropertyName("HabitatGlobalId")] int HabitatGlobalId,
    [property: System.Text.Json.Serialization.JsonPropertyName("AnimalDataGlobalId")] int AnimalDataGlobalId,
    int PositionX
) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.PurchaseLivestockAnimalCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static PurchaseLivestockAnimalCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new PurchaseLivestockAnimalCommand(stream.ReadBoolean(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteBoolean(Mirrored);
        stream.WriteVarInt(PositionY);
        stream.WriteVarInt(HabitatGlobalId);
        stream.WriteVarInt(AnimalDataGlobalId);
        stream.WriteVarInt(PositionX);
    }
}
