using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Events;

/// <summary>Reports the final standings of the previous neighborhood event.</summary>
/// <param name="MemberScores">The members' scores, which the social manager stores.</param>
/// <param name="Neighborhoods">The neighborhoods' standings, which the neighborhood manager stores.</param>
/// <param name="Status">Result code of the request, as the server numbers it.</param>
/// <param name="EventName">Name of the event the standings belong to.</param>
public sealed record PreviousEventStatusResponseMessage(
    ReadOnlyMemory<PreviousEventMemberScore>? MemberScores,
    ReadOnlyMemory<PreviousEventNeighborhoodScore>? Neighborhoods,
    int Status,
    string? EventName
) : CodecMessage<PreviousEventStatusResponseMessage>, ICodecProvider<PreviousEventStatusResponseMessage>
{
    /// <inheritdoc />
    public static ICodec<PreviousEventStatusResponseMessage> Codec { get; } = Codecs.Composite(
        Codecs.NullableMemory(PreviousEventMemberScore.Codec),
        static message => message.MemberScores,
        Codecs.NullableMemory(PreviousEventNeighborhoodScore.Codec),
        static message => message.Neighborhoods,
        Codecs.VarInt,
        static message => message.Status,
        Codecs.OptionalString,
        static message => message.EventName,
        static (memberScores, neighborhoods, status, eventName) => new PreviousEventStatusResponseMessage(memberScores, neighborhoods, status, eventName)
    );
}
