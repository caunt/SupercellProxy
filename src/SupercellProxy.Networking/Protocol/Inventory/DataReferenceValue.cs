using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Inventory;

/// <summary>
/// A number stored against a game data entry, as inventory maps keep them; for example how many of an item the player owns.
/// </summary>
/// <param name="GlobalDataId">Global id of the game data entry the number belongs to.</param>
/// <param name="Value">The number stored for that entry.</param>
public sealed record DataReferenceValue([property: JsonPropertyName("GlobalDataId")] int GlobalDataId, int Value) : ICodecProvider<DataReferenceValue>
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
