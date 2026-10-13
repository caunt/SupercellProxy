using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.States;

/// <summary>Sends the boat with all of its crates filled.</summary>
public sealed record CompleteBoatOrderCommand : CodecCommand<CompleteBoatOrderCommand>, ICommandCodecProvider<CompleteBoatOrderCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CompleteBoatOrderCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new CompleteBoatOrderCommand()));
}
