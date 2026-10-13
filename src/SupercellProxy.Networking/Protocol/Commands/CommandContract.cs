using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>A command type's identity and wire layout, independent of its record type.</summary>
public abstract class CommandContract
{
    private protected CommandContract(int type, CommandLayout layout, bool productionAllowed)
    {
        Type = type;
        Layout = layout;
        ProductionAllowed = productionAllowed;
    }

    /// <summary>Gets the record type the contract reads and writes.</summary>
    public abstract Type CommandType { get; }

    /// <summary>Gets the direction of the command: clientbound for server commands, serverbound otherwise.</summary>
    public abstract MessageDirection Direction { get; }

    /// <summary>Gets where the shared metadata sits relative to the command's fields.</summary>
    public CommandLayout Layout { get; }

    /// <summary>Gets a value indicating whether native accepts the command in the production environment.</summary>
    public bool ProductionAllowed { get; }

    /// <summary>Gets the internal contract id; <see cref="CommandRegistry"/> maps it to each release's wire id.</summary>
    public int Type { get; }

    internal abstract Command ReadCommand(MessageStream stream);

    internal abstract void WriteCommand(ProtocolEncoder encoder, Command command);
}
