using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Buildings.Upgrades;

/// <summary>Completes a building's pending upgrade and grants its experience.</summary>
/// <param name="ObjectGlobalId">The building object id.</param>
/// <param name="Level">The level the building reaches.</param>
public sealed record CompleteBuildingUpgradeCommand(int ObjectGlobalId, int Level) : CodecCommand<CompleteBuildingUpgradeCommand>, ICommandCodecProvider<CompleteBuildingUpgradeCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CompleteBuildingUpgradeCommand> Codec { get; } = Codecs.Command(
        type: 126,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ObjectGlobalId,
            Codecs.VarInt,
            static command => command.Level,
            static (objectGlobalId, level) => new CompleteBuildingUpgradeCommand(objectGlobalId, level)
        )
    );
}
