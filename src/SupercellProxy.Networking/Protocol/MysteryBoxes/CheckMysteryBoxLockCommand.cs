using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MysteryBoxes;

/// <summary>
/// Defines the Check Mystery Box Lock Command contract.
/// </summary>
/// <summary>
/// Defines the Box Global Id contract.
/// </summary>
public sealed record CheckMysteryBoxLockCommand([property: System.Text.Json.Serialization.JsonPropertyName("BoxGlobalId")] int BoxGlobalId) : CodecCommand<CheckMysteryBoxLockCommand>, ICommandCodecProvider<CheckMysteryBoxLockCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CheckMysteryBoxLockCommand> Codec { get; } = Codecs.Command(
        type: 46,
        Codecs.Composite(Codecs.VarInt, static command => command.BoxGlobalId, static (box) => new CheckMysteryBoxLockCommand(box))
    );
}
