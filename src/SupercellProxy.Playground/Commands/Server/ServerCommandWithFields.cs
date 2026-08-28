using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>A server command with a native-proven primitive field sequence and unknown semantic field names.</para>
/// </summary>
internal sealed record ServerCommandWithFields : ServerCommand
{
    private readonly bool _baseFirst;

    /// <summary>
    /// Initializes a new <see cref="ServerCommandWithFields"/> instance.
    /// </summary>
    public ServerCommandWithFields(
        int type,
        ReadOnlyMemory<CommandField> fields,
        int serverCommandId,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(serverCommandId, executionPhaseCounter, debugData0, debugData1)
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
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type { get; }

    /// <summary>
    /// Gets the <c language="csharp">Fields</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandField> Fields { get; }

    internal static ServerCommandWithFields Decode(
        int type,
        ReadOnlySpan<CommandFieldSchema> fieldSchemas,
        bool baseFirst,
        MessageStream stream,
        CommandEnvironment environment
    )
    {
        var commandFields = default((
            int ServerCommandId,
            (
                int ExecutionPhaseCounter,
                CommandData? DebugData0,
                CommandData? DebugData1
            ) CommandFields
        ));
        var fields = Array.Empty<CommandField>();

        if (baseFirst)
            commandFields = DecodeServerCommand(stream, environment);

        fields = CommandFieldSchema.DecodeFields(fieldSchemas, stream);

        if (!baseFirst)
            commandFields = DecodeServerCommand(stream, environment);

        return new ServerCommandWithFields(type, fields, commandFields, baseFirst);
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        if (_baseFirst)
            EncodeServerCommand(stream, environment);

        foreach (var field in Fields.Span)
            field.Encode(stream);

        if (!_baseFirst)
            EncodeServerCommand(stream, environment);
    }
}
