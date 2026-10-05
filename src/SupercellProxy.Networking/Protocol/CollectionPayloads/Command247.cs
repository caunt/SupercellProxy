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
    public Command247(int globalId, ReadOnlyMemory<int> globalIds, int unknown0, ReadOnlyMemory<int> diagnosticValues)
    {
        if (diagnosticValues.Length is not 0 && diagnosticValues.Length != globalIds.Length)
            throw new InvalidDataException(message: "Logic command 247 diagnostic values must be empty or match the data-reference count.");

        GlobalId = globalId;
        GlobalIds = globalIds.ToArray();
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
    public int GlobalId { get; }

    /// <summary>
    /// Gets the <c language="csharp">GlobalIds</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("GlobalIds")]
    public ReadOnlyMemory<int> GlobalIds { get; }

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

        int globalId = stream.ReadVarInt();
        int[] globalIds = CommandVarIntArrayField.DecodeValues(stream.ReadVarInt(), stream);
        int unknown0 = stream.ReadVarInt();

        int[] diagnosticValues =
            environment is CommandEnvironment.Production
                ? []
                : new int[globalIds.Length];

        for (int index = 0; index < diagnosticValues.Length; index++)
            diagnosticValues[index] = stream.ReadInt32();

        return new Command247(globalId, globalIds, unknown0, diagnosticValues);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        if (environment is not CommandEnvironment.Production && DiagnosticValues.Length != GlobalIds.Length)
            throw new InvalidDataException(message: "Logic command 247 requires one diagnostic value per data reference outside production.");

        stream.WriteVarInt(GlobalId);
        stream.WriteVarInt(GlobalIds.Length);

        foreach (int globalId in GlobalIds.Span)
            stream.WriteVarInt(globalId);

        stream.WriteVarInt(Unknown0);

        if (environment is CommandEnvironment.Production)
            return;

        foreach (int diagnosticValue in DiagnosticValues.Span)
            stream.WriteInt32(diagnosticValue);
    }
}
