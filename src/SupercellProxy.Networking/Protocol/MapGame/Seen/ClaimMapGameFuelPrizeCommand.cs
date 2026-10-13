using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Seen;

/// <summary>Claims the pending Valley fuel wheel prize.</summary>
public sealed record ClaimMapGameFuelPrizeCommand : CodecCommand<ClaimMapGameFuelPrizeCommand>, ICommandCodecProvider<ClaimMapGameFuelPrizeCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimMapGameFuelPrizeCommand> Codec { get; } = Codecs.Command(type: 614, Codecs.Unit(static () => new ClaimMapGameFuelPrizeCommand()));
}
