using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>
/// Serverbound command that finishes clearing a tree or stump whose clearing was started, removing it from the farm.
/// </summary>
/// <param name="ForestGlobalId">Game object id of the tree or stump being cleared.</param>
public sealed record CompleteForestClearingCommand([property: JsonPropertyName("ForestGlobalId")] int ForestGlobalId) : CodecCommand<CompleteForestClearingCommand>, ICommandCodecProvider<CompleteForestClearingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CompleteForestClearingCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.ForestGlobalId, static forest => new CompleteForestClearingCommand(forest))
    );
}
