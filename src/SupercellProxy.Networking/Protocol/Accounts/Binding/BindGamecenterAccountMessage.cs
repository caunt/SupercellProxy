using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Binding;

/// <summary>Binds a Game Center account.</summary>
/// <param name="PlayerId">The player id.</param>
/// <param name="BundleId">The bundle id.</param>
/// <param name="PublicKeyLink">The public key link.</param>
/// <param name="Signature">The signature.</param>
/// <param name="Salt">The salt.</param>
/// <param name="Timestamp">The timestamp.</param>
/// <param name="Force">The force.</param>
public sealed record BindGamecenterAccountMessage(
    string? PlayerId,
    string? BundleId,
    string? PublicKeyLink,
    ReadOnlyMemory<byte>? Signature,
    ReadOnlyMemory<byte>? Salt,
    ReadOnlyMemory<byte>? Timestamp,
    bool Force
) : CodecMessage<BindGamecenterAccountMessage>, ICodecProvider<BindGamecenterAccountMessage>
{
    /// <inheritdoc />
    public static ICodec<BindGamecenterAccountMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.PlayerId,
        Codecs.OptionalString,
        static message => message.BundleId,
        Codecs.OptionalString,
        static message => message.PublicKeyLink,
        Codecs.OptionalByteArray,
        static message => message.Signature,
        Codecs.OptionalByteArray,
        static message => message.Salt,
        Codecs.OptionalByteArray,
        static message => message.Timestamp,
        Codecs.Boolean,
        static message => message.Force,
        static (playerId, bundleId, publicKeyLink, signature, salt, timestamp, force) => new BindGamecenterAccountMessage(playerId, bundleId, publicKeyLink, signature, salt, timestamp, force)
    );
}
