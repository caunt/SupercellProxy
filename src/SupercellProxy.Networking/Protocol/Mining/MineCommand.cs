using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Mining;

/// <summary>Consumes one mining tool and creates drops for collection from the mine.</summary>
/// <param name="ToolGlobalId">
/// Global id of the mining tool row (such as dynamite or TNT) to use, written as a VarInt and the command's only field.
/// </param>
public sealed record MineCommand(int ToolGlobalId) : CodecCommand<MineCommand>, ICommandCodecProvider<MineCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MineCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.ToolGlobalId, static tool => new MineCommand(tool)));
}
