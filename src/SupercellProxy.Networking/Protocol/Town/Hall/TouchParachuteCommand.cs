using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Hall;

/// <summary>Touches a parachute in the player's own town; native only validates the town and the parachute lookup.</summary>
/// <param name="ParachuteGlobalId">Gets the parachute game object id.</param>
/// <param name="Value">Gets the second value carried to the server.</param>
public sealed record TouchParachuteCommand(int ParachuteGlobalId, int Value) : CodecCommand<TouchParachuteCommand>, ICommandCodecProvider<TouchParachuteCommand>
{
    /// <inheritdoc />
    public static CommandCodec<TouchParachuteCommand> Codec { get; } = Codecs.Command(
        type: 223,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ParachuteGlobalId,
            Codecs.VarInt,
            static command => command.Value,
            static (parachuteGlobalId, @value) => new TouchParachuteCommand(parachuteGlobalId, value)
        )
    );
}
