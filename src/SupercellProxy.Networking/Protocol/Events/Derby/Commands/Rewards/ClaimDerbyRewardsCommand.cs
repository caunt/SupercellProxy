using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Claims the selected ordinary, bingo and bunny derby reward choices.</summary>
public sealed record ClaimDerbyRewardsCommand(DerbyRewardSelection Selection) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ClaimDerbyRewardsCommandType;
    /// <summary>Decodes the selected reward choices.</summary>
    public static ClaimDerbyRewardsCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        return new(DerbyRewardSelection.Decode(stream));
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        Selection.Encode(stream);
    }
}
