using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">CommandDataReferenceArrayField</c>.
/// </summary>
internal sealed record CommandDataReferenceArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandDataReferenceArrayField"/> instance.
    /// </summary>
    public CommandDataReferenceArrayField(ReadOnlyMemory<int> globalIds) =>
        GlobalIds = globalIds.ToArray();

    /// <summary>
    /// Gets the <c language="csharp">GlobalIds</c> value.
    /// </summary>
    public ReadOnlyMemory<int> GlobalIds { get; }
    internal override CommandFieldType FieldType => CommandFieldType.DataReferenceArray;

    internal static CommandDataReferenceArrayField Decode(MessageStream stream) =>
        new(CommandVarIntArrayField.DecodeValues(stream.ReadVarInt(), stream));

    internal override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(GlobalIds.Length);

        foreach (var globalId in GlobalIds.Span)
            stream.WriteVarInt(globalId);
    }
}
