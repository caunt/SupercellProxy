using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Requests public, neighbourhood, or Farm Pass help for a boat crate.</summary>
public sealed record RequestBoatCrateHelpCommand(int CrateIndex, int RequestKind) : Command
{
    /// <summary>Gets the registered command identifier.</summary>
    public override int Type => CommandRegistry.RequestBoatCrateHelpCommandType;

    /// <summary>Reads the crate and request kind before the base command metadata.</summary>
    public static RequestBoatCrateHelpCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new RequestBoatCrateHelpCommand(stream.ReadUInt16(), stream.ReadUInt16());
    }

    /// <summary>Writes the native unsigned-short fields.</summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteUInt16(checked((ushort)CrateIndex));
        stream.WriteUInt16(checked((ushort)RequestKind));
    }
}
