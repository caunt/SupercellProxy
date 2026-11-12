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
    /// Initializes a new instance of the <see cref="HarvestFieldCommand"/> record. Ends a field's harvest.
    /// </summary>
    /// <param name="fieldGlobalId">Game object id of the harvested field.</param>
    public HarvestFieldCommand(int fieldGlobalId)
    {
        FieldGlobalId = fieldGlobalId;
    }

    /// <inheritdoc />
    public static CommandCodec<HarvestFieldCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.FieldGlobalId, static fieldGlobalId => new HarvestFieldCommand(fieldGlobalId)),
        CommandLayout.MetadataFirst);

    /// <summary>
    /// Gets the game object id of the field whose harvest ends.
    /// </summary>
    /// <value>The field's game object id, written as a VarInt; it is the same id the earlier start and gain commands of this harvest carried.</value>
    [JsonPropertyName("FieldGlobalId")]
    public int FieldGlobalId { get; }
}
