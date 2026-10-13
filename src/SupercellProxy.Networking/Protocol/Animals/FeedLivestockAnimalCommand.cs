using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>
/// Defines the Feed Livestock Animal Command contract.
/// </summary>
/// <summary>
/// Defines the Animal Global Id contract.
/// </summary>
public sealed record FeedLivestockAnimalCommand([property: System.Text.Json.Serialization.JsonPropertyName("AnimalGlobalId")] int AnimalGlobalId) : CodecCommand<FeedLivestockAnimalCommand>, ICommandCodecProvider<FeedLivestockAnimalCommand>
{
    /// <inheritdoc />
    public static CommandCodec<FeedLivestockAnimalCommand> Codec { get; } = Codecs.Command(
        type: 532,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.AnimalGlobalId,
            static (animalGlobalId) => new FeedLivestockAnimalCommand(animalGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
