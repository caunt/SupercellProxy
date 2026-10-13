using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Seen;

/// <summary>Opens the Farm Pass building.</summary>
public sealed record OpenFarmPassBuildingCommand : CodecCommand<OpenFarmPassBuildingCommand>, ICommandCodecProvider<OpenFarmPassBuildingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<OpenFarmPassBuildingCommand> Codec { get; } = Codecs.Command(type: 562, Codecs.Unit(static () => new OpenFarmPassBuildingCommand()));
}
