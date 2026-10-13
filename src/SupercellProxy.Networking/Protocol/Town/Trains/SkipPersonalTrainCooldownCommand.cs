using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Trains;

/// <summary>Ends the personal train cooldown, paying diamonds in the own town and the regular speed-up price elsewhere.</summary>
public sealed record SkipPersonalTrainCooldownCommand : CodecCommand<SkipPersonalTrainCooldownCommand>, ICommandCodecProvider<SkipPersonalTrainCooldownCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SkipPersonalTrainCooldownCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new SkipPersonalTrainCooldownCommand()));
}
