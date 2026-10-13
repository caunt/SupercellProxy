using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>Refreshes the shop category badges.</summary>
public sealed record RefreshShopBadgesCommand : CodecCommand<RefreshShopBadgesCommand>, ICommandCodecProvider<RefreshShopBadgesCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RefreshShopBadgesCommand> Codec { get; } = Codecs.Command(type: 681, Codecs.Unit(static () => new RefreshShopBadgesCommand()));
}
