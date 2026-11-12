using SupercellProxy.Networking.Protocol.MapGame.Events.PawnEvents;
using SupercellProxy.Networking.Protocol.MapGame.Events.TaskEvents;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>
/// <para>One polymorphic native map-game event carried by server command 274.</para>
/// <para>The event type selects the event record; the records follow the native 1.72.84 event factory.</para>
/// </summary>
/// <param name="Type">The native event type.</param>
public abstract record MapGameEvent(int Type) : ICodecProvider<MapGameEvent>
{
    /// <summary>Records a participant collecting a chicken task.</summary>
    public const int ChickenTaskCollectedType = 21;

    /// <summary>Records a participant's contribution to the shared chicken goal.</summary>
    public const int ChickensCollectedType = 18;

    /// <summary>Carries a value followed by a data reference.</summary>
    public const int DataReferenceType = 40;

    /// <summary>Completes a personal Valley task, including dump and delivery tasks, with its authoritative pawn and task.</summary>
    public const int DumpTaskCompletedType = 4;

    /// <summary>Removes the participant's escaped sanctuary-animal tasks.</summary>
    public const int EscapedSanctuaryAnimalsRemovedType = 38;

    /// <summary>Updates the transient Valley flag of a linked Chronos event.</summary>
    public const int EventFlagUpdatedType = 26;

    /// <summary>Collects a free gas-station offer.</summary>
    public const int GasStationCollectedType = 34;

    /// <summary>Collects a paid gas-station offer.</summary>
    public const int GasStationPurchasedType = 35;

    /// <summary>Replaces a participant's gas-station offer.</summary>
    public const int GasStationUpdatedType = 36;

    /// <summary>Carries an id followed by two values; native names it nowhere in the client.</summary>
    public const int IdValuesType = 5;

    /// <summary>Adds an emoji to a map node.</summary>
    public const int NodeEmojiAddedType = 22;

    /// <summary>Marks an existing node emoji as collected by a Valley participant.</summary>
    public const int NodeEmojiCollectedType = 24;

    /// <summary>Removes every node emoji matching the supplied avatar, node, and data filters.</summary>
    public const int NodeEmojiRemovedType = 23;

    /// <summary>Completes an obstacle and records its helper.</summary>
    public const int ObstacleCompletedType = 17;

    /// <summary>Requests another participant's help with an obstacle.</summary>
    public const int ObstacleHelpRequestedType = 16;

    /// <summary>Completes an obstacle using the transmitted diamond charge.</summary>
    public const int ObstaclePaidCompletionType = 15;

    /// <summary>Refreshes an existing pawn's level, name, options and neighborhood profile.</summary>
    public const int PawnDetailsUpdatedType = 28;

    /// <summary>Moves a Valley pawn and carries the authoritative movement projection.</summary>
    public const int PawnMovedType = 2;

    /// <summary>Sets or clears the neighborhood task attached to a pawn.</summary>
    public const int PawnNeighborhoodTaskUpdatedType = 25;

    /// <summary>Refreshes an existing pawn's neighborhood identity and emblem.</summary>
    public const int PawnNeighborhoodUpdatedType = 31;

    /// <summary>Changes a pawn's retained notification and value entries.</summary>
    public const int PawnNotificationUpdatedType = 27;

    /// <summary>Adds or refreshes a Valley pawn's social profile.</summary>
    public const int PawnProfileUpdatedType = 30;

    /// <summary>Expires an existing personal task using the server's task state.</summary>
    public const int PawnTaskExpiredType = 6;

    /// <summary>Carries a pawn and a task; native names it nowhere in the client.</summary>
    public const int PawnTaskType = 19;

    /// <summary>Replaces a personal task at its map node.</summary>
    public const int PawnTaskUpdatedType = 8;

    /// <summary>Carries a value followed by an optional live event profile.</summary>
    public const int ProfileType = 39;

    /// <summary>Assigns an existing shared sanctuary-animal task to a Valley participant.</summary>
    public const int SanctuaryAnimalCollectedType = 32;

    /// <summary>Records a participant's contribution to the shared sanctuary-animal goal.</summary>
    public const int SanctuaryAnimalsDeliveredType = 37;

    /// <summary>Offloads a participant's carried sanctuary animals.</summary>
    public const int SanctuaryAnimalsOffloadedType = 33;

    /// <summary>Confirms or releases a participant's shared-task submission.</summary>
    public const int SharedTaskCommitResultType = 29;

    /// <summary>Processes shared-task completion and participant rewards.</summary>
    public const int SharedTaskCompletedType = 13;

    /// <summary>Expires an existing task in the shared task group.</summary>
    public const int SharedTaskExpiredType = 9;

    /// <summary>Assigns a free participant slot in a shared task.</summary>
    public const int SharedTaskJoinedType = 12;

    /// <summary>Marks a shared dump task and records the participant who marked it.</summary>
    public const int SharedTaskMarkedType = 20;

    /// <summary>Removes an existing task from the shared task group.</summary>
    public const int SharedTaskRemovedType = 10;

    /// <summary>Replaces a task in the shared task group.</summary>
    public const int SharedTaskUpdatedType = 11;

    /// <summary>Identifies a shared map-game state synchronization event.</summary>
    public const int StateSynchronizationType = 1;

    /// <summary>Skips a task's remaining replacement cooldown.</summary>
    public const int TaskCooldownSkippedType = 14;

    /// <summary>Removes an existing task from its owner or shared group.</summary>
    public const int TaskRemovedType = 7;

