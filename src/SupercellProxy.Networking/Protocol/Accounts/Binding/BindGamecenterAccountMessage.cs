using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Binding;

/// <summary>Binds a Game Center account.</summary>
/// <param name="PlayerId">Game Center player id to link to this farm.</param>
/// <param name="BundleId">The app's bundle identifier, part of the data Apple signed.</param>
/// <param name="PublicKeyLink">Address of Apple's certificate the server uses to check <paramref name="Signature"/>.</param>
/// <param name="Signature">Apple's identity-verification signature over the player id, bundle id, time stamp and salt.</param>
/// <param name="Salt">Random bytes Apple mixed into the signed data.</param>
/// <param name="Timestamp">Signing time as Apple encoded it, part of the signed data.</param>
/// <param name="Force">Whether to bind even when the Game Center account already belongs to another farm.</param>
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
