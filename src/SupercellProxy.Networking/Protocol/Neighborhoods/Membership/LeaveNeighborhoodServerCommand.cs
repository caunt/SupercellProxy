using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Confirms removal of the player from the specified neighborhood.</summary>
public sealed record LeaveNeighborhoodServerCommand(LongId NeighborhoodId) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.LeaveNeighborhoodServerCommandType;

    /// <summary>Decodes the neighborhood identifier before server-command metadata.</summary>
    public static LeaveNeighborhoodServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadLongId());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongId(NeighborhoodId);
    }
}
