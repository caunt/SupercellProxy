using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.CropFields;

/// <summary>
/// <para>Completes harvesting a crop from a field.</para>
/// </summary>
public sealed record HarvestFieldCommand : CodecCommand<HarvestFieldCommand>, ICommandCodecProvider<HarvestFieldCommand>
{
    /// <summary>
    /// Initializes a new <see cref="HarvestFieldCommand"/> instance.
    /// </summary>
    public HarvestFieldCommand(int fieldGlobalId)
    {
        FieldGlobalId = fieldGlobalId;
    }

    /// <inheritdoc />
    public static CommandCodec<HarvestFieldCommand> Codec { get; } = Codecs.Command(
        type: 506,
        Codecs.Composite(Codecs.VarInt, static command => command.FieldGlobalId, static (fieldGlobalId) => new HarvestFieldCommand(fieldGlobalId)),
        CommandLayout.MetadataFirst
    );

    /// <summary>
    /// Gets the <c language="csharp">FieldGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("FieldGlobalId")]
    public int FieldGlobalId { get; }
}
