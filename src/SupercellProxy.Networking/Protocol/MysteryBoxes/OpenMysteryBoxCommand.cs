using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MysteryBoxes;

/// <summary>
/// Defines the Open Mystery Box Command contract.
/// </summary>
/// <summary>
/// Defines the Box Global Id contract.
/// </summary>
/// <summary>
/// Defines the Legacy Flag contract.
/// </summary>
public sealed record OpenMysteryBoxCommand([property: System.Text.Json.Serialization.JsonPropertyName("BoxGlobalId")] int BoxGlobalId, bool LegacyFlag) : CodecCommand<OpenMysteryBoxCommand>, ICommandCodecProvider<OpenMysteryBoxCommand>
{
    /// <inheritdoc />
    public static CommandCodec<OpenMysteryBoxCommand> Codec { get; } = Codecs.Command(
        type: 44,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BoxGlobalId,
            Codecs.Boolean,
            static command => command.LegacyFlag,
            static (box, flag) => new OpenMysteryBoxCommand(box, flag)
        )
    );
}
