using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>
/// Serverbound command that starts clearing a tree or stump, spending the clearing tool (axe or saw) it needs.
/// </summary>
/// <param name="ForestGlobalId">Game object id of the tree or stump to clear.</param>
/// <param name="BuyMissingTool">Whether to buy the tool with coins when the player has none.</param>
public sealed record StartForestClearingCommand([property: JsonPropertyName("ForestGlobalId")] int ForestGlobalId, bool BuyMissingTool) : CodecCommand<StartForestClearingCommand>, ICommandCodecProvider<StartForestClearingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartForestClearingCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ForestGlobalId,
            Codecs.Boolean,
            static command => command.BuyMissingTool,
            static (forest, buyTool) => new StartForestClearingCommand(forest, buyTool)
        )
    );
}
