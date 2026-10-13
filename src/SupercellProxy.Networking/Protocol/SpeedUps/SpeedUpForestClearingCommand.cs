using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.SpeedUps;

/// <summary>Finishes clearing a forest obstacle immediately for diamonds.</summary>
/// <param name="ForestGlobalId">The forest game object id.</param>
public sealed record SpeedUpForestClearingCommand(int ForestGlobalId) : CodecCommand<SpeedUpForestClearingCommand>, ICommandCodecProvider<SpeedUpForestClearingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpForestClearingCommand> Codec { get; } = Codecs.Command(
        type: 19,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ForestGlobalId,
            static (forestGlobalId) => new SpeedUpForestClearingCommand(forestGlobalId)
        )
    );
}
