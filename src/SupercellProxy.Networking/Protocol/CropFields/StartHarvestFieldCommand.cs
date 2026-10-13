using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.CropFields;

/// <summary>
/// <para>Starts harvesting a ready crop field.</para>
/// </summary>
public sealed record StartHarvestFieldCommand : CodecCommand<StartHarvestFieldCommand>, ICommandCodecProvider<StartHarvestFieldCommand>
{
    /// <summary>
    /// Initializes a new <see cref="StartHarvestFieldCommand"/> instance.
    /// </summary>
    public StartHarvestFieldCommand(int fieldGlobalId)
    {
        FieldGlobalId = fieldGlobalId;
    }

    /// <inheritdoc />
    public static CommandCodec<StartHarvestFieldCommand> Codec { get; } = Codecs.Command(
        type: 544,
        Codecs.Composite(Codecs.VarInt, static command => command.FieldGlobalId, static (fieldGlobalId) => new StartHarvestFieldCommand(fieldGlobalId)),
        CommandLayout.MetadataFirst
    );

    /// <summary>
    /// Gets the <c language="csharp">FieldGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("FieldGlobalId")]
    public int FieldGlobalId { get; }
}
