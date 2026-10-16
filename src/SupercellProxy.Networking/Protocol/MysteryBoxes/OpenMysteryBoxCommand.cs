using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MysteryBoxes;

/// <summary>
/// Serverbound command that opens a mystery box found on the farm, revealing its reward.
/// </summary>
/// <param name="BoxGlobalId">Game object id of the mystery box.</param>
/// <param name="LegacyFlag">Flag sent after the box id; its meaning is not identified.</param>
public sealed record OpenMysteryBoxCommand([property: JsonPropertyName("BoxGlobalId")] int BoxGlobalId, bool LegacyFlag) : CodecCommand<OpenMysteryBoxCommand>, ICommandCodecProvider<OpenMysteryBoxCommand>
{
    /// <inheritdoc />
    public static CommandCodec<OpenMysteryBoxCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BoxGlobalId,
            Codecs.Boolean,
            static command => command.LegacyFlag,
            static (box, flag) => new OpenMysteryBoxCommand(box, flag)
        )
    );
}
