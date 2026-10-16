using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Serverbound command raising a building one rank, such as the barn, the silo, a fishing-area building or a town service
/// building; the upgrade materials are spent.
/// </summary>
/// <param name="BuildingGlobalId">Global id of the building's game object.</param>
/// <param name="UpgradeOption">
/// 0 for a plain upgrade; for a town service building 1 adds a service slot, 2 a coin bonus, 3 an experience and reputation
/// bonus, and 4 a time bonus.
/// </param>
public sealed record UpgradeBuildingCommand([property: JsonPropertyName("BuildingGlobalId")] int BuildingGlobalId, int UpgradeOption) : CodecCommand<UpgradeBuildingCommand>, ICommandCodecProvider<UpgradeBuildingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<UpgradeBuildingCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BuildingGlobalId,
            Codecs.VarInt,
            static command => command.UpgradeOption,
            static (building, option) => new UpgradeBuildingCommand(building, option)
        )
    );
}
