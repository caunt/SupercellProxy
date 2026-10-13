using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Animals;

/// <summary>Offloads and collects the animals the valley pawn carries at its current node.</summary>
public sealed record OffloadMapGameAnimalsCommand : CodecCommand<OffloadMapGameAnimalsCommand>, ICommandCodecProvider<OffloadMapGameAnimalsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<OffloadMapGameAnimalsCommand> Codec { get; } = Codecs.Command(type: 598, Codecs.Unit(static () => new OffloadMapGameAnimalsCommand()));
}
