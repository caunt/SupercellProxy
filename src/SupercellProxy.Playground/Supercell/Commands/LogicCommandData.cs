using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Diagnostic command state included outside production environments.
/// </summary>
public sealed record LogicCommandData
{
    public const int ValueCount = 128;

    public LogicCommandData(ReadOnlyMemory<int> values, string text, int unknown0)
    {
        if (values.Length != ValueCount)
            throw new ArgumentException($"Logic command data must contain exactly {ValueCount} values.", nameof(values));

        Values = values;
        Text = text;
        Unknown0 = unknown0;
    }

    public ReadOnlyMemory<int> Values { get; }
    public string Text { get; }
    public int Unknown0 { get; }

    internal static LogicCommandData Decode(SupercellStream stream)
    {
        var values = new int[ValueCount];

        for (var i = 0; i < values.Length; i++)
            values[i] = stream.ReadVarInt();

        return new LogicCommandData(values, stream.ReadString(), stream.ReadVarInt());
    }

    internal void Encode(SupercellStream stream)
    {
        foreach (var value in Values.Span)
            stream.WriteVarInt(value);

        stream.WriteString(Text);
        stream.WriteVarInt(Unknown0);
    }
}
