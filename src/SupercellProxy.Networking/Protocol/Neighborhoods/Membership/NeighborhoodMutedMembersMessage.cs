using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Replaces the neighborhood manager's muted member ids and names; the chat refreshes.</summary>
/// <param name="MemberIds">Accounts of the muted members, matching <paramref name="MemberNames"/> by position.</param>
/// <param name="MemberNames">Names of the muted members.</param>
/// <param name="Value">Number sent with the lists; its meaning is not identified.</param>
public sealed record NeighborhoodMutedMembersMessage(ReadOnlyMemory<LongId> MemberIds, ReadOnlyMemory<string?> MemberNames, int Value) : CodecMessage<NeighborhoodMutedMembersMessage>, ICodecProvider<NeighborhoodMutedMembersMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodMutedMembersMessage> Codec { get; } = Codecs.Composite(
        Codecs.Memory(Codecs.LongId),
        static message => message.MemberIds,
        Codecs.Memory(Codecs.OptionalString),
        static message => message.MemberNames,
        Codecs.VarInt,
        static message => message.Value,
        static (memberIds, memberNames, @value) => new NeighborhoodMutedMembersMessage(memberIds, memberNames, value)
    );
}
