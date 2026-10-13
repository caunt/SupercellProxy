using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing.Ducks;

/// <summary>Finishes a duck immediately for diamonds.</summary>
/// <param name="DuckGlobalId">The duck game object id.</param>
public sealed record SpeedUpDuckCommand(int DuckGlobalId) : CodecCommand<SpeedUpDuckCommand>, ICommandCodecProvider<SpeedUpDuckCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpDuckCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.DuckGlobalId, static (duckGlobalId) => new SpeedUpDuckCommand(duckGlobalId))
    );
}
