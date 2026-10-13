using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.States;

/// <summary>Settles the pending boat order track points.</summary>
public sealed record SettleBoatTrackPointsCommand : CodecCommand<SettleBoatTrackPointsCommand>, ICommandCodecProvider<SettleBoatTrackPointsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SettleBoatTrackPointsCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new SettleBoatTrackPointsCommand()));
}
