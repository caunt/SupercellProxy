using SupercellProxy.Networking.Protocol.Inventory;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// Defines the Map Game Participant contract.
/// </summary>
/// <summary>
/// Defines the Id0 contract.
/// </summary>
/// <summary>
/// Defines the Id1 contract.
/// </summary>
/// <summary>
/// Defines the Values contract.
/// </summary>
/// <summary>
/// Defines the Route Values contract.
/// </summary>
/// <summary>
/// Defines the Task Data Ids contract.
/// </summary>
/// <summary>
/// Defines the Name Present contract.
/// </summary>
/// <summary>
/// Defines the Name contract.
/// </summary>
/// <summary>
/// Defines the Optional Values contract.
/// </summary>
/// <summary>
/// Defines the Data Id contract.
/// </summary>
/// <summary>
/// Defines the Entries contract.
/// </summary>
public sealed record MapGameParticipant(
    [property: System.Text.Json.Serialization.JsonPropertyName("Id0")] LongId? Id0,
    [property: System.Text.Json.Serialization.JsonPropertyName("Id1")] LongId? Id1,
    int[] Values,
    int[] RouteValues,
    [property: System.Text.Json.Serialization.JsonPropertyName("TaskDataIds")] int[] TaskDataIds,
    bool NamePresent,
    string? Name,
    int[]? OptionalValues,
    [property: System.Text.Json.Serialization.JsonPropertyName("DataId")] int DataId,
    DataReferenceValue[] Entries
) : ICodecProvider<MapGameParticipant>
{
    /// <inheritdoc />
    public static ICodec<MapGameParticipant> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static participant => participant.Id0,
        Codecs.OptionalLongId,
        static participant => participant.Id1,
        Codecs.FixedArray(Codecs.VarInt, length: 5),
        static participant => participant.Values,
        Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
        static participant => participant.RouteValues,
        Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
        static participant => participant.TaskDataIds,
        Codecs.Flagged(Codecs.Present(Codecs.OptionalString), absent: default, static name => name.IsPresent),
        static participant => new Presence<string?>(participant.NamePresent, participant.Name),
        Codecs.Optional(Codecs.FixedArray(Codecs.VarInt, length: 3)),
        static participant => participant.OptionalValues,
        Codecs.VarInt,
        static participant => participant.DataId,
        Codecs.Array(DataReferenceValue.Codec, CollectionCount.Bounded),
        static participant => participant.Entries,
        static (id0, id1, values, routeValues, taskDataIds, name, optionalValues, dataId, entries) => new MapGameParticipant(id0, id1, values, routeValues, taskDataIds, name.IsPresent, name.Value, optionalValues, dataId, entries)
    );
}
