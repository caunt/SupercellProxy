using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>
/// <para>Diagnostic command state included outside production environments.</para>
/// </summary>
public sealed record CommandData : ICodecProvider<CommandData>
{
    /// <summary>The number of diagnostic values native sends.</summary>
    public const int ValueCount = 128;

    /// <summary>Initializes a new instance of the <see cref="CommandData"/> record. Builds the diagnostic block from its parts, checking it has the fixed number of values native always writes.</summary>
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
    public static ICodec<CommandData> Codec { get; } = Codecs.Composite(Codecs.FixedArray(Codecs.VarInt, ValueCount),
        static data => data.Values.ToArray(),
        Codecs.String,
        static data => data.Text,
        Codecs.VarInt,
        static data => data.Unknown0,
        static (values, text, unknown0) => new CommandData(values, text, unknown0));

    /// <summary>Gets the diagnostic text.</summary>
    /// <value>The free-form text native writes after the <see cref="ValueCount"/> numbers; it can be empty.</value>
    public string Text { get; }

    /// <summary>Gets the integer native writes after the diagnostic text; its meaning is not identified, so it is only carried through.</summary>
    /// <value>The trailing VarInt of the block, written back exactly as it was read.</value>
    public int Unknown0 { get; }

    /// <summary>Gets the diagnostic values.</summary>
    /// <value>Exactly <see cref="ValueCount"/> VarInt numbers in wire order; the constructor rejects any other count.</value>
    public ReadOnlyMemory<int> Values { get; }
}
