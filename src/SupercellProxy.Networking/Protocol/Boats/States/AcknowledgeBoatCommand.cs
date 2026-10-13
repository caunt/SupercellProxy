using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.States;

/// <summary>Acknowledges the returned boat.</summary>
public sealed record AcknowledgeBoatCommand : CodecCommand<AcknowledgeBoatCommand>, ICommandCodecProvider<AcknowledgeBoatCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AcknowledgeBoatCommand> Codec { get; } = Codecs.Command(type: 674, Codecs.Unit(static () => new AcknowledgeBoatCommand()));
}
