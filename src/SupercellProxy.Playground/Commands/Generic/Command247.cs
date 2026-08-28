using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Logic command 247. The stripped client does not expose semantic field names.</para>
/// </summary>
internal sealed record Command247 : Command
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 247;

    /// <summary>
    /// Initializes a new <see cref="Command247"/> instance.
    /// </summary>
    public Command247(
        int globalId,
        ReadOnlyMemory<int> globalIds,
        int unknown0,
        ReadOnlyMemory<int> diagnosticValues,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        if (diagnosticValues.Length is not 0 && diagnosticValues.Length != globalIds.Length)
            throw new InvalidDataException(
                "Logic command 247 diagnostic values must be empty or match the data-reference count."
            );

        GlobalId = globalId;
        GlobalIds = globalIds.ToArray();
        Unknown0 = unknown0;
        DiagnosticValues = diagnosticValues.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Gets the <c language="csharp">GlobalId</c> value.
    /// </summary>
    public int GlobalId { get; }

    /// <summary>
    /// Gets the <c language="csharp">GlobalIds</c> value.
    /// </summary>
    public ReadOnlyMemory<int> GlobalIds { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">DiagnosticValues</c> value.
    /// </summary>
    public ReadOnlyMemory<int> DiagnosticValues { get; }

    internal static Command247 Decode(MessageStream stream, CommandEnvironment environment)
    {
        var commandFields = DecodeCommand(stream, environment);
        var globalId = stream.ReadVarInt();
        var globalIds = CommandVarIntArrayField.DecodeValues(stream.ReadVarInt(), stream);
        var unknown0 = stream.ReadVarInt();
        var diagnosticValues =
            environment is CommandEnvironment.Production
                ? Array.Empty<int>()
                : new int[globalIds.Length];

        for (var i = 0; i < diagnosticValues.Length; i++)
            diagnosticValues[i] = stream.ReadInt32();

        return new Command247(
            globalId,
            globalIds,
            unknown0,
            diagnosticValues,
            commandFields.ExecutionPhaseCounter,
            commandFields.DebugData0,
            commandFields.DebugData1
        );
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        if (
            environment is not CommandEnvironment.Production
            && DiagnosticValues.Length != GlobalIds.Length
        )
            throw new InvalidDataException(
                "Logic command 247 requires one diagnostic value per data reference outside production."
            );

        EncodeCommand(stream, environment);
        stream.WriteVarInt(GlobalId);
        stream.WriteVarInt(GlobalIds.Length);

        foreach (var globalId in GlobalIds.Span)
            stream.WriteVarInt(globalId);

        stream.WriteVarInt(Unknown0);

        if (environment is CommandEnvironment.Production)
            return;

        foreach (var diagnosticValue in DiagnosticValues.Span)
            stream.WriteInt32(diagnosticValue);
    }
}
