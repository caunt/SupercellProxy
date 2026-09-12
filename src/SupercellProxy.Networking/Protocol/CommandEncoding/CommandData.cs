using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding;

/// <summary>
/// <para>Diagnostic command state included outside production environments.</para>
/// </summary>
public sealed record CommandData
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
            throw new ArgumentException($"Logic command data must contain exactly {ValueCount} values.", nameof(values));

        Values = values;
        Text = text;
        Unknown0 = unknown0;
    }

    /// <summary>
    /// Gets the <c language="csharp">Text</c> value.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<int> Values { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandData Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int[] values = new int[ValueCount];

        for (int index = 0; index < values.Length; index++)
            values[index] = stream.ReadVariableInt();

        return new CommandData(values, stream.ReadString(), stream.ReadVariableInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        foreach (int value in Values.Span)
            stream.WriteVariableInt(value);

        stream.WriteString(Text);
        stream.WriteVariableInt(Unknown0);
    }
}
