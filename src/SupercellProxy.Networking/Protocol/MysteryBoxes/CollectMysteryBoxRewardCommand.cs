using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MysteryBoxes;

/// <summary>
/// Serverbound command that takes the reward out of an opened mystery box.
/// </summary>
/// <param name="BoxGlobalId">Game object id of the opened mystery box.</param>
public sealed record CollectMysteryBoxRewardCommand([property: JsonPropertyName("BoxGlobalId")] int BoxGlobalId) : CodecCommand<CollectMysteryBoxRewardCommand>, ICommandCodecProvider<CollectMysteryBoxRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectMysteryBoxRewardCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.BoxGlobalId, static box => new CollectMysteryBoxRewardCommand(box)));
}
