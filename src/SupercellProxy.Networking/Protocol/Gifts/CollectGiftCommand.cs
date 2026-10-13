using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>
/// Defines the Collect Gift Command contract.
/// </summary>
/// <summary>
/// Defines the Gift Global Id contract.
/// </summary>
public sealed record CollectGiftCommand([property: System.Text.Json.Serialization.JsonPropertyName("GiftGlobalId")] int GiftGlobalId) : CodecCommand<CollectGiftCommand>, ICommandCodecProvider<CollectGiftCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectGiftCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.GiftGlobalId, static (gift) => new CollectGiftCommand(gift)));
}
