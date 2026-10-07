using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Clears the active task from the player's farm-side derby manager.</summary>
public sealed record ClearActiveDerbyTaskCommand : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ClearActiveDerbyTaskCommandType;
    /// <summary>Decodes a command with no additional payload fields.</summary>
    public static ClearActiveDerbyTaskCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        return new();
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment) { }
}
