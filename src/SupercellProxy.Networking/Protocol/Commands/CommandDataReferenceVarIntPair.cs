using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>A data reference and an amount, as native writes item lists.</summary>
/// <param name="GlobalId">The referenced data id.</param>
/// <param name="Value">The amount.</param>
[StructLayout(LayoutKind.Auto)]
public readonly record struct CommandDataReferenceVarIntPair([property: JsonPropertyName("GlobalId")] int GlobalId, int Value)
    : ICodecProvider<CommandDataReferenceVarIntPair>
{
    /// <inheritdoc />
    public static ICodec<CommandDataReferenceVarIntPair> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static pair => pair.GlobalId,
        Codecs.VarInt,
        static pair => pair.Value,
        static (globalId, value) => new CommandDataReferenceVarIntPair(globalId, value)
    );
}
