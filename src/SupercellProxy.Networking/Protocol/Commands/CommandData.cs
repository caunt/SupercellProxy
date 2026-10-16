using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>
/// <para>Diagnostic command state included outside production environments.</para>
/// </summary>
public sealed record CommandData : ICodecProvider<CommandData>
{
    /// <summary>The number of diagnostic values native sends.</summary>
    public const int ValueCount = 128;

    /// <summary>Builds the diagnostic block from its parts, checking it has the fixed number of values native always writes.</summary>
    /// <param name="values">Exactly <see cref="ValueCount"/> diagnostic numbers.</param>
    /// <param name="text">Diagnostic text written after the numbers.</param>
    /// <param name="unknown0">The trailing integer, passed through unchanged.</param>
    /// <exception cref="ArgumentException"><paramref name="values"/> does not hold exactly <see cref="ValueCount"/> numbers.</exception>
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

    /// <summary>Integer native writes after the diagnostic text; its meaning is not identified, so it is only carried through.</summary>
    public int Unknown0 { get; }

    /// <summary>Gets the diagnostic values.</summary>
    public ReadOnlyMemory<int> Values { get; }
}
