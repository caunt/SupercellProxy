using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.CropFields;

/// <summary>Plants a crop on a field.</summary>
/// <param name="CropGlobalId">The crop data id.</param>
/// <param name="BuySeeds">Whether missing seeds are bought.</param>
/// <param name="FieldGlobalId">The field game object id.</param>
public sealed record PlantFieldCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("CropGlobalId")] int CropGlobalId,
    bool BuySeeds,
    [property: System.Text.Json.Serialization.JsonPropertyName("FieldGlobalId")] int FieldGlobalId
) : CodecCommand<PlantFieldCommand>, ICommandCodecProvider<PlantFieldCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PlantFieldCommand> Codec { get; } = Codecs.Command(
        type: 514,
        Codecs.Versioned(
            Release17381.Version,
            Codecs.Composite(
                Codecs.Boolean,
                static command => command.BuySeeds,
                Codecs.VarInt,
                static command => command.FieldGlobalId,
                Codecs.VarInt,
                static command => command.CropGlobalId,
                static (buySeeds, fieldGlobalId, cropGlobalId) => new PlantFieldCommand(cropGlobalId, buySeeds, fieldGlobalId)
            ),
            Codecs.Composite(
                Codecs.VarInt,
                static command => command.CropGlobalId,
                Codecs.Boolean,
                static command => command.BuySeeds,
                Codecs.VarInt,
                static command => command.FieldGlobalId,
                static (cropGlobalId, buySeeds, fieldGlobalId) => new PlantFieldCommand(cropGlobalId, buySeeds, fieldGlobalId)
            )
        ),
        CommandLayout.MetadataFirst
    );
}
