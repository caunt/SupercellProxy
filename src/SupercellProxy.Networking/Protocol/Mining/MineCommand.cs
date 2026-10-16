using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Mining;

/// <summary>Consumes one mining tool and creates drops for collection from the mine.</summary>
public sealed record MineCommand(int ToolGlobalId) : CodecCommand<MineCommand>, ICommandCodecProvider<MineCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MineCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.ToolGlobalId, static tool => new MineCommand(tool)));
}
