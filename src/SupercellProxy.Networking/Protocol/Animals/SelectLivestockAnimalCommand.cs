using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>
/// Serverbound command recording which animal in a habitat the player tapped: that animal becomes selected and every other animal
/// in the same habitat is deselected. Selecting an animal that is already selected, or one from another habitat, changes nothing.
/// </summary>
/// <param name="HabitatGlobalId">Game object id of the habitat (coop, pen and so on).</param>
/// <param name="AnimalGlobalId">Game object id of the tapped animal; an id matching no animal deselects the whole habitat.</param>
public sealed record SelectLivestockAnimalCommand(
    [property: JsonPropertyName("HabitatGlobalId")] int HabitatGlobalId,
    [property: JsonPropertyName("AnimalGlobalId")] int AnimalGlobalId
) : CodecCommand<SelectLivestockAnimalCommand>, ICommandCodecProvider<SelectLivestockAnimalCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SelectLivestockAnimalCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.HabitatGlobalId,
            Codecs.VarInt,
            static command => command.AnimalGlobalId,
            static (habitat, animal) => new SelectLivestockAnimalCommand(habitat, animal)
        )
    );
}
