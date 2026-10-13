using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>Buys a livestock animal into a habitat.</summary>
/// <param name="Mirrored">Whether the animal faces the mirrored direction.</param>
/// <param name="PositionY">The vertical position.</param>
/// <param name="HabitatGlobalId">The habitat game object id.</param>
/// <param name="AnimalDataGlobalId">The animal data id.</param>
/// <param name="PositionX">The horizontal position.</param>
public sealed record PurchaseLivestockAnimalCommand(
    bool Mirrored,
    int PositionY,
    [property: System.Text.Json.Serialization.JsonPropertyName("HabitatGlobalId")] int HabitatGlobalId,
    [property: System.Text.Json.Serialization.JsonPropertyName("AnimalDataGlobalId")] int AnimalDataGlobalId,
    int PositionX
) : CodecCommand<PurchaseLivestockAnimalCommand>, ICommandCodecProvider<PurchaseLivestockAnimalCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PurchaseLivestockAnimalCommand> Codec { get; } = Codecs.Command(
        type: 641,
        Codecs.Versioned(
            Release17381.Version,
            Codecs.Composite(
                Codecs.VarInt,
                static command => command.HabitatGlobalId,
                Codecs.VarInt,
                static command => command.AnimalDataGlobalId,
                Codecs.VarInt,
                static command => command.PositionX,
                Codecs.Boolean,
                static command => command.Mirrored,
                Codecs.VarInt,
                static command => command.PositionY,
                static (habitatGlobalId, animalDataGlobalId, positionX, mirrored, positionY) => new PurchaseLivestockAnimalCommand(mirrored, positionY, habitatGlobalId, animalDataGlobalId, positionX)
            ),
            Codecs.Composite(
                Codecs.Boolean,
                static command => command.Mirrored,
                Codecs.VarInt,
                static command => command.PositionY,
                Codecs.VarInt,
                static command => command.HabitatGlobalId,
                Codecs.VarInt,
                static command => command.AnimalDataGlobalId,
                Codecs.VarInt,
                static command => command.PositionX,
                static (mirrored, positionY, habitatGlobalId, animalDataGlobalId, positionX) => new PurchaseLivestockAnimalCommand(mirrored, positionY, habitatGlobalId, animalDataGlobalId, positionX)
            )
        ),
        CommandLayout.MetadataFirst
    );
}
