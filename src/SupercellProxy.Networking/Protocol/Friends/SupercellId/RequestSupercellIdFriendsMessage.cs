using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.SupercellId;

/// <summary>Requests the Supercell ID friend list.</summary>
public sealed record RequestSupercellIdFriendsMessage : CodecMessage<RequestSupercellIdFriendsMessage>, ICodecProvider<RequestSupercellIdFriendsMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestSupercellIdFriendsMessage> Codec { get; } = Codecs.Unit(static () => new RequestSupercellIdFriendsMessage());
}
