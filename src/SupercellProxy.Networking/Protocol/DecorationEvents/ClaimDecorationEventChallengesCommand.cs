using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.DecorationEvents;

/// <summary>Claims the decoration event challenge reward.</summary>
public sealed record ClaimDecorationEventChallengesCommand : CodecCommand<ClaimDecorationEventChallengesCommand>, ICommandCodecProvider<ClaimDecorationEventChallengesCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimDecorationEventChallengesCommand> Codec { get; } = Codecs.Command(type: 637, Codecs.Unit(static () => new ClaimDecorationEventChallengesCommand()));
}
