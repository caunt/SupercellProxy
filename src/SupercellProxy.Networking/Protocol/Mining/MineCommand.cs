using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Mining;

/// <summary>Consumes one mining tool and creates drops for collection from the mine.</summary>
public sealed record MineCommand(int ToolGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.MineCommandType;

    /// <summary>Decodes the tool id.</summary>
    public static MineCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int tool = stream.ReadVarInt();

        return new(tool);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(ToolGlobalId);
    }
}
