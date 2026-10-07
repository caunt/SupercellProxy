using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Dismisses previous derby rewards when the player did not meet the participation requirement.</summary>
public sealed record ResetDerbyRewardsCommand : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ResetDerbyRewardsCommandType;
    /// <summary>Decodes the command without additional payload.</summary>
    public static ResetDerbyRewardsCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        return new();
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment) { }
}
