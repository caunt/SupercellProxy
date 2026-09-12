using SupercellProxy.Networking.Protocol.CommandEncoding.FieldSchemas;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding;

/// <summary>
/// <para>A server command with a native-proven primitive field sequence and unknown semantic field names.</para>
/// </summary>
public sealed record ServerCommandWithFields : ServerCommand
{
    private readonly bool _baseFirst;

    /// <summary>
    /// Initializes a new <see cref="ServerCommandWithFields"/> instance.
    /// </summary>
    public ServerCommandWithFields(
        int type,
        ReadOnlyMemory<CommandField> fields,
        int serverCommandIdentifier,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(serverCommandIdentifier, executionPhaseCounter, debugData0, debugData1)
    {
        _baseFirst = CommandRegistry.ValidateFields(type, fields.Span, isServerCommand: true);
        Type = type;
        Fields = fields.ToArray();
    }

    private ServerCommandWithFields(
        int type,
        ReadOnlyMemory<CommandField> fields,
        (
            int ServerCommandId,
            (
                int ExecutionPhaseCounter,
                CommandData? DebugData0,
                CommandData? DebugData1
            ) CommandFields
        ) commandFields,
        bool baseFirst
    )
        : base(
            commandFields.ServerCommandId,
            commandFields.CommandFields.ExecutionPhaseCounter,
            commandFields.CommandFields.DebugData0,
            commandFields.CommandFields.DebugData1
        )
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
    public static ServerCommandWithFields Decode(int type, ReadOnlySpan<CommandFieldSchema> fieldSchemas, bool baseFirst, MessageStream stream, CommandEnvironment environment)
    {
        (int ServerCommandId, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) CommandFields) commandFields = default;

        if (baseFirst)
            commandFields = DecodeServerCommand(stream, environment);

        CommandField[]? fields = CommandFieldSchema.DecodeFields(fieldSchemas, stream);

        if (!baseFirst)
            commandFields = DecodeServerCommand(stream, environment);

        return new ServerCommandWithFields(type, fields, commandFields, baseFirst);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        if (_baseFirst)
            EncodeServerCommand(stream, environment);

        foreach (CommandField field in Fields.Span)
            field.Encode(stream);

        if (!_baseFirst)
            EncodeServerCommand(stream, environment);
    }
}
