using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Characters;

/// <summary>Changes whether Angus is interacting with the player.</summary>
public sealed record SetAngusInteractionCommand(bool Active, int AngusGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.SetAngusInteractionCommandType;

    internal static Version SinceVersion { get; } = new(major: 1, minor: 73, build: 81);

    /// <summary>Decodes the interaction flag and Angus instance.</summary>
    public static SetAngusInteractionCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new SetAngusInteractionCommand(stream.ReadBoolean(), stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteBoolean(Active);
        stream.WriteVarInt(AngusGlobalId);
    }
}
