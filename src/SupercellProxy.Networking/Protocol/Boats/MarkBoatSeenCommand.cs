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
    /// Initializes a new instance of the <see cref="MarkBoatSeenCommand"/> record. Acknowledges either the docked boat or the next one.
    /// </summary>
    /// <param name="nextBoat">True for the announced next boat; <see langword="false"/> for the boat that is docked now.</param>
    public MarkBoatSeenCommand(bool nextBoat)
    {
        NextBoat = nextBoat;
    }

    /// <inheritdoc />
    public static CommandCodec<MarkBoatSeenCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.Boolean, static command => command.NextBoat, static nextBoat => new MarkBoatSeenCommand(nextBoat)),
        CommandLayout.MetadataFirst);

    /// <summary>
    /// Gets a value indicating which boat is acknowledged: the announced next boat or the docked one.
    /// </summary>
    /// <value><see langword="true"/> if the announced next boat is acknowledged, while no boat is docked; otherwise, <see langword="false"/>.</value>
    /// <remarks><see langword="false"/> acknowledges the boat that is docked now and loading or choosing a destination.</remarks>
    public bool NextBoat { get; }
}
