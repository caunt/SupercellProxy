using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Records the number of derby bunny appearances shown to the player.</summary>
public sealed record MarkFlyingDerbyBunniesSeenCommand(int Count) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.MarkFlyingDerbyBunniesSeenCommandType;

    /// <summary>Decodes the seen count before the command metadata.</summary>
    public static MarkFlyingDerbyBunniesSeenCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(Count);
    }
}
