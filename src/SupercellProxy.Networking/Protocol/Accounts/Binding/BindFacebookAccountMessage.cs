using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Binding;

/// <summary>Binds a Facebook account.</summary>
/// <param name="FacebookId">The facebook id.</param>
/// <param name="AuthenticationToken">The authentication token.</param>
/// <param name="Force">The force.</param>
public sealed record BindFacebookAccountMessage(string? FacebookId, string? AuthenticationToken, bool Force) : CodecMessage<BindFacebookAccountMessage>, ICodecProvider<BindFacebookAccountMessage>
{
    /// <inheritdoc />
    public static ICodec<BindFacebookAccountMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.FacebookId,
        Codecs.OptionalString,
        static message => message.AuthenticationToken,
        Codecs.Boolean,
        static message => message.Force,
        static (facebookId, authenticationToken, force) => new BindFacebookAccountMessage(facebookId, authenticationToken, force)
    );
}
