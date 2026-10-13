using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>
/// <para>Diagnostic command state included outside production environments.</para>
/// </summary>
public sealed record CommandData : ICodecProvider<CommandData>
{
    /// <summary>The number of diagnostic values native sends.</summary>
    public const int ValueCount = 128;

    /// <summary>Initializes a new <see cref="CommandData"/> instance.</summary>
    public CommandData(ReadOnlyMemory<int> values, string text, int unknown0)
    {
        if (values.Length != ValueCount)
            throw new ArgumentException($"Logic command data must contain exactly {ValueCount} values.", nameof(values));

        Values = values;
        Text = text;
        Unknown0 = unknown0;
    }

    /// <inheritdoc />
    public static ICodec<CommandData> Codec { get; } = Codecs.Composite(
        Codecs.FixedArray(Codecs.VarInt, ValueCount),
        static data => data.Values.ToArray(),
        Codecs.String,
        static data => data.Text,
        Codecs.VarInt,
        static data => data.Unknown0,
        static (values, text, unknown0) => new CommandData(values, text, unknown0)
    );

    /// <summary>Gets the diagnostic text.</summary>
    public string Text { get; }

    /// <summary>Gets the <c language="csharp">Unknown0</c> value.</summary>
    public int Unknown0 { get; }

    /// <summary>Gets the diagnostic values.</summary>
    public ReadOnlyMemory<int> Values { get; }
}
