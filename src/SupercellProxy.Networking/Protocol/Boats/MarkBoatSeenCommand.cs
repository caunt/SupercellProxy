using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Defines the Mark Boat Seen Command contract.
/// </summary>
public sealed record MarkBoatSeenCommand : CodecCommand<MarkBoatSeenCommand>, ICommandCodecProvider<MarkBoatSeenCommand>
{
    /// <summary>
    /// Provides the Command Type value or operation.
    /// </summary>
    public const int CommandType = 600;

    /// <summary>
    /// Provides the Mark Boat Seen Command value or operation.
    /// </summary>
    public MarkBoatSeenCommand(bool nextBoat)
    {
        NextBoat = nextBoat;
    }

    /// <inheritdoc />
    public static CommandCodec<MarkBoatSeenCommand> Codec { get; } = Codecs.Command(
        type: 600,
        Codecs.Composite(Codecs.Boolean, static command => command.NextBoat, static (nextBoat) => new MarkBoatSeenCommand(nextBoat)),
        CommandLayout.MetadataFirst
    );

    /// <summary>
    /// Gets the Next Boat value.
    /// </summary>
    public bool NextBoat { get; }
}
