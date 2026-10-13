using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Marks an exhausted plant for clearing, optionally buying its missing tool with coins.</summary>
public sealed record StartPlantClearingCommand(int PlantId, bool BuyMissingTool) : CodecCommand<StartPlantClearingCommand>, ICommandCodecProvider<StartPlantClearingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartPlantClearingCommand> Codec { get; } = Codecs.Command(
        type: 60,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.PlantId,
            Codecs.Boolean,
            static command => command.BuyMissingTool,
            static (id, buy) => new StartPlantClearingCommand(id, buy)
        )
    );
}
