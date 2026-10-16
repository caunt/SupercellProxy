using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.PrizeWheels;

/// <summary>
/// Serverbound command collecting the prize the wheel landed on.
/// </summary>
/// <param name="ClientPresentation">Flag about how the client shows the collected prize; the managed client sends false.</param>
public sealed record CollectWheelRewardCommand(bool ClientPresentation) : CodecCommand<CollectWheelRewardCommand>, ICommandCodecProvider<CollectWheelRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectWheelRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.Boolean, static command => command.ClientPresentation, static presentation => new CollectWheelRewardCommand(presentation))
    );
}
