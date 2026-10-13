using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.States;

/// <summary>Discards the current boat order.</summary>
public sealed record DiscardBoatOrderCommand : CodecCommand<DiscardBoatOrderCommand>, ICommandCodecProvider<DiscardBoatOrderCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DiscardBoatOrderCommand> Codec { get; } = Codecs.Command(type: 533, Codecs.Unit(static () => new DiscardBoatOrderCommand()));
}
