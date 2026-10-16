using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Binding;

/// <summary>Binds a Google Play Games account.</summary>
/// <param name="PlayerId">Google Play Games player id to link to this farm.</param>
/// <param name="AuthorizationCode">One-time server authorization code from Google sign-in, which the server exchanges with Google.</param>
/// <param name="Force">Whether to bind even when the Google Play Games account already belongs to another farm.</param>
public sealed record BindGoogleServiceAccountMessage(string? PlayerId, string? AuthorizationCode, bool Force) : CodecMessage<BindGoogleServiceAccountMessage>, ICodecProvider<BindGoogleServiceAccountMessage>
{
    /// <inheritdoc />
    public static ICodec<BindGoogleServiceAccountMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.PlayerId,
        Codecs.OptionalString,
        static message => message.AuthorizationCode,
        Codecs.Boolean,
        static message => message.Force,
        static (playerId, authorizationCode, force) => new BindGoogleServiceAccountMessage(playerId, authorizationCode, force)
    );
}
