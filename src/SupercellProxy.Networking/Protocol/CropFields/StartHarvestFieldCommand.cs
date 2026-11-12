using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.CropFields;

/// <summary>
/// <para>Starts harvesting a ready crop field.</para>
/// </summary>
public sealed record StartHarvestFieldCommand : CodecCommand<StartHarvestFieldCommand>, ICommandCodecProvider<StartHarvestFieldCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StartHarvestFieldCommand"/> record. Starts the harvest of one field.
    /// </summary>
    /// <param name="fieldGlobalId">Game object id of the ready field.</param>
    public StartHarvestFieldCommand(int fieldGlobalId)
    {
        FieldGlobalId = fieldGlobalId;
    }

    /// <inheritdoc />
    public static CommandCodec<StartHarvestFieldCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.FieldGlobalId, static fieldGlobalId => new StartHarvestFieldCommand(fieldGlobalId)),
        CommandLayout.MetadataFirst);

    /// <summary>
    /// Gets the game object id of the field being harvested.
    /// </summary>
    /// <value>The field's game object id, written as a VarInt; the follow-up gain and harvest commands carry the same id.</value>
    [JsonPropertyName("FieldGlobalId")]
    public int FieldGlobalId { get; }
}
