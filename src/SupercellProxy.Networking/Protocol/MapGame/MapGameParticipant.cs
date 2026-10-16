using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Inventory;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// A Valley board player as kept in the client avatar; its fields follow the same order as <see cref="MapGamePawn"/>, with the
/// numbers grouped into arrays.
/// </summary>
/// <param name="Id0">Account of the player, at the position of <see cref="MapGamePawn.AvatarId"/>.</param>
/// <param name="Id1">Neighbourhood of the player, at the position of <see cref="MapGamePawn.NeighborhoodId"/>.</param>
/// <param name="Values">
/// Five numbers at the positions of the pawn's farm level, current node, empty nodes travelled, carried-animal immunity and
/// empty nodes travelled while carrying animals.
/// </param>
/// <param name="RouteValues">Nodes the player revealed, at the position of <see cref="MapGamePawn.VisitedNodeIds"/>.</param>
/// <param name="TaskDataIds">Id list at the position of <see cref="MapGamePawn.SelectedOptions"/>.</param>
/// <param name="NamePresent">Whether the name was sent.</param>
/// <param name="Name">Name shown for the player.</param>
/// <param name="OptionalValues">Emblem background, pattern and symbol; null when no emblem is set.</param>
/// <param name="DataId">Global id at the position of <see cref="MapGamePawn.DerbyTaskGlobalId"/>.</param>
/// <param name="Entries">Table entries with values, at the position of <see cref="MapGamePawn.PendingNotifications"/>.</param>
public sealed record MapGameParticipant(
    [property: JsonPropertyName("Id0")] LongId? Id0,
    [property: JsonPropertyName("Id1")] LongId? Id1,
    int[] Values,
    int[] RouteValues,
    [property: JsonPropertyName("TaskDataIds")] int[] TaskDataIds,
    bool NamePresent,
    string? Name,
    int[]? OptionalValues,
    [property: JsonPropertyName("DataId")] int DataId,
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
