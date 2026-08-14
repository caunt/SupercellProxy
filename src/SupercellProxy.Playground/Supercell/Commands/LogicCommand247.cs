using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Logic command 247. The stripped client does not expose semantic field names.
/// </summary>
public sealed record LogicCommand247 : LogicCommand
{
    public const int CommandType = 247;

    public LogicCommand247(
        int globalId,
        ReadOnlyMemory<int> globalIds,
        int unknown0,
        ReadOnlyMemory<int> diagnosticValues,
        int executeSubTick = -1,
        LogicCommandData? debugData0 = null,
        LogicCommandData? debugData1 = null)
        : base(executeSubTick, debugData0, debugData1)
    {
        if (diagnosticValues.Length is not 0 && diagnosticValues.Length != globalIds.Length)
            throw new InvalidDataException("Logic command 247 diagnostic values must be empty or match the data-reference count.");

        GlobalId = globalId;
        GlobalIds = globalIds.ToArray();
        Unknown0 = unknown0;
        DiagnosticValues = diagnosticValues.ToArray();
    }

    public override int Type => CommandType;
    public int GlobalId { get; }
    public ReadOnlyMemory<int> GlobalIds { get; }
    public int Unknown0 { get; }
    public ReadOnlyMemory<int> DiagnosticValues { get; }

    internal static LogicCommand247 Decode(SupercellStream stream, LogicEnvironment environment)
    {
        var commandFields = DecodeLogicCommand(stream, environment);
        var globalId = stream.ReadVarInt();
        var globalIds = LogicCommandVarIntArrayField.DecodeValues(stream.ReadVarInt(), stream);
        var unknown0 = stream.ReadVarInt();
        var diagnosticValues = environment is LogicEnvironment.Production ? Array.Empty<int>() : new int[globalIds.Length];

        for (var i = 0; i < diagnosticValues.Length; i++)
            diagnosticValues[i] = stream.ReadInt32();

        return new LogicCommand247(
            globalId,
            globalIds,
            unknown0,
            diagnosticValues,
            commandFields.ExecuteSubTick,
            commandFields.DebugData0,
            commandFields.DebugData1);
    }

    internal override void EncodeBody(SupercellStream stream, LogicEnvironment environment)
    {
        if (environment is not LogicEnvironment.Production && DiagnosticValues.Length != GlobalIds.Length)
            throw new InvalidDataException("Logic command 247 requires one diagnostic value per data reference outside production.");

        EncodeLogicCommand(stream, environment);
        stream.WriteVarInt(GlobalId);
        stream.WriteVarInt(GlobalIds.Length);

        foreach (var globalId in GlobalIds.Span)
            stream.WriteVarInt(globalId);

        stream.WriteVarInt(Unknown0);

        if (environment is LogicEnvironment.Production)
            return;

        foreach (var diagnosticValue in DiagnosticValues.Span)
            stream.WriteInt32(diagnosticValue);
    }
}
