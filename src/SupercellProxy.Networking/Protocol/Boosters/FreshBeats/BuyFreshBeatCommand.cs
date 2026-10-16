using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boosters.FreshBeats;

/// <summary>Buys a Fresh Beats booster.</summary>
/// <param name="BeatIndex">Position of the bought beat among those Fresh Beats offers.</param>
public sealed record BuyFreshBeatCommand(int BeatIndex) : CodecCommand<BuyFreshBeatCommand>, ICommandCodecProvider<BuyFreshBeatCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyFreshBeatCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.BeatIndex, static beatIndex => new BuyFreshBeatCommand(beatIndex)),
        CommandLayout.MetadataFirst
    );
}
