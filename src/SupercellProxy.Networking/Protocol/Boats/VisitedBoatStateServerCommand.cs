using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Releases;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Defines the Visited Boat State Server Command contract.
/// </summary>
/// <summary>
/// Defines the New State contract.
/// </summary>
/// <summary>
/// Defines the Expected State contract.
/// </summary>
/// <summary>
/// Defines the Order Index contract.
/// </summary>
/// <summary>
/// Defines the Owner High contract.
/// </summary>
/// <summary>
/// Defines the Owner Low contract.
/// </summary>
/// <summary>
/// Defines the State Duration contract.
/// </summary>
/// <summary>
/// Defines the State Ticks contract.
/// </summary>
public sealed record VisitedBoatStateServerCommand(int NewState, int ExpectedState, int OrderIndex, int OwnerHigh, int OwnerLow, int StateDuration, int StateTicks)
    : CodecServerCommand<VisitedBoatStateServerCommand>, ICommandCodecProvider<VisitedBoatStateServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<VisitedBoatStateServerCommand> Codec { get; } = Codecs.Command(
        type: 810,
        Codecs.Versioned(
            Release17381.Version,
            Codecs.Composite(
                Codecs.VarInt,
                static command => command.StateTicks,
                Codecs.VarInt,
                static command => command.NewState,
                Codecs.VarInt,
                static command => command.OwnerHigh,
                Codecs.VarInt,
                static command => command.StateDuration,
                Codecs.VarInt,
                static command => command.OrderIndex,
                Codecs.VarInt,
                static command => command.ExpectedState,
                Codecs.VarInt,
                static command => command.OwnerLow,
                static (stateTicks, newState, ownerHigh, stateDuration, orderIndex, expectedState, ownerLow) => new VisitedBoatStateServerCommand(newState, expectedState, orderIndex, ownerHigh, ownerLow, stateDuration, stateTicks)
            ),
            Codecs.Composite(
                Codecs.VarInt,
                static command => command.NewState,
                Codecs.VarInt,
                static command => command.ExpectedState,
                Codecs.VarInt,
                static command => command.OrderIndex,
                Codecs.VarInt,
                static command => command.OwnerHigh,
                Codecs.VarInt,
                static command => command.OwnerLow,
                Codecs.VarInt,
                static command => command.StateDuration,
                Codecs.VarInt,
                static command => command.StateTicks,
                static (newState, expectedState, orderIndex, ownerHigh, ownerLow, stateDuration, stateTicks) => new VisitedBoatStateServerCommand(newState, expectedState, orderIndex, ownerHigh, ownerLow, stateDuration, stateTicks)
            )
        ),
        CommandLayout.MetadataFirst
    );
}
