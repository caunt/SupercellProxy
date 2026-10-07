using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Requests automatic neighborhood creation through the player's home.</summary>
public sealed record RequestAutomaticNeighborhoodCreationCommand(string Name, int Unknown0, int Unknown1, int Unknown2, int Unknown3) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.RequestAutomaticNeighborhoodCreationCommandType;

    /// <summary>Decodes the creation arguments before the command metadata.</summary>
    public static RequestAutomaticNeighborhoodCreationCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadString(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteString(Name);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteVarInt(Unknown3);
    }
}
