using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Collects one fruit from a fruit tree or berry bush.</summary>
/// <param name="FruitIndex">Zero-based slot of the fruit on the source, written first as a VarInt.</param>
/// <param name="FruitTreeGlobalId">Game object global id of the fruit tree or berry bush on the player's own farm.</param>
public sealed record CollectFruitCommand(int FruitIndex, [property: JsonPropertyName("FruitTreeGlobalId")] int FruitTreeGlobalId) : CodecCommand<CollectFruitCommand>, ICommandCodecProvider<CollectFruitCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectFruitCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
            static command => command.FruitIndex,
            Codecs.VarInt,
            static command => command.FruitTreeGlobalId,
            static (fruitIndex, fruitTreeGlobalId) => new CollectFruitCommand(fruitIndex, fruitTreeGlobalId)),
        CommandLayout.MetadataFirst);
}
