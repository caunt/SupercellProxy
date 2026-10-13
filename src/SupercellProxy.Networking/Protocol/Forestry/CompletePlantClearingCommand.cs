using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Collects an already cut plant's rewards and removes the plant.</summary>
public sealed record CompletePlantClearingCommand(int PlantId) : CodecCommand<CompletePlantClearingCommand>, ICommandCodecProvider<CompletePlantClearingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CompletePlantClearingCommand> Codec { get; } = Codecs.Command(
        type: 61,
        Codecs.Composite(Codecs.VarInt, static command => command.PlantId, static (id) => new CompletePlantClearingCommand(id))
    );
}
