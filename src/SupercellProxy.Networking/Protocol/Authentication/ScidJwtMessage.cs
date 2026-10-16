using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>Delivers a Supercell ID JSON Web Token and its expiration time.</summary>
public sealed record ScidJwtMessage(string Token, long ExpiresAtUnixTimeSeconds) : CodecMessage<ScidJwtMessage>, ICodecProvider<ScidJwtMessage>
{
    /// <inheritdoc />
    public static ICodec<ScidJwtMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.String,
            static message => message.Token,
            Codecs.Int64,
            static message => message.ExpiresAtUnixTimeSeconds,
            static (token, expiresAtUnixTimeSeconds) => new ScidJwtMessage(token, expiresAtUnixTimeSeconds)
        ),
        trailingDataMessage: "The Supercell ID JWT message has trailing data."
    );

    /// <summary>Omits the credential from logs.</summary>
    public override string ToString()
    {
        return $"{nameof(ScidJwtMessage)} {{ ExpiresAtUnixTimeSeconds = {ExpiresAtUnixTimeSeconds} }}";
    }
}
