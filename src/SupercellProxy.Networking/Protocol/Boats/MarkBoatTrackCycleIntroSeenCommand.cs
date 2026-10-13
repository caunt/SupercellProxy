using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Acknowledges the introduction for the current boat-track reward cycle.</summary>
public sealed record MarkBoatTrackCycleIntroSeenCommand(long CycleResetTimestamp) : CodecCommand<MarkBoatTrackCycleIntroSeenCommand>, ICommandCodecProvider<MarkBoatTrackCycleIntroSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkBoatTrackCycleIntroSeenCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarLong,
            static command => command.CycleResetTimestamp,
            static (cycleResetTimestamp) => new MarkBoatTrackCycleIntroSeenCommand(cycleResetTimestamp)
        ),
        CommandLayout.MetadataFirst
    );
}
