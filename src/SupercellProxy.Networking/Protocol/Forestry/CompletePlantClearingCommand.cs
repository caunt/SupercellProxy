using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Collects an already cut plant's rewards and removes the plant.</summary>
/// <param name="PlantId">Game object global id of the fruit tree, bush or gatherer source whose clearing was started, written as a VarInt.</param>
public sealed record CompletePlantClearingCommand(int PlantId) : CodecCommand<CompletePlantClearingCommand>, ICommandCodecProvider<CompletePlantClearingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CompletePlantClearingCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.PlantId, static id => new CompletePlantClearingCommand(id)));
}
