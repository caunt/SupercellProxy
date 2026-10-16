using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Reports a farm's friend count to the client's friend manager.</summary>
public sealed record FriendCountMessage(LongId HomeId, int FriendCount) : CodecMessage<FriendCountMessage>, ICodecProvider<FriendCountMessage>
{
    /// <inheritdoc />
    public static ICodec<FriendCountMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.LongId,
            static message => message.HomeId,
            Codecs.VarInt,
            static message => message.FriendCount,
            static (homeId, friendCount) => new FriendCountMessage(homeId, friendCount)
        ),
        trailingDataMessage: "The friend count has trailing data."
    );

    /// <summary>Omits the private farm id from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(FriendCountMessage);
    }
}
