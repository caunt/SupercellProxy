using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// A server command with a native-proven primitive field sequence and unknown semantic field names.
/// </summary>
public sealed record LogicServerCommandWithFields : LogicServerCommand
{
    private readonly bool _baseFirst;

    public LogicServerCommandWithFields(int type, ReadOnlyMemory<LogicCommandField> fields, int serverCommandId, int executeSubTick = -1, LogicCommandData? debugData0 = null, LogicCommandData? debugData1 = null)
        : base(serverCommandId, executeSubTick, debugData0, debugData1)
    {
        _baseFirst = LogicCommandRegistry.ValidateFields(type, fields.Span, isServerCommand: true);
        Type = type;
        Fields = fields.ToArray();
    }

    private LogicServerCommandWithFields(int type, ReadOnlyMemory<LogicCommandField> fields, LogicServerCommandFields commandFields, bool baseFirst)
        : base(
            commandFields.ServerCommandId,
            commandFields.LogicCommandFields.ExecuteSubTick,
            commandFields.LogicCommandFields.DebugData0,
            commandFields.LogicCommandFields.DebugData1)
    {
        _baseFirst = baseFirst;
        Type = type;
        Fields = fields;
    }

    public override int Type { get; }
    public ReadOnlyMemory<LogicCommandField> Fields { get; }

    internal static LogicServerCommandWithFields Decode(int type, ReadOnlySpan<LogicCommandFieldSchema> fieldSchemas, bool baseFirst, SupercellStream stream, LogicEnvironment environment)
    {
        var commandFields = default(LogicServerCommandFields);
        var fields = Array.Empty<LogicCommandField>();

        if (baseFirst)
            commandFields = DecodeLogicServerCommand(stream, environment);

        fields = LogicCommandFieldSchema.DecodeFields(fieldSchemas, stream);

        if (!baseFirst)
            commandFields = DecodeLogicServerCommand(stream, environment);

        return new LogicServerCommandWithFields(type, fields, commandFields, baseFirst);
    }

    internal override void EncodeBody(SupercellStream stream, LogicEnvironment environment)
    {
        if (_baseFirst)
            EncodeLogicServerCommand(stream, environment);

        foreach (var field in Fields.Span)
            field.Encode(stream);

        if (!_baseFirst)
            EncodeLogicServerCommand(stream, environment);
    }
}
