using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>
/// Defines the Start Forest Clearing Command contract.
/// </summary>
/// <summary>
/// Defines the Forest Global Id contract.
/// </summary>
/// <summary>
/// Defines the Buy Missing Tool contract.
/// </summary>
public sealed record StartForestClearingCommand([property: System.Text.Json.Serialization.JsonPropertyName("ForestGlobalId")] int ForestGlobalId, bool BuyMissingTool) : CodecCommand<StartForestClearingCommand>, ICommandCodecProvider<StartForestClearingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartForestClearingCommand> Codec { get; } = Codecs.Command(
        type: 18,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ForestGlobalId,
            Codecs.Boolean,
            static command => command.BuyMissingTool,
            static (forest, buyTool) => new StartForestClearingCommand(forest, buyTool)
        )
    );
}
