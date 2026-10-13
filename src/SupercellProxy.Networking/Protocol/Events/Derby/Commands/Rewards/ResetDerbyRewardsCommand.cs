using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Dismisses previous derby rewards when the player did not meet the participation requirement.</summary>
public sealed record ResetDerbyRewardsCommand : CodecCommand<ResetDerbyRewardsCommand>, ICommandCodecProvider<ResetDerbyRewardsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ResetDerbyRewardsCommand> Codec { get; } = Codecs.Command(type: 698, Codecs.Unit(static () => new ResetDerbyRewardsCommand()), CommandLayout.MetadataFirst);
}
