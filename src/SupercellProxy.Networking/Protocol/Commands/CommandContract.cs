using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>A command type's identity and wire layout, independent of its record type.</summary>
[JsonConverter(typeof(CommandContractJsonConverter))]
public abstract class CommandContract
{
    private protected CommandContract(string name, CommandLayout layout, bool productionAllowed)
    {
        Name = name;
        Layout = layout;
        ProductionAllowed = productionAllowed;
    }

    /// <summary>Gets the record type the contract reads and writes.</summary>
    public abstract Type CommandType { get; }

    /// <summary>Gets the direction of the command: clientbound for server commands, serverbound otherwise.</summary>
    public abstract MessageDirection Direction { get; }

    /// <summary>Gets where the shared metadata sits relative to the command's fields.</summary>
    public CommandLayout Layout { get; }

    /// <summary>Gets the contract's name: its record's name, or the native command's name for a record that carries several.</summary>
    public string Name { get; }

    /// <summary>Gets a value indicating whether native accepts the command in the production environment.</summary>
    public bool ProductionAllowed { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return Name;
    }

    internal abstract Command ReadCommand(MessageStream stream);

    internal abstract void WriteCommand(ProtocolEncoder encoder, Command command);
}
