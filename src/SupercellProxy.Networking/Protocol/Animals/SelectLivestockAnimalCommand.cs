using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

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
) : CodecCommand<SelectLivestockAnimalCommand>, ICommandCodecProvider<SelectLivestockAnimalCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SelectLivestockAnimalCommand> Codec { get; } = Codecs.Command(
        type: 21,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.HabitatGlobalId,
            Codecs.VarInt,
            static command => command.AnimalGlobalId,
            static (habitat, animal) => new SelectLivestockAnimalCommand(habitat, animal)
        )
    );
}
