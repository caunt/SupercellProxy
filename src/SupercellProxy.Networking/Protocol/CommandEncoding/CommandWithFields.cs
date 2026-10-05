using SupercellProxy.Networking.Protocol.CommandEncoding.FieldSchemas;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CommandEncoding;

/// <summary>A command whose own fields follow a registered native schema.</summary>
public sealed record CommandWithFields : Command
{
    /// <summary>Creates a command from its type and payload fields.</summary>
    public CommandWithFields(int type, ReadOnlyMemory<CommandField> fields)
    {
        CommandRegistry.ValidateFields(type, fields.Span, MessageDirection.Serverbound);
        Type = type;
        Fields = fields.ToArray();
    }

    private CommandWithFields(int type, CommandField[] fields)
    {
        Type = type;
        Fields = fields;
    }

    /// <summary>Gets the command's payload fields.</summary>
    public ReadOnlyMemory<CommandField> Fields { get; }

    /// <inheritdoc />
    public override int Type { get; }

    /// <summary>Decodes the payload fields; the registry handles common metadata.</summary>
    public static CommandWithFields Decode(int type, ReadOnlySpan<CommandFieldSchema> fieldSchemas, MessageStream stream)
    {
        return new CommandWithFields(type, CommandFieldSchema.DecodeFields(fieldSchemas, stream));
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        foreach (CommandField field in Fields.Span)
            field.Encode(stream);
    }
}
