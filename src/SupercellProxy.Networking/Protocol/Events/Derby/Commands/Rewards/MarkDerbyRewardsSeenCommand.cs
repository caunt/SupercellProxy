using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Records the seen threshold count for one derby reward family.</summary>
public sealed record MarkDerbyRewardsSeenCommand(int Count, bool ThresholdRewards, bool BunnyRewards) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.MarkDerbyRewardsSeenCommandType;
    /// <summary>Decodes the seen count and reward-family selectors.</summary>
    public static MarkDerbyRewardsSeenCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt(), stream.ReadBoolean(), stream.ReadBoolean());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(Count);
        stream.WriteBoolean(ThresholdRewards);
        stream.WriteBoolean(BunnyRewards);
    }
}
