using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.CropFields;

/// <summary>
/// <para>Applies the crop and experience rewards for a started field harvest.</para>
/// </summary>
public sealed record HarvestFieldGainCommand : CodecCommand<HarvestFieldGainCommand>, ICommandCodecProvider<HarvestFieldGainCommand>
{
    /// <summary>
    /// Collects the crop of a field whose harvest started 50 sub-ticks earlier.
    /// </summary>
    /// <param name="fieldGlobalId">Game object id of the harvested field.</param>
    public HarvestFieldGainCommand(int fieldGlobalId)
    {
        FieldGlobalId = fieldGlobalId;
    }

    /// <inheritdoc />
    public static CommandCodec<HarvestFieldGainCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.FieldGlobalId, static fieldGlobalId => new HarvestFieldGainCommand(fieldGlobalId)),
        CommandLayout.MetadataFirst
    );

    /// <summary>
    /// Game object id of the field whose crop is collected.
    /// </summary>
    [JsonPropertyName("FieldGlobalId")]
    public int FieldGlobalId { get; }
}
