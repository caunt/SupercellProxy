using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Marks an exhausted plant for clearing, optionally buying its missing tool with coins.</summary>
/// <param name="PlantId">Game object global id of the exhausted fruit tree, bush or gatherer source on the player's own farm.</param>
/// <param name="BuyMissingTool"><see langword="true"/> to buy the plant's clearing tool with coins when the player has none; <see langword="false"/> to clear only with a tool already owned. Written as a Boolean after the plant id.</param>
public sealed record StartPlantClearingCommand(int PlantId, bool BuyMissingTool) : CodecCommand<StartPlantClearingCommand>, ICommandCodecProvider<StartPlantClearingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartPlantClearingCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.PlantId,
        Codecs.Boolean,
        static command => command.BuyMissingTool,
        static (id, buy) => new StartPlantClearingCommand(id, buy)));
}
