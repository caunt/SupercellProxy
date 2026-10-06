using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Acknowledges the introduction for the current boat-track reward cycle.</summary>
public sealed record MarkBoatTrackCycleIntroSeenCommand(long CycleResetTimestamp) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.MarkBoatTrackCycleIntroSeenCommandType;

    /// <summary>Decodes the reward cycle's reset timestamp.</summary>
    public static MarkBoatTrackCycleIntroSeenCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MarkBoatTrackCycleIntroSeenCommand(stream.ReadVarLong());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarLong(CycleResetTimestamp);
    }
}
