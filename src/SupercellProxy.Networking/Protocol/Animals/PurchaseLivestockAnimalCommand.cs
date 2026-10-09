using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>Places one purchased livestock animal in an existing habitat.</summary>
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

    internal static Version ReorderedFieldsVersion { get; } = new(major: 1, minor: 73, build: 81);

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static PurchaseLivestockAnimalCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (stream.GameVersion >= ReorderedFieldsVersion)
        {
            int habitatGlobalId = stream.ReadVarInt();
            int animalDataGlobalId = stream.ReadVarInt();
            int positionX = stream.ReadVarInt();
            bool mirrored = stream.ReadBoolean();

            return new PurchaseLivestockAnimalCommand(mirrored, stream.ReadVarInt(), habitatGlobalId, animalDataGlobalId, positionX);
        }

        return new PurchaseLivestockAnimalCommand(stream.ReadBoolean(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        if (stream.GameVersion >= ReorderedFieldsVersion)
        {
            stream.WriteVarInt(HabitatGlobalId);
            stream.WriteVarInt(AnimalDataGlobalId);
            stream.WriteVarInt(PositionX);
            stream.WriteBoolean(Mirrored);
            stream.WriteVarInt(PositionY);

            return;
        }

        stream.WriteBoolean(Mirrored);
        stream.WriteVarInt(PositionY);
        stream.WriteVarInt(HabitatGlobalId);
        stream.WriteVarInt(AnimalDataGlobalId);
        stream.WriteVarInt(PositionX);
    }
}
