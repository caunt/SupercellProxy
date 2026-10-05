using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>
/// Defines the Select Livestock Animal Command contract.
/// </summary>
/// <summary>
/// Defines the Habitat Global Id contract.
/// </summary>
/// <summary>
/// Defines the Animal Global Id contract.
/// </summary>
public sealed record SelectLivestockAnimalCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("HabitatGlobalId")] int HabitatGlobalId,
    [property: System.Text.Json.Serialization.JsonPropertyName("AnimalGlobalId")] int AnimalGlobalId
) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.SelectLivestockAnimalCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static SelectLivestockAnimalCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int habitat = stream.ReadVarInt();
        int animal = stream.ReadVarInt();

        return new SelectLivestockAnimalCommand(habitat, animal);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(HabitatGlobalId);
        stream.WriteVarInt(AnimalGlobalId);
    }
}
