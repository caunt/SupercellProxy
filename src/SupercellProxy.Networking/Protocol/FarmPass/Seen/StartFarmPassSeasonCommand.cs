using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Seen;

/// <summary>Starts the new Farm Pass season and rerolls its tasks.</summary>
public sealed record StartFarmPassSeasonCommand : CodecCommand<StartFarmPassSeasonCommand>, ICommandCodecProvider<StartFarmPassSeasonCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartFarmPassSeasonCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new StartFarmPassSeasonCommand()));
}
