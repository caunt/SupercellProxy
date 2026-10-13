using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Voting;

/// <summary>Two decoration designs of one event offered for a vote.</summary>
/// <param name="EventId">The decoration event id.</param>
/// <param name="First">The first candidate design.</param>
/// <param name="Second">The second candidate design.</param>
/// <param name="EventVariantId">The decoration event variant id.</param>
public sealed record DecorationVotePair(int EventId, DecorationVoteCandidate? First, DecorationVoteCandidate? Second, int EventVariantId) : ICodecProvider<DecorationVotePair>
{
    /// <inheritdoc />
    public static ICodec<DecorationVotePair> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.EventId,
        Codecs.Optional(DecorationVoteCandidate.Codec),
        static value => value.First,
        Codecs.Optional(DecorationVoteCandidate.Codec),
        static value => value.Second,
        Codecs.VarInt,
        static value => value.EventVariantId,
        static (eventId, first, second, eventVariantId) => new DecorationVotePair(eventId, first, second, eventVariantId)
    );
}
