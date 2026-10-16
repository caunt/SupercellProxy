using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>
/// Serverbound command that opens a gift waiting on the farm and adds its contents to the player's storage.
/// </summary>
/// <param name="GiftGlobalId">Game object id of the gift to open.</param>
public sealed record CollectGiftCommand([property: JsonPropertyName("GiftGlobalId")] int GiftGlobalId) : CodecCommand<CollectGiftCommand>, ICommandCodecProvider<CollectGiftCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectGiftCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.GiftGlobalId, static gift => new CollectGiftCommand(gift)));
}
