using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.PrizeWheels;

/// <summary>
/// Defines the Collect Wheel Reward Command contract.
/// </summary>
/// <summary>
/// Defines the Client Presentation contract.
/// </summary>
public sealed record CollectWheelRewardCommand(bool ClientPresentation) : CodecCommand<CollectWheelRewardCommand>, ICommandCodecProvider<CollectWheelRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectWheelRewardCommand> Codec { get; } = Codecs.Command(
        type: 80,
        Codecs.Composite(
            Codecs.Boolean,
            static command => command.ClientPresentation,
            static (presentation) => new CollectWheelRewardCommand(presentation)
        )
    );
}
