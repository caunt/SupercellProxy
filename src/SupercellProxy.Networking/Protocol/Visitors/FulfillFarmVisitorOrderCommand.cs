using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>Accepts a waiting farm visitor's offer for its requested goods.</summary>
public sealed record FulfillFarmVisitorOrderCommand(int VisitorGlobalId) : Command
{
    /// <inheritdoc/>
    public override int Type => CommandRegistry.FulfillFarmVisitorOrderCommandType;

    /// <inheritdoc/>
    public static FulfillFarmVisitorOrderCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);


        return new FulfillFarmVisitorOrderCommand(stream.ReadVarInt());
    }

    /// <inheritdoc/>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(VisitorGlobalId);
    }
}
