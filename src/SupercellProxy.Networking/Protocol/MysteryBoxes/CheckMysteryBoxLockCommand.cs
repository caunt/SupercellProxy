using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MysteryBoxes;

/// <summary>
/// Serverbound command asking whether a mystery box on the farm is still locked.
/// </summary>
/// <param name="BoxGlobalId">Game object id of the mystery box.</param>
public sealed record CheckMysteryBoxLockCommand([property: JsonPropertyName("BoxGlobalId")] int BoxGlobalId) : CodecCommand<CheckMysteryBoxLockCommand>, ICommandCodecProvider<CheckMysteryBoxLockCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CheckMysteryBoxLockCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.BoxGlobalId, static box => new CheckMysteryBoxLockCommand(box)));
}
