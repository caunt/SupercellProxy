using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Claims the previous derby's podium rewards and finishes its presentation.</summary>
public sealed record ClaimDerbyPlacementRewardsCommand : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ClaimDerbyPlacementRewardsCommandType;
    /// <summary>Decodes the command without additional payload.</summary>
    public static ClaimDerbyPlacementRewardsCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        return new();
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment) { }
}
