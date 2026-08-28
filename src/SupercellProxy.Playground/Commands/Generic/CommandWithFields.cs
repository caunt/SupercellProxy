using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>A command with a native-proven primitive field sequence and unknown semantic field names.</para>
/// </summary>
internal sealed record CommandWithFields : Command
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
        : base(
            commandFields.ExecutionPhaseCounter,
            commandFields.DebugData0,
            commandFields.DebugData1
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

    internal static CommandWithFields Decode(
        int type,
        ReadOnlySpan<CommandFieldSchema> fieldSchemas,
        bool baseFirst,
        MessageStream stream,
        CommandEnvironment environment
    )
    {
        var commandFields = default((
            int ExecutionPhaseCounter,
            CommandData? DebugData0,
            CommandData? DebugData1
        ));
        var fields = Array.Empty<CommandField>();

        if (baseFirst)
            commandFields = DecodeCommand(stream, environment);

        fields = CommandFieldSchema.DecodeFields(fieldSchemas, stream);

        if (!baseFirst)
            commandFields = DecodeCommand(stream, environment);

        return new CommandWithFields(type, fields, commandFields, baseFirst);
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        if (_baseFirst)
            EncodeCommand(stream, environment);

        foreach (var field in Fields.Span)
            field.Encode(stream);

        if (!_baseFirst)
            EncodeCommand(stream, environment);
    }
}
