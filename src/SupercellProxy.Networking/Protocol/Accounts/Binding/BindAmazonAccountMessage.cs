using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Binding;

/// <summary>Binds an Amazon account.</summary>
/// <param name="UserId">Amazon user id to link to this farm.</param>
/// <param name="AccessToken">Amazon login token that proves the player owns that user id.</param>
/// <param name="Force">Whether to bind even when the Amazon account already belongs to another farm.</param>
public sealed record BindAmazonAccountMessage(string? UserId, string? AccessToken, bool Force) : CodecMessage<BindAmazonAccountMessage>, ICodecProvider<BindAmazonAccountMessage>
{
    /// <inheritdoc />
    public static ICodec<BindAmazonAccountMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.UserId,
        Codecs.OptionalString,
        static message => message.AccessToken,
        Codecs.Boolean,
        static message => message.Force,
        static (userId, accessToken, force) => new BindAmazonAccountMessage(userId, accessToken, force)
    );
}
