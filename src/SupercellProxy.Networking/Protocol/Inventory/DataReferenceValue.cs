using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Inventory;

/// <summary>
/// Represents <c language="csharp">DataReferenceValue</c>.
/// </summary>
public sealed record DataReferenceValue([property: System.Text.Json.Serialization.JsonPropertyName("GlobalDataId")] int GlobalDataId, int Value) : ICodecProvider<DataReferenceValue>
{
    /// <inheritdoc />
    public static ICodec<DataReferenceValue> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.GlobalDataId,
        Codecs.VarInt,
        static value => value.Value,
        static (globalDataId, @value) => new DataReferenceValue(globalDataId, value)
    );
}
