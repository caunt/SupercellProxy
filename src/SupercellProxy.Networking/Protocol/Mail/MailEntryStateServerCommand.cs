using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Mail;

/// <summary>Notifies a home that a helper acted on one of its game objects; native adds a letter from that helper.</summary>
/// <param name="GameObjectId">The game object id.</param>
/// <param name="HomeOwnerId">The home owner id.</param>
/// <param name="HelperId">The helper id.</param>
public sealed record MailEntryStateServerCommand(int GameObjectId, LongId HomeOwnerId, LongId HelperId) : CodecServerCommand<MailEntryStateServerCommand>, ICommandCodecProvider<MailEntryStateServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MailEntryStateServerCommand> Codec { get; } = Codecs.Command(
        type: 207,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.GameObjectId,
            Codecs.LongId,
            static command => command.HomeOwnerId,
            Codecs.LongId,
            static command => command.HelperId,
            static (gameObjectId, homeOwnerId, helperId) => new MailEntryStateServerCommand(gameObjectId, homeOwnerId, helperId)
        )
    );
}
