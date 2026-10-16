using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts;

/// <summary>Confirms an account action from a confirmation popup.</summary>
/// <param name="AvatarId">Account the confirmation popup was shown for; null when it named none.</param>
/// <param name="Action">Name of the action the player confirmed, as the popup received it from the server.</param>
public sealed record ConfirmAccountActionMessage(LongId? AvatarId, string? Action) : CodecMessage<ConfirmAccountActionMessage>, ICodecProvider<ConfirmAccountActionMessage>
{
    /// <inheritdoc />
    public static ICodec<ConfirmAccountActionMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static message => message.AvatarId,
        Codecs.OptionalString,
        static message => message.Action,
        static (avatarId, action) => new ConfirmAccountActionMessage(avatarId, action)
    );
}
