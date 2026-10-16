using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Joining;

/// <summary>Carries one neighborhood member profile; the client does not handle it.</summary>
/// <param name="Member">Profile of the member.</param>
public sealed record NeighborhoodMemberMessage(NeighborhoodMemberEntry Member) : CodecMessage<NeighborhoodMemberMessage>, ICodecProvider<NeighborhoodMemberMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodMemberMessage> Codec { get; } = Codecs.Composite(
        SupercellProxy.Networking.Protocol.Neighborhoods.Joining.NeighborhoodMemberEntry.Codec,
        static message => message.Member,
        static member => new NeighborhoodMemberMessage(member)
    );
}
