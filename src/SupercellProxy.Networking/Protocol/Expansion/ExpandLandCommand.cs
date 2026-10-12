using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Expansion;

/// <summary>Unlocks an expansion using its outstanding material requirements.</summary>
public sealed record ExpandLandCommand(int ExpansionDataGlobalId, int SpendReason) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ExpandLandCommandType;

    /// <summary>Decodes the expansion and the resource-spend reason.</summary>
    public static ExpandLandCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(ExpansionDataGlobalId);
        stream.WriteVarInt(SpendReason);
    }
}
