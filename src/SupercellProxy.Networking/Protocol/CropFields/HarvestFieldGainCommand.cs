using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.CropFields;

/// <summary>
/// <para>Applies the crop and experience rewards for a started field harvest.</para>
/// </summary>
public sealed record HarvestFieldGainCommand : CodecCommand<HarvestFieldGainCommand>, ICommandCodecProvider<HarvestFieldGainCommand>
{
    /// <summary>
    /// Initializes a new <see cref="HarvestFieldGainCommand"/> instance.
    /// </summary>
    public HarvestFieldGainCommand(int fieldGlobalId)
    {
        FieldGlobalId = fieldGlobalId;
    }

    /// <inheritdoc />
    public static CommandCodec<HarvestFieldGainCommand> Codec { get; } = Codecs.Command(
        type: 657,
        Codecs.Composite(Codecs.VarInt, static command => command.FieldGlobalId, static (fieldGlobalId) => new HarvestFieldGainCommand(fieldGlobalId)),
        CommandLayout.MetadataFirst
    );

    /// <summary>
    /// Gets the <c language="csharp">FieldGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("FieldGlobalId")]
    public int FieldGlobalId { get; }
}
