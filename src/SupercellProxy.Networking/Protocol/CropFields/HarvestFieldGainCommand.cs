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
    /// Initializes a new instance of the <see cref="HarvestFieldGainCommand"/> record. Collects the crop of a field whose harvest was started.
    /// </summary>
    /// <param name="fieldGlobalId">Game object id of the harvested field.</param>
    public HarvestFieldGainCommand(int fieldGlobalId)
    {
        FieldGlobalId = fieldGlobalId;
    }

    /// <inheritdoc />
    public static CommandCodec<HarvestFieldGainCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.FieldGlobalId, static fieldGlobalId => new HarvestFieldGainCommand(fieldGlobalId)),
        CommandLayout.MetadataFirst);

    /// <summary>
    /// Gets the game object id of the field whose crop is collected.
    /// </summary>
    /// <value>The field's game object id, written as a VarInt; it is the same id the <see cref="StartHarvestFieldCommand"/> of this harvest carried.</value>
    [JsonPropertyName("FieldGlobalId")]
    public int FieldGlobalId { get; }
}
