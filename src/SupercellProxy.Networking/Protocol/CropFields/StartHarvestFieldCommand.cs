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
    /// Starts the harvest of one field; the gain follows 50 sub-ticks later and the field resets 10 sub-ticks after that.
    /// </summary>
    /// <param name="fieldGlobalId">Game object id of the ready field.</param>
    public StartHarvestFieldCommand(int fieldGlobalId)
    {
        FieldGlobalId = fieldGlobalId;
    }

    /// <inheritdoc />
    public static CommandCodec<StartHarvestFieldCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.FieldGlobalId, static fieldGlobalId => new StartHarvestFieldCommand(fieldGlobalId)),
        CommandLayout.MetadataFirst
    );

    /// <summary>
    /// Game object id of the field being harvested.
    /// </summary>
    [JsonPropertyName("FieldGlobalId")]
    public int FieldGlobalId { get; }
}
