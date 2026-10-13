using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Defines the Neighborhood Id Value contract.
/// </summary>
/// <summary>
/// Defines the Id contract.
/// </summary>
/// <summary>
/// Defines the Value contract.
/// </summary>
public sealed record NeighborhoodIdValue([property: System.Text.Json.Serialization.JsonPropertyName("Id")] LongId Id, int Value) : ICodecProvider<NeighborhoodIdValue>
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
