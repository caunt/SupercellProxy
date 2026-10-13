using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Collects one fruit from a fruit tree or berry bush.</summary>
public sealed record CollectFruitCommand(int FruitIndex, [property: System.Text.Json.Serialization.JsonPropertyName("FruitTreeGlobalId")] int FruitTreeGlobalId) : CodecCommand<CollectFruitCommand>, ICommandCodecProvider<CollectFruitCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectFruitCommand> Codec { get; } = Codecs.Command(
        type: 675,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.FruitIndex,
            Codecs.VarInt,
            static command => command.FruitTreeGlobalId,
            static (fruitIndex, fruitTreeGlobalId) => new CollectFruitCommand(fruitIndex, fruitTreeGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
