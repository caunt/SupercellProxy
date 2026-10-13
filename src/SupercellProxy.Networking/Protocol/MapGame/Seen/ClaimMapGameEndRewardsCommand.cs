using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Seen;

/// <summary>Claims the rewards collected when the Valley season ended.</summary>
public sealed record ClaimMapGameEndRewardsCommand : CodecCommand<ClaimMapGameEndRewardsCommand>, ICommandCodecProvider<ClaimMapGameEndRewardsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimMapGameEndRewardsCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new ClaimMapGameEndRewardsCommand()));
}
