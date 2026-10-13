using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Seen;

/// <summary>Marks the Farm Pass points as seen.</summary>
public sealed record MarkFarmPassPointsSeenCommand : CodecCommand<MarkFarmPassPointsSeenCommand>, ICommandCodecProvider<MarkFarmPassPointsSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkFarmPassPointsSeenCommand> Codec { get; } = Codecs.Command(type: 710, Codecs.Unit(static () => new MarkFarmPassPointsSeenCommand()));
}
