using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Inventory;

/// <summary>Exchanges diamonds for a coin package.</summary>
/// <param name="PackageGlobalId">The coin package data id.</param>
public sealed record BuyCashPackageCommand(int PackageGlobalId) : CodecCommand<BuyCashPackageCommand>, ICommandCodecProvider<BuyCashPackageCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyCashPackageCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.PackageGlobalId, static packageGlobalId => new BuyCashPackageCommand(packageGlobalId))
    );
}
