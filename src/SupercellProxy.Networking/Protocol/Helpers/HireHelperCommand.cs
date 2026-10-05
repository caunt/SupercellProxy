using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Helpers;

/// Hires one farm helper at a configured duration tier.
public sealed record HireHelperCommand(int HelperIndex, int HireTierIndex) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.HireHelperCommandType;

    /// Decodes the helper and tier before the shared command fields.
    public static HireHelperCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int helperIndex = stream.ReadVarInt();
        int tierIndex = stream.ReadVarInt();

        return new HireHelperCommand(helperIndex, tierIndex);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(HelperIndex);
        stream.WriteVarInt(HireTierIndex);
    }
}
