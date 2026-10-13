using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>Event type 2: a pawn moved along the map.</summary>
/// <param name="SourceNodeId">The node the pawn left.</param>
/// <param name="ReachedNodeId">The node the pawn reached.</param>
/// <param name="FuelSpent">The fuel the move cost.</param>
/// <param name="Pawn">The moved pawn.</param>
/// <param name="VisitedNodeIds">The nodes visited on the way.</param>
/// <param name="ContinuationRoute">The route still ahead.</param>
/// <param name="EmptyNodeTask">The task spawned on an empty node.</param>
/// <param name="DestinationTask">The task at the destination.</param>
/// <param name="EscapedAnimals">The sanctuary animals that escaped.</param>
public sealed record MapGamePawnMovedEvent(
    int SourceNodeId,
    int ReachedNodeId,
    int FuelSpent,
    MapGamePawn? Pawn,
    ReadOnlyMemory<int>? VisitedNodeIds,
    ReadOnlyMemory<int>? ContinuationRoute,
    MapGameTask? EmptyNodeTask,
    MapGameTask? DestinationTask,
    MapGameTaskCollection? EscapedAnimals
) : MapGameEvent(MapGameEvent.PawnMovedType), ICodecProvider<MapGamePawnMovedEvent>
{
    /// <inheritdoc />
    public static new ICodec<MapGamePawnMovedEvent> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.SourceNodeId,
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.ReachedNodeId,
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.FuelSpent,
        Codecs.Optional(MapGamePawn.Codec),
        static mapGameEvent => mapGameEvent.Pawn,
        Codecs.OptionalValue(Codecs.Memory(Codecs.VarInt)),
        static mapGameEvent => mapGameEvent.VisitedNodeIds,
        Codecs.OptionalValue(Codecs.Memory(Codecs.VarInt)),
        static mapGameEvent => mapGameEvent.ContinuationRoute,
        Codecs.Optional(MapGameTask.Codec),
        static mapGameEvent => mapGameEvent.EmptyNodeTask,
        Codecs.Optional(MapGameTask.Codec),
        static mapGameEvent => mapGameEvent.DestinationTask,
        Codecs.Optional(MapGameTaskCollection.Codec),
        static mapGameEvent => mapGameEvent.EscapedAnimals,
        static (sourceNodeId, reachedNodeId, fuelSpent, pawn, visitedNodeIds, continuationRoute, emptyNodeTask, destinationTask, escapedAnimals) => new MapGamePawnMovedEvent(sourceNodeId, reachedNodeId, fuelSpent, pawn, visitedNodeIds, continuationRoute, emptyNodeTask, destinationTask, escapedAnimals)
    );
}