    /// <summary>
    /// Gets the wire codec: the event type selects the event record.
    /// </summary>
    /// <value>A codec that reads the variable-length event type first and hands the rest to the matching event record's codec; a type with no listed record fails with <see cref="InvalidDataException"/> in either direction.</value>
    public static ICodec<MapGameEvent> Codec { get; } = Codecs.Dispatch(Codecs.VarInt,
        static mapGameEvent => mapGameEvent.Type,
        cases: [
            Case(StateSynchronizationType, MapGameStateSynchronizationEvent.Codec),
            Case(PawnMovedType, MapGamePawnMovedEvent.Codec),
            Case(DumpTaskCompletedType, MapGamePawnTaskEvent.CodecFor(DumpTaskCompletedType)),
            Case(IdValuesType, MapGameIdValuesEvent.Codec),
            Case(PawnTaskExpiredType, MapGamePawnTaskEvent.CodecFor(PawnTaskExpiredType)),
            Case(TaskRemovedType, MapGamePawnTaskEvent.CodecFor(TaskRemovedType)),
            Case(PawnTaskUpdatedType, MapGamePawnTaskEvent.CodecFor(PawnTaskUpdatedType)),
            Case(SharedTaskExpiredType, MapGamePawnTaskEvent.CodecFor(SharedTaskExpiredType)),
            Case(SharedTaskRemovedType, MapGamePawnTaskEvent.CodecFor(SharedTaskRemovedType)),
            Case(SharedTaskUpdatedType, MapGamePawnTaskEvent.CodecFor(SharedTaskUpdatedType)),
            Case(SharedTaskJoinedType, MapGamePawnTaskEvent.CodecFor(SharedTaskJoinedType)),
            Case(SharedTaskCompletedType, MapGamePawnTaskEvent.CodecFor(SharedTaskCompletedType)),
            Case(TaskCooldownSkippedType, MapGamePawnTaskValueEvent.CodecFor(TaskCooldownSkippedType)),
            Case(ObstaclePaidCompletionType, MapGamePawnTaskValueEvent.CodecFor(ObstaclePaidCompletionType)),
            Case(ObstacleHelpRequestedType, MapGamePawnTaskEvent.CodecFor(ObstacleHelpRequestedType)),
            Case(ObstacleCompletedType, MapGamePawnTaskRewardEvent.CodecFor(ObstacleCompletedType)),
            Case(ChickensCollectedType, MapGameContributionEvent.CodecFor(ChickensCollectedType)),
            Case(PawnTaskType, MapGamePawnTaskEvent.CodecFor(PawnTaskType)),
            Case(SharedTaskMarkedType, MapGamePawnTaskEvent.CodecFor(SharedTaskMarkedType)),
            Case(ChickenTaskCollectedType, MapGamePawnTaskRewardEvent.CodecFor(ChickenTaskCollectedType)),
            Case(NodeEmojiAddedType, MapGameNodeEmojiAddedEvent.Codec),
            Case(NodeEmojiRemovedType, MapGameNodeEmojiEvent.CodecFor(NodeEmojiRemovedType)),
            Case(NodeEmojiCollectedType, MapGameNodeEmojiEvent.CodecFor(NodeEmojiCollectedType)),
            Case(PawnNeighborhoodTaskUpdatedType, MapGamePawnNeighborhoodTaskEvent.Codec),
            Case(EventFlagUpdatedType, MapGameEventFlagEvent.Codec),
            Case(PawnNotificationUpdatedType, MapGamePawnNotificationEvent.Codec),
            Case(PawnDetailsUpdatedType, MapGamePawnEvent.CodecFor(PawnDetailsUpdatedType)),
            Case(SharedTaskCommitResultType, MapGameSharedTaskCommitResultEvent.Codec),
            Case(PawnProfileUpdatedType, MapGamePawnEvent.CodecFor(PawnProfileUpdatedType)),
            Case(PawnNeighborhoodUpdatedType, MapGamePawnEvent.CodecFor(PawnNeighborhoodUpdatedType)),
            Case(SanctuaryAnimalCollectedType, MapGamePawnTaskRewardEvent.CodecFor(SanctuaryAnimalCollectedType)),
            Case(SanctuaryAnimalsOffloadedType, MapGameSanctuaryAnimalsOffloadedEvent.Codec),
            Case(GasStationCollectedType, MapGamePawnTaskEvent.CodecFor(GasStationCollectedType)),
            Case(GasStationPurchasedType, MapGamePawnTaskValueEvent.CodecFor(GasStationPurchasedType)),
            Case(GasStationUpdatedType, MapGamePawnTaskEvent.CodecFor(GasStationUpdatedType)),
            Case(SanctuaryAnimalsDeliveredType, MapGameContributionEvent.CodecFor(SanctuaryAnimalsDeliveredType)),
            Case(EscapedSanctuaryAnimalsRemovedType, MapGameEscapedSanctuaryAnimalsRemovedEvent.Codec),
            Case(ProfileType, MapGameProfileEvent.Codec),
            Case(DataReferenceType, MapGameDataReferenceEvent.Codec),
        ]);

    private static KeyValuePair<int, ICodec<MapGameEvent>> Case<TEvent>(int type, ICodec<TEvent> codec) where TEvent : MapGameEvent
    {
        return new KeyValuePair<int, ICodec<MapGameEvent>>(type, Codecs.Subtype<MapGameEvent, TEvent>(codec));
    }
}
