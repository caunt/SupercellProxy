using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Defines the Upgrade Building Command contract.
/// </summary>
/// <summary>
/// Defines the Building Global Id contract.
/// </summary>
/// <summary>
/// Defines the Upgrade Option contract.
/// </summary>
public sealed record UpgradeBuildingCommand([property: System.Text.Json.Serialization.JsonPropertyName("BuildingGlobalId")] int BuildingGlobalId, int UpgradeOption) : CodecCommand<UpgradeBuildingCommand>, ICommandCodecProvider<UpgradeBuildingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<UpgradeBuildingCommand> Codec { get; } = Codecs.Command(
        type: 11,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BuildingGlobalId,
            Codecs.VarInt,
            static command => command.UpgradeOption,
            static (building, option) => new UpgradeBuildingCommand(building, option)
        )
    );
}
