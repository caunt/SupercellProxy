using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Accounts.Rewards;

/// <summary>Claims the configured one-time account-link reward.</summary>
public sealed record ClaimAccountLinkRewardCommand(AccountLinkRewardKind Kind) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ClaimAccountLinkRewardCommandType;

    /// <summary>Decodes the reward selection before the command metadata.</summary>
    public static ClaimAccountLinkRewardCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new((AccountLinkRewardKind)stream.ReadInt32());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteInt32((int)Kind);
    }
}
