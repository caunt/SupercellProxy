using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.SpeedUps;

/// <summary>Finishes a construction site immediately for diamonds.</summary>
/// <param name="ConstructionGlobalId">The construction building game object id.</param>
public sealed record SpeedUpConstructionCommand(int ConstructionGlobalId) : CodecCommand<SpeedUpConstructionCommand>, ICommandCodecProvider<SpeedUpConstructionCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpConstructionCommand> Codec { get; } = Codecs.Command(
        type: 16,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ConstructionGlobalId,
            static (constructionGlobalId) => new SpeedUpConstructionCommand(constructionGlobalId)
        )
    );
}
