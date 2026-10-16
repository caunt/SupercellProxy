using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Clientbound update that moves the boat of the farm being visited to a new state (arriving, loading, departing and so on), so the
/// visitor sees the owner's boat change while visiting. Ignored unless the visitor is on that owner's farm and the boat is still in
/// the expected state.
/// </summary>
/// <param name="NewState">Boat state to switch to, as native numbers them (1 away, 3 arriving, 4 loading, 8 choosing a destination…).</param>
/// <param name="ExpectedState">State the boat must be in for the update to apply; a mismatch means the visitor's copy is stale.</param>
/// <param name="OrderIndex">Destination order the owner chose, which becomes the crates being loaded.</param>
/// <param name="OwnerHigh">First word of the owner's home id.</param>
/// <param name="OwnerLow">Second word of the owner's home id.</param>
/// <param name="StateDuration">How long the new state lasts, in game updates.</param>
/// <param name="StateTicks">How many of those updates have already passed.</param>
public sealed record VisitedBoatStateServerCommand(int NewState, int ExpectedState, int OrderIndex, int OwnerHigh, int OwnerLow, int StateDuration, int StateTicks)
    : CodecServerCommand<VisitedBoatStateServerCommand>, ICommandCodecProvider<VisitedBoatStateServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<VisitedBoatStateServerCommand> Codec { get; } = Codecs.Command(
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
