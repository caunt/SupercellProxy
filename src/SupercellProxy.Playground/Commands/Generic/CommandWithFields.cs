using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

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
        int executeSubTick = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executeSubTick, debugData0, debugData1)
    {
        _baseFirst = CommandRegistry.ValidateFields(type, fields.Span, isServerCommand: false);
        Type = type;
        Fields = fields.ToArray();
    }

    private CommandWithFields(
        int type,
        ReadOnlyMemory<CommandField> fields,
        (int ExecuteSubTick, CommandData? DebugData0, CommandData? DebugData1) commandFields,
        bool baseFirst
    )
        : base(commandFields.ExecuteSubTick, commandFields.DebugData0, commandFields.DebugData1)
    {
        _baseFirst = baseFirst;
        Type = type;
        Fields = fields;
    }

    /// <summary>
    /// Gets the <c>Type</c> value.
    /// </summary>
    public override int Type { get; }

    /// <summary>
    /// Gets the <c>Fields</c> value.
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
            int ExecuteSubTick,
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
