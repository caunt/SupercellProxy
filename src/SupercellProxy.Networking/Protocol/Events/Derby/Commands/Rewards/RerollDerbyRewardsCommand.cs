using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Purchases replacement choices for selected derby reward thresholds.</summary>
public sealed record RerollDerbyRewardsCommand(DerbyRewardSelection Selection) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.RerollDerbyRewardsCommandType;
    /// <summary>Decodes the selected reward thresholds.</summary>
    public static RerollDerbyRewardsCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        return new(DerbyRewardSelection.Decode(stream));
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        Selection.Encode(stream);
    }
}
