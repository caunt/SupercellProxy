using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>
/// Defines the Complete Forest Clearing Command contract.
/// </summary>
/// <summary>
/// Defines the Forest Global Id contract.
/// </summary>
public sealed record CompleteForestClearingCommand([property: System.Text.Json.Serialization.JsonPropertyName("ForestGlobalId")] int ForestGlobalId) : CodecCommand<CompleteForestClearingCommand>, ICommandCodecProvider<CompleteForestClearingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CompleteForestClearingCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.ForestGlobalId, static (forest) => new CompleteForestClearingCommand(forest))
    );
}
