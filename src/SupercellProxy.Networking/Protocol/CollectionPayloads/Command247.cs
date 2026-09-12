using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CollectionPayloads;

/// <summary>
/// <para>Logic command 247. The stripped client does not expose semantic field names.</para>
/// </summary>
public sealed record Command247 : Command
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 247;

    /// <summary>
    /// Initializes a new <see cref="Command247"/> instance.
    /// </summary>
    public Command247(
        int globalIdentifier,
        ReadOnlyMemory<int> globalIdentifiers,
        int unknown0,
        ReadOnlyMemory<int> diagnosticValues,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        if (diagnosticValues.Length is not 0 && diagnosticValues.Length != globalIdentifiers.Length)
            throw new InvalidDataException(message: "Logic command 247 diagnostic values must be empty or match the data-reference count.");

        GlobalIdentifier = globalIdentifier;
        GlobalIdentifiers = globalIdentifiers.ToArray();
        Unknown0 = unknown0;
        DiagnosticValues = diagnosticValues.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">DiagnosticValues</c> value.
    /// </summary>
    public ReadOnlyMemory<int> DiagnosticValues { get; }

    /// <summary>
    /// Gets the <c language="csharp">GlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("GlobalId")]
    public int GlobalIdentifier { get; }

    /// <summary>
    /// Gets the <c language="csharp">GlobalIds</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("GlobalIds")]
    public ReadOnlyMemory<int> GlobalIdentifiers { get; }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static Command247 Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields = DecodeCommand(stream, environment);
        int globalIdentifier = stream.ReadVariableInt();
        int[] globalIdentifiers = CommandVariableIntArrayField.DecodeValues(stream.ReadVariableInt(), stream);
        int unknown0 = stream.ReadVariableInt();

        int[] diagnosticValues =
            environment is CommandEnvironment.Production
                ? []
                : new int[globalIdentifiers.Length];

        for (int index = 0; index < diagnosticValues.Length; index++)
            diagnosticValues[index] = stream.ReadInt32();

        return new Command247(
            globalIdentifier,
            globalIdentifiers,
            unknown0,
            diagnosticValues,
            commandFields.ExecutionPhaseCounter,
            commandFields.DebugData0,
            commandFields.DebugData1
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        if (environment is not CommandEnvironment.Production && DiagnosticValues.Length != GlobalIdentifiers.Length)
            throw new InvalidDataException(message: "Logic command 247 requires one diagnostic value per data reference outside production.");

        EncodeCommand(stream, environment);
        stream.WriteVariableInt(GlobalIdentifier);
        stream.WriteVariableInt(GlobalIdentifiers.Length);

        foreach (int globalIdentifier in GlobalIdentifiers.Span)
            stream.WriteVariableInt(globalIdentifier);

        stream.WriteVariableInt(Unknown0);

        if (environment is CommandEnvironment.Production)
            return;

        foreach (int diagnosticValue in DiagnosticValues.Span)
            stream.WriteInt32(diagnosticValue);
    }
}
