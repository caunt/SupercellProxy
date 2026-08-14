using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// A command with a native-proven primitive field sequence and unknown semantic field names.
/// </summary>
public sealed record LogicCommandWithFields : LogicCommand
{
    private readonly bool _baseFirst;

    public LogicCommandWithFields(int type, ReadOnlyMemory<LogicCommandField> fields, int executeSubTick = -1, LogicCommandData? debugData0 = null, LogicCommandData? debugData1 = null)
        : base(executeSubTick, debugData0, debugData1)
    {
        _baseFirst = LogicCommandRegistry.ValidateFields(type, fields.Span, isServerCommand: false);
        Type = type;
        Fields = fields.ToArray();
    }

    private LogicCommandWithFields(int type, ReadOnlyMemory<LogicCommandField> fields, LogicCommandFields commandFields, bool baseFirst)
        : base(commandFields.ExecuteSubTick, commandFields.DebugData0, commandFields.DebugData1)
    {
        _baseFirst = baseFirst;
        Type = type;
        Fields = fields;
    }

    public override int Type { get; }
    public ReadOnlyMemory<LogicCommandField> Fields { get; }

    internal static LogicCommandWithFields Decode(int type, ReadOnlySpan<LogicCommandFieldSchema> fieldSchemas, bool baseFirst, SupercellStream stream, LogicEnvironment environment)
    {
        var commandFields = default(LogicCommandFields);
        var fields = Array.Empty<LogicCommandField>();

        if (baseFirst)
            commandFields = DecodeLogicCommand(stream, environment);

        fields = LogicCommandFieldSchema.DecodeFields(fieldSchemas, stream);

        if (!baseFirst)
            commandFields = DecodeLogicCommand(stream, environment);

        return new LogicCommandWithFields(type, fields, commandFields, baseFirst);
    }

    internal override void EncodeBody(SupercellStream stream, LogicEnvironment environment)
    {
        if (_baseFirst)
            EncodeLogicCommand(stream, environment);

        foreach (var field in Fields.Span)
            field.Encode(stream);

        if (!_baseFirst)
            EncodeLogicCommand(stream, environment);
    }

}
