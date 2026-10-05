using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>Declines a waiting farm visitor's goods offer.</summary>
public sealed record RejectFarmVisitorOrderCommand(int VisitorGlobalId) : Command
{
    /// <inheritdoc/>
    public override int Type => CommandRegistry.RejectFarmVisitorOrderCommandType;

    /// <inheritdoc/>
    public static RejectFarmVisitorOrderCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int visitorGlobalId = stream.ReadVarInt();


        return new RejectFarmVisitorOrderCommand(visitorGlobalId);
    }

    /// <inheritdoc/>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(VisitorGlobalId);
    }
}
