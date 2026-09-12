using SupercellProxy.Networking.Protocol.CommandEncoding.FieldSchemas;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding;

/// <summary>
/// <para>A command with a native-proven primitive field sequence and unknown semantic field names.</para>
/// </summary>
public sealed record CommandWithFields : Command
{
    private readonly bool _baseFirst;

    /// <summary>
    /// Initializes a new <see cref="CommandWithFields"/> instance.
    /// </summary>
    public CommandWithFields(
        int type,
        ReadOnlyMemory<CommandField> fields,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        _baseFirst = CommandRegistry.ValidateFields(type, fields.Span, isServerCommand: false);
        Type = type;
        Fields = fields.ToArray();
    }

    private CommandWithFields(
        int type,
        ReadOnlyMemory<CommandField> fields,
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields,
        bool baseFirst
    )
        : base(commandFields.ExecutionPhaseCounter, commandFields.DebugData0, commandFields.DebugData1)
    {
        _baseFirst = baseFirst;
        Type = type;
        Fields = fields;
    }

    /// <summary>
    /// Gets the <c language="csharp">Fields</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandField> Fields { get; }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandWithFields Decode(int type, ReadOnlySpan<CommandFieldSchema> fieldSchemas, bool baseFirst, MessageStream stream, CommandEnvironment environment)
    {
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields = default;

        if (baseFirst)
            commandFields = DecodeCommand(stream, environment);

        CommandField[]? fields = CommandFieldSchema.DecodeFields(fieldSchemas, stream);

        if (!baseFirst)
            commandFields = DecodeCommand(stream, environment);

        return new CommandWithFields(type, fields, commandFields, baseFirst);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        if (_baseFirst)
            EncodeCommand(stream, environment);

        foreach (CommandField field in Fields.Span)
            field.Encode(stream);

        if (!_baseFirst)
            EncodeCommand(stream, environment);
    }
}
