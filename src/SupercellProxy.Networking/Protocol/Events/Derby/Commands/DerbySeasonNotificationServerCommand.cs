using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Reports a derby season result whose native effects are presentation callbacks.</summary>
public sealed record DerbySeasonNotificationServerCommand(int Status, LongId NeighborhoodId, LongId? InstanceId, int Value) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.DerbySeasonNotificationServerCommandType;

    /// <summary>Decodes the notification before its server-command metadata.</summary>
    public static DerbySeasonNotificationServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt(), stream.ReadLongId(), stream.ReadOptionalLongId(), stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(Status);
        stream.WriteLongId(NeighborhoodId);
        stream.WriteOptionalLongId(InstanceId);
        stream.WriteVarInt(Value);
    }
}
