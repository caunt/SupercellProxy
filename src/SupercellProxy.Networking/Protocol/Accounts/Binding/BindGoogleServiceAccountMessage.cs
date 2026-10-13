using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Binding;

/// <summary>Binds a Google Play Games account.</summary>
/// <param name="PlayerId">The player id.</param>
/// <param name="AuthorizationCode">The authorization code.</param>
/// <param name="Force">The force.</param>
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
