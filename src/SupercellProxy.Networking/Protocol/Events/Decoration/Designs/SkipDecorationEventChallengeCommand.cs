using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Designs;

/// <summary>Pays to skip a decoration event challenge.</summary>
/// <param name="Price">Skip price the client showed the player.</param>
/// <param name="EventId">Decoration event the challenge belongs to.</param>
/// <param name="ChallengeIndex">Position of the challenge in the event's list.</param>
public sealed record SkipDecorationEventChallengeCommand(int Price, int EventId, int ChallengeIndex) : CodecCommand<SkipDecorationEventChallengeCommand>, ICommandCodecProvider<SkipDecorationEventChallengeCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SkipDecorationEventChallengeCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Price,
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.ChallengeIndex,
            static (price, eventId, challengeIndex) => new SkipDecorationEventChallengeCommand(price, eventId, challengeIndex)
        ),
        CommandLayout.MetadataFirst
    );
}
