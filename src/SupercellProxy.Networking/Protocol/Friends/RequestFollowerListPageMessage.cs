using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Requests the next follower page; the server owns the cursor for this connection.</summary>
public sealed record RequestFollowerListPageMessage : CodecMessage<RequestFollowerListPageMessage>, ICodecProvider<RequestFollowerListPageMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestFollowerListPageMessage> Codec { get; } = Codecs.Complete(
        Codecs.Unit(static () => new RequestFollowerListPageMessage()),
        trailingDataMessage: "The follower-page request has trailing data."
    );

    /// <summary>Identifies the request without exposing client data.</summary>
    public override string ToString()
    {
        return nameof(RequestFollowerListPageMessage);
    }
}
