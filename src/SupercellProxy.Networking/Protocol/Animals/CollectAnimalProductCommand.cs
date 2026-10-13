using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>
/// Defines the Collect Animal Product Command contract.
/// </summary>
/// <summary>
/// Defines the Animal Global Id contract.
/// </summary>
public sealed record CollectAnimalProductCommand([property: System.Text.Json.Serialization.JsonPropertyName("AnimalGlobalId")] int AnimalGlobalId) : CodecCommand<CollectAnimalProductCommand>, ICommandCodecProvider<CollectAnimalProductCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectAnimalProductCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.AnimalGlobalId,
            static (animalGlobalId) => new CollectAnimalProductCommand(animalGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
