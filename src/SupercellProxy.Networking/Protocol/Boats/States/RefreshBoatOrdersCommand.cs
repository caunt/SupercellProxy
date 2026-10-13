using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.States;

/// <summary>Pays diamonds to swap the boat orders for new ones.</summary>
public sealed record RefreshBoatOrdersCommand : CodecCommand<RefreshBoatOrdersCommand>, ICommandCodecProvider<RefreshBoatOrdersCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RefreshBoatOrdersCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new RefreshBoatOrdersCommand()));
}
