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
    /// <value>The <see cref="Command"/>-derived record type; several contracts can share one type when a record carries more than one native command.</value>
    public abstract Type CommandType { get; }

    /// <summary>Gets the direction of the command: clientbound for server commands, serverbound otherwise.</summary>
    /// <value><see cref="MessageDirection.Clientbound"/> for <see cref="ServerCommand"/> records, whose metadata leads with the server command id; <see cref="MessageDirection.Serverbound"/> for every other command.</value>
    public abstract MessageDirection Direction { get; }

    /// <summary>Gets where the shared metadata sits relative to the command's fields.</summary>
    /// <value><see cref="CommandLayout.MetadataFirst"/> when the execution phase counter and debug data precede the command's fields; <see cref="CommandLayout.FieldsFirst"/> when they follow them.</value>
    public CommandLayout Layout { get; }

    /// <summary>Gets the contract's name: its record's name, or the native command's name for a record that carries several.</summary>
    /// <value>A native-style name such as <c>OwnBoatCrateHelpedServerCommand</c>; it is what <see cref="ToString"/> returns, what error messages show, and how the contract is written to JSON.</value>
    public string Name { get; }

    /// <summary>Gets a value indicating whether native accepts the command in the production environment.</summary>
    /// <value><see langword="true"/> if the command may be sent to a production server; otherwise, <see langword="false"/>.</value>
    /// <remarks>Debug-only commands have the value <see langword="false"/>, and the registry refuses to read or write them in the production environment.</remarks>
    public bool ProductionAllowed { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return Name;
    }

    internal abstract Command ReadCommand(MessageChannel stream);

    internal abstract void WriteCommand(ProtocolEncoder encoder, Command command);
}
