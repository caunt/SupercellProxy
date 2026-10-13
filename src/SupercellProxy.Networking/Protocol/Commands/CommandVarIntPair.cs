using System.Runtime.InteropServices;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>Two var-ints native writes back to back.</summary>
/// <param name="Value0">The first value.</param>
/// <param name="Value1">The second value.</param>
[StructLayout(LayoutKind.Auto)]
public readonly record struct CommandVarIntPair(int Value0, int Value1) : ICodecProvider<CommandVarIntPair>
{
    /// <summary>Gets the count bound native applies to pair arrays: two encoded bytes per pair at least.</summary>
    public static CollectionCount ArrayCount { get; } = CollectionCount.PerElement(minimumElementSize: 2);

    /// <inheritdoc />
    public static ICodec<CommandVarIntPair> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static pair => pair.Value0,
        Codecs.VarInt,
        static pair => pair.Value1,
        static (value0, value1) => new CommandVarIntPair(value0, value1)
    );
}
