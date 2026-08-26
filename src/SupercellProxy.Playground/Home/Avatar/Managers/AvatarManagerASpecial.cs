using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c>AvatarManagerASpecial</c>.
/// </summary>
public record AvatarManagerASpecial
{
    /// <summary>
    /// Gets or sets the <c>UsesCompressedData</c> value.
    /// </summary>
    public bool UsesCompressedData { get; init; }

    /// <summary>
    /// Gets or sets the <c>Text</c> value.
    /// </summary>
    public string? Text { get; init; }

    /// <summary>
    /// Gets or sets the <c>CompressedData</c> value.
    /// </summary>
    public Memory<byte>? CompressedData { get; init; }

    /// <summary>
    /// Gets or sets the <c>Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c>Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; init; }

    /// <summary>
    /// Gets or sets the <c>UnknownString0</c> value.
    /// </summary>
    public string? UnknownString0 { get; init; }

    /// <summary>
    /// Gets or sets the <c>UnknownValues</c> value.
    /// </summary>
    public int[] UnknownValues { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>UnknownString1</c> value.
    /// </summary>
    public string? UnknownString1 { get; init; }

    internal static AvatarManagerASpecial Decode(MessageStream stream)
    {
        var usesCompressedData = stream.ReadBoolean();

        return new AvatarManagerASpecial
        {
            UsesCompressedData = usesCompressedData,
            Text = usesCompressedData ? null : stream.ReadOptionalString(),
            CompressedData = usesCompressedData ? stream.ReadOptionalByteArray() : null,
            Unknown0 = stream.ReadVarInt(),
            Unknown1 = stream.ReadVarInt(),
            UnknownString0 = stream.ReadOptionalString(),
            UnknownValues = stream.ReadVarIntArray(11),
            UnknownString1 = stream.ReadOptionalString(),
        };
    }

    internal void Encode(MessageStream stream)
    {
        if (UnknownValues.Length is not 11)
            throw new InvalidOperationException("Unexpected manager field count.");

        stream.WriteBoolean(UsesCompressedData);

        if (UsesCompressedData)
            stream.WriteOptionalByteArray(CompressedData);
        else
            stream.WriteOptionalString(Text);

        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteOptionalString(UnknownString0);

        foreach (var value in UnknownValues)
            stream.WriteVarInt(value);
        stream.WriteOptionalString(UnknownString1);
    }
}
