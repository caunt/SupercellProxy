using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Delivers a neighborhood notification whose native effects are presentation callbacks.</summary>
public sealed record NeighborhoodNotificationServerCommand(int Status, LongId NeighborhoodId, LongId? RelatedId) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.NeighborhoodNotificationServerCommandType;

    /// <summary>Decodes the notification before server-command metadata.</summary>
    public static NeighborhoodNotificationServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt(), stream.ReadLongId(), stream.ReadOptionalLongId());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(Status);
        stream.WriteLongId(NeighborhoodId);
        stream.WriteOptionalLongId(RelatedId);
    }
}
