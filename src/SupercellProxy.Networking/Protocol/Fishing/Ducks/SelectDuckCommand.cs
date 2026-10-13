using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing.Ducks;

/// <summary>Makes a duck the active one of its salon.</summary>
/// <param name="SalonGlobalId">The duck salon game object id.</param>
/// <param name="DuckGlobalId">The duck game object id.</param>
public sealed record SelectDuckCommand(int SalonGlobalId, int DuckGlobalId) : CodecCommand<SelectDuckCommand>, ICommandCodecProvider<SelectDuckCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SelectDuckCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.SalonGlobalId,
            Codecs.VarInt,
            static command => command.DuckGlobalId,
            static (salonGlobalId, duckGlobalId) => new SelectDuckCommand(salonGlobalId, duckGlobalId)
        )
    );
}
