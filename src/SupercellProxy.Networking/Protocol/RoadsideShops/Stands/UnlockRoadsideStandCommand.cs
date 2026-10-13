using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops.Stands;

/// <summary>Buys the next roadside shop stand.</summary>
public sealed record UnlockRoadsideStandCommand : CodecCommand<UnlockRoadsideStandCommand>, ICommandCodecProvider<UnlockRoadsideStandCommand>
{
    /// <inheritdoc />
    public static CommandCodec<UnlockRoadsideStandCommand> Codec { get; } = Codecs.Command(type: 631, Codecs.Unit(static () => new UnlockRoadsideStandCommand()));
}
