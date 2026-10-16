using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.CropFields;

/// <summary>
/// <para>Completes harvesting a crop from a field.</para>
/// </summary>
public sealed record HarvestFieldCommand : CodecCommand<HarvestFieldCommand>, ICommandCodecProvider<HarvestFieldCommand>
{
    /// <summary>
    /// Ends a field's harvest, 10 sub-ticks after its gain, leaving the field empty and ready to plant.
    /// </summary>
    /// <param name="fieldGlobalId">Game object id of the harvested field.</param>
    public HarvestFieldCommand(int fieldGlobalId)
    {
        FieldGlobalId = fieldGlobalId;
    }

    /// <inheritdoc />
    public static CommandCodec<HarvestFieldCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.FieldGlobalId, static fieldGlobalId => new HarvestFieldCommand(fieldGlobalId)),
        CommandLayout.MetadataFirst
    );

    /// <summary>
    /// Game object id of the field that returns to empty.
    /// </summary>
    [JsonPropertyName("FieldGlobalId")]
    public int FieldGlobalId { get; }
}
