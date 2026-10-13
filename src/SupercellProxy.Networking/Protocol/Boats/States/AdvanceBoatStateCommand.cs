using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.States;

/// <summary>Advances the boat to its next state.</summary>
public sealed record AdvanceBoatStateCommand : CodecCommand<AdvanceBoatStateCommand>, ICommandCodecProvider<AdvanceBoatStateCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AdvanceBoatStateCommand> Codec { get; } = Codecs.Command(type: 663, Codecs.Unit(static () => new AdvanceBoatStateCommand()));
}
