using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>
/// Defines the Feed Livestock Animal Command contract.
/// </summary>
/// <summary>
/// Defines the Animal Global Id contract.
/// </summary>
public sealed record FeedLivestockAnimalCommand([property: System.Text.Json.Serialization.JsonPropertyName("AnimalGlobalId")] int AnimalGlobalId) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.FeedLivestockAnimalCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static FeedLivestockAnimalCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new FeedLivestockAnimalCommand(stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(AnimalGlobalId);
    }
}
