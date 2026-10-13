using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Enters the farm edit mode when it can be activated.</summary>
public sealed record EnterEditModeCommand : CodecCommand<EnterEditModeCommand>, ICommandCodecProvider<EnterEditModeCommand>
{
    /// <inheritdoc />
    public static CommandCodec<EnterEditModeCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new EnterEditModeCommand()));
}
