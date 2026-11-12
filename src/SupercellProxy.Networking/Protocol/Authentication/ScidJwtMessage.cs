using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>Delivers a Supercell ID JSON Web Token and its expiration time.</summary>
/// <param name="Token">
/// The signed JWT in its compact text form, written as a length-prefixed string; it is a credential, so <see cref="ToString"/> leaves it out.
/// </param>
/// <param name="ExpiresAtUnixTimeSeconds">
/// When the token stops being valid, as a 64-bit count of seconds since the Unix epoch in UTC; reading fails if any bytes follow it.
/// </param>
public sealed record ScidJwtMessage(string Token, long ExpiresAtUnixTimeSeconds) : CodecMessage<ScidJwtMessage>, ICodecProvider<ScidJwtMessage>
{
    /// <inheritdoc />
    public static ICodec<ScidJwtMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.String,
            static message => message.Token,
            Codecs.Int64,
            static message => message.ExpiresAtUnixTimeSeconds,
            static (token, expiresAtUnixTimeSeconds) => new ScidJwtMessage(token, expiresAtUnixTimeSeconds)),
        trailingDataMessage: "The Supercell ID JWT message has trailing data.");

    /// <summary>Omits the credential from logs.</summary>
    /// <returns>
    /// The type name with only <see cref="ExpiresAtUnixTimeSeconds"/>, such as <c>ScidJwtMessage { ExpiresAtUnixTimeSeconds = 1700000000 }</c>;
    /// <see cref="Token"/> is never included.
    /// </returns>
    public override string ToString()
    {
        return $"{nameof(ScidJwtMessage)} {{ ExpiresAtUnixTimeSeconds = {ExpiresAtUnixTimeSeconds} }}";
    }
}
