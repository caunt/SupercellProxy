using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MysteryBoxes;

/// <summary>
/// Defines the Collect Mystery Box Reward Command contract.
/// </summary>
/// <summary>
/// Defines the Box Global Id contract.
/// </summary>
public sealed record CollectMysteryBoxRewardCommand([property: System.Text.Json.Serialization.JsonPropertyName("BoxGlobalId")] int BoxGlobalId) : CodecCommand<CollectMysteryBoxRewardCommand>, ICommandCodecProvider<CollectMysteryBoxRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectMysteryBoxRewardCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.BoxGlobalId, static (box) => new CollectMysteryBoxRewardCommand(box)));
}
