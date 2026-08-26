using System.Globalization;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Commands whose native 1.72.84 bodies contain the shared optional map-game task structure.</para>
/// </summary>
public sealed record MapGameTaskCommand : Command
{
    /// <summary>
    /// Defines the <c>CommandTypes</c> value.
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
    private static readonly HashSet<int> _typesWithOptionalValues = [284, 291, 310];

    /// <summary>
    /// Initializes a new <see cref="MapGameTaskCommand"/> instance.
    /// </summary>
    public MapGameTaskCommand(
        int type,
        MapGameTask? task,
        ReadOnlyMemory<int>? optionalValues = null,
        int executeSubTick = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executeSubTick, debugData0, debugData1)
    {
        if (!CommandTypes.Contains(type))
            throw new NotSupportedException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Logic command type {type} does not use the map-game task schema."
                )
            );

        if (!_typesWithOptionalValues.Contains(type) && optionalValues is not null)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Logic command type {type} has no optional value array."
                )
            );

        Type = type;
        Task = task;
        OptionalValues = optionalValues?.ToArray();
    }

    /// <summary>
    /// Gets the <c>Type</c> value.
    /// </summary>
    public override int Type { get; }

    /// <summary>
    /// Gets the <c>Task</c> value.
    /// </summary>
    public MapGameTask? Task { get; }

    /// <summary>
    /// Gets the <c>OptionalValues</c> value.
    /// </summary>
    public ReadOnlyMemory<int>? OptionalValues { get; }

    internal static MapGameTaskCommand Decode(
        int type,
        MessageStream stream,
        CommandEnvironment environment,
        ICommandDataResolver? dataResolver
    )
    {
        var commandFields = DecodeCommand(stream, environment);
        var task = stream.ReadBoolean() ? MapGameTask.Decode(stream, dataResolver) : null;
        ReadOnlyMemory<int>? optionalValues = null;

        if (_typesWithOptionalValues.Contains(type) && stream.ReadBoolean())
            optionalValues = CommandVarIntArrayField.DecodeValues(stream.ReadVarInt(), stream);

        return new MapGameTaskCommand(
            type,
            task,
            optionalValues,
            commandFields.ExecuteSubTick,
            commandFields.DebugData0,
            commandFields.DebugData1
        );
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteBoolean(Task is not null);
        Task?.Encode(stream);

        if (!_typesWithOptionalValues.Contains(Type))
            return;

        stream.WriteBoolean(OptionalValues is not null);

        if (OptionalValues is null)
            return;

        stream.WriteVarInt(OptionalValues.Value.Length);

        foreach (var value in OptionalValues.Value.Span)
            stream.WriteVarInt(value);
    }
}
