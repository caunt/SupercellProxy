using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>
/// Serverbound command that takes the finished product (eggs, milk, wool and so on) from a livestock animal, as tapping a ready
/// animal does. The animal then waits to be fed again.
/// </summary>
/// <param name="AnimalGlobalId">Game object id of the animal on the farm, not its animal type.</param>
public sealed record CollectAnimalProductCommand([property: JsonPropertyName("AnimalGlobalId")] int AnimalGlobalId) : CodecCommand<CollectAnimalProductCommand>, ICommandCodecProvider<CollectAnimalProductCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectAnimalProductCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.AnimalGlobalId,
            static animalGlobalId => new CollectAnimalProductCommand(animalGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
