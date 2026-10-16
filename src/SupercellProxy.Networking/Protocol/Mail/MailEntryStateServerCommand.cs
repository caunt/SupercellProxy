using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Mail;

/// <summary>Notifies a home that a helper acted on one of its game objects; native adds a letter from that helper.</summary>
/// <param name="GameObjectId">The farm object the helper acted on.</param>
/// <param name="HomeOwnerId">Owner of the farm that was helped.</param>
/// <param name="HelperId">Player who helped; the letter comes from them.</param>
public sealed record MailEntryStateServerCommand(int GameObjectId, LongId HomeOwnerId, LongId HelperId) : CodecServerCommand<MailEntryStateServerCommand>, ICommandCodecProvider<MailEntryStateServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MailEntryStateServerCommand> Codec { get; } = Codecs.Command(
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
