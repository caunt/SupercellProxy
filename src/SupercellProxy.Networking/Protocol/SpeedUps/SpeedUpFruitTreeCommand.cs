using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.SpeedUps;

/// <summary>Finishes a fruit tree or gatherer mine immediately for diamonds.</summary>
/// <param name="TreeGlobalId">The fruit tree or gatherer mine game object id.</param>
public sealed record SpeedUpFruitTreeCommand(int TreeGlobalId) : CodecCommand<SpeedUpFruitTreeCommand>, ICommandCodecProvider<SpeedUpFruitTreeCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpFruitTreeCommand> Codec { get; } = Codecs.Command(
        type: 59,
        Codecs.Composite(Codecs.VarInt, static command => command.TreeGlobalId, static (treeGlobalId) => new SpeedUpFruitTreeCommand(treeGlobalId))
    );
}
