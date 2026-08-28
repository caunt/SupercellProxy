using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Diagnostic command state included outside production environments.</para>
/// </summary>
internal sealed record CommandData
{
    /// <summary>
    /// Defines the <c language="csharp">ValueCount</c> value.
    /// </summary>
    public const int ValueCount = 128;

    /// <summary>
    /// Initializes a new <see cref="CommandData"/> instance.
    /// </summary>
    public CommandData(ReadOnlyMemory<int> values, string text, int unknown0)
    {
        if (values.Length != ValueCount)
            throw new ArgumentException(
                $"Logic command data must contain exactly {ValueCount} values.",
                nameof(values)
            );

        Values = values;
        Text = text;
        Unknown0 = unknown0;
    }

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<int> Values { get; }

    /// <summary>
    /// Gets the <c language="csharp">Text</c> value.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    internal static CommandData Decode(MessageStream stream)
    {
        var values = new int[ValueCount];

        for (var i = 0; i < values.Length; i++)
            values[i] = stream.ReadVarInt();

        return new CommandData(values, stream.ReadString(), stream.ReadVarInt());
    }

    internal void Encode(MessageStream stream)
    {
        foreach (var value in Values.Span)
            stream.WriteVarInt(value);
        stream.WriteString(Text);
        stream.WriteVarInt(Unknown0);
    }
}
