using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Sets selected state flags on one Mini Pass instance.</summary>
public sealed record SetMiniPassStateFlagsCommand(int Flags, bool SecondaryPass) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.SetMiniPassStateFlagsCommandType;

    /// <summary>Decodes the flag mask and pass selector.</summary>
    public static SetMiniPassStateFlagsCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new SetMiniPassStateFlagsCommand(stream.ReadVarInt(), stream.ReadBoolean());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(Flags);
        stream.WriteBoolean(SecondaryPass);
    }
}
