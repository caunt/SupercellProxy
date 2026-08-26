using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>CommandByteArrayField</c>.
/// </summary>
public sealed record CommandByteArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandByteArrayField"/> instance.
    /// </summary>
    public CommandByteArrayField(ReadOnlyMemory<byte> value)
    {
        Value = value.ToArray();
    }

    /// <summary>
    /// Gets the <c>Value</c> value.
    /// </summary>
    public ReadOnlyMemory<byte> Value { get; }
    internal override CommandFieldType FieldType => CommandFieldType.ByteArray;

    internal override void Encode(MessageStream stream) => stream.WriteByteArray(Value.Span);
}
