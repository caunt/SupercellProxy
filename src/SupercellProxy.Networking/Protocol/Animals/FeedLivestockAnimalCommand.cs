using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>
/// Serverbound command that feeds one livestock animal from the barn's feed, starting its next product.
/// </summary>
/// <param name="AnimalGlobalId">Game object id of the animal on the farm, not its animal type.</param>
public sealed record FeedLivestockAnimalCommand([property: JsonPropertyName("AnimalGlobalId")] int AnimalGlobalId) : CodecCommand<FeedLivestockAnimalCommand>, ICommandCodecProvider<FeedLivestockAnimalCommand>
{
    /// <inheritdoc />
    public static CommandCodec<FeedLivestockAnimalCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.AnimalGlobalId, static animalGlobalId => new FeedLivestockAnimalCommand(animalGlobalId)),
        CommandLayout.MetadataFirst
    );
}
