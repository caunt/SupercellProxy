using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers;

/// <summary>Selects the helper house mode.</summary>
/// <param name="Mode">The helper house mode.</param>
public sealed record SetHelperHouseModeCommand(int Mode) : CodecCommand<SetHelperHouseModeCommand>, ICommandCodecProvider<SetHelperHouseModeCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetHelperHouseModeCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.Mode, static mode => new SetHelperHouseModeCommand(mode)));
}
