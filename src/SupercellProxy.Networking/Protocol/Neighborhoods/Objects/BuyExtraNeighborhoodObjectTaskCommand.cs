using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Objects;

/// <summary>Buys an extra neighborhood object task with diamonds.</summary>
public sealed record BuyExtraNeighborhoodObjectTaskCommand : CodecCommand<BuyExtraNeighborhoodObjectTaskCommand>, ICommandCodecProvider<BuyExtraNeighborhoodObjectTaskCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyExtraNeighborhoodObjectTaskCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new BuyExtraNeighborhoodObjectTaskCommand()));
}
