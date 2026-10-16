using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>An id paired with a number, as ranking lists send them.</summary>
/// <param name="Id">Account or neighbourhood id of the entry.</param>
/// <param name="Value">Number of the entry, such as its score.</param>
public sealed record NeighborhoodIdValue([property: JsonPropertyName("Id")] LongId Id, int Value) : ICodecProvider<NeighborhoodIdValue>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodIdValue> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static entry => entry.Id,
        Codecs.VarInt,
        static entry => entry.Value,
        static (id, value) => new NeighborhoodIdValue(id, value)
    );
}
