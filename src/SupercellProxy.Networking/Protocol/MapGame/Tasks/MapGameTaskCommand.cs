using System.Globalization;

using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// <para>Commands whose native 1.72.84 bodies contain the shared optional map-game task structure.</para>
/// </summary>
public sealed record MapGameTaskCommand : Command
{
    /// <summary>
    /// Defines the <c language="csharp">CommandTypes</c> value.
    /// </summary>
    public static readonly int[] CommandTypes =
    [
        278,
        279,
        280,
        281,
        282,
        283,
        284,
        290,
        291,
        295,
        310,
        312,
        314,
    ];
    private static readonly HashSet<int> TypesWithOptionalValues = [284, 291, 310];

    /// <summary>
    /// Initializes a new <see cref="MapGameTaskCommand"/> instance.
    /// </summary>
    public MapGameTaskCommand(
        int type,
        MapGameTask? task,
        ReadOnlyMemory<int>? optionalValues = null,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        if (!CommandTypes.Contains(type))
            throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Logic command type {type} does not use the map-game task schema."));

        if (!TypesWithOptionalValues.Contains(type) && optionalValues is not null)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Logic command type {type} has no optional value array."));

        Type = type;
        Task = task;
        OptionalValues = optionalValues?.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">OptionalValues</c> value.
    /// </summary>
    public ReadOnlyMemory<int>? OptionalValues { get; }

    /// <summary>
    /// Gets the <c language="csharp">Task</c> value.
    /// </summary>
    public MapGameTask? Task { get; }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameTaskCommand Decode(int type, MessageStream stream, CommandEnvironment environment, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields = DecodeCommand(stream, environment);
        MapGameTask? task = stream.ReadBoolean() ? MapGameTask.Decode(stream, dataResolver) : null;
        ReadOnlyMemory<int>? optionalValues = null;

        if (TypesWithOptionalValues.Contains(type) && stream.ReadBoolean())
            optionalValues = CommandVariableIntArrayField.DecodeValues(stream.ReadVariableInt(), stream);

        return new MapGameTaskCommand(type, task, optionalValues, commandFields.ExecutionPhaseCounter, commandFields.DebugData0, commandFields.DebugData1);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteBoolean(Task is not null);
        Task?.Encode(stream);

        if (!TypesWithOptionalValues.Contains(Type))
            return;

        stream.WriteBoolean(OptionalValues is not null);

        if (OptionalValues is null)
            return;

        stream.WriteVariableInt(OptionalValues.Value.Length);

        foreach (int value in OptionalValues.Value.Span)
            stream.WriteVariableInt(value);
    }
}
