using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Serverbound command recording that the player has looked at the boat, which clears its "new" marker. Either the docked boat
/// (while it is loading or choosing a destination) or the announced next boat (while no boat is docked) can be acknowledged, once each.
/// </summary>
public sealed record MarkBoatSeenCommand : CodecCommand<MarkBoatSeenCommand>, ICommandCodecProvider<MarkBoatSeenCommand>
{
    /// <summary>
    /// Acknowledges either the docked boat or the next one.
    /// </summary>
    /// <param name="nextBoat">True for the announced next boat; false for the boat that is docked now.</param>
    public MarkBoatSeenCommand(bool nextBoat)
    {
        NextBoat = nextBoat;
    }

    /// <inheritdoc />
    public static CommandCodec<MarkBoatSeenCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.Boolean, static command => command.NextBoat, static nextBoat => new MarkBoatSeenCommand(nextBoat)),
        CommandLayout.MetadataFirst
    );

    /// <summary>
    /// True when the announced next boat is acknowledged; false for the boat that is docked now.
    /// </summary>
    public bool NextBoat { get; }
}
