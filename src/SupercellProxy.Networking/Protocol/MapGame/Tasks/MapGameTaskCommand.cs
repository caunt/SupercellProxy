using System.Collections.Frozen;
using System.Globalization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// <para>Commands whose native 1.72.84 bodies contain the shared optional map-game task structure.</para>
/// <para>Commands 284, 291 and 310 append an optional var-int array.</para>
/// </summary>
public sealed record MapGameTaskCommand : Command
{
    /// <summary>The command that collects a chicken.</summary>
    public const int CollectChickenCommandType = 291;

    /// <summary>The command that collects a gas station.</summary>
    public const int CollectGasStationCommandType = 312;

    /// <summary>The command that collects a sanctuary animal.</summary>
    public const int CollectSanctuaryAnimalCommandType = 310;

    /// <summary>The command that completes a delivery task.</summary>
    public const int CompleteDeliveryTaskCommandType = 280;

    /// <summary>The command that completes a dump task.</summary>
    public const int CompleteDumpTaskCommandType = 278;

    private const int OptionalValuesCommandType = 284;

    /// <summary>
    /// Initializes a new <see cref="MapGameTaskCommand"/> instance.
    /// </summary>
    public MapGameTaskCommand(int type, MapGameTask? task, ReadOnlyMemory<int>? optionalValues = null)
    {
        if (!ContractCodecs.ContainsKey(type))
            throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Logic command type {type} does not use the map-game task schema."));

        if (!HasOptionalValues(type) && optionalValues is not null)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Logic command type {type} has no optional value array."));

        Type = type;
        Task = task;
        OptionalValues = optionalValues is null ? null : (ReadOnlyMemory<int>?)optionalValues.Value.ToArray();
    }

    /// <summary>
    /// Gets the codec of each command id that carries the task structure.
    /// </summary>
    public static FrozenDictionary<int, CommandCodec<MapGameTaskCommand>> ContractCodecs { get; } = new[]
    {
        CompleteDumpTaskCommandType,
        279,
        CompleteDeliveryTaskCommandType,
        281,
        282,
        283,
        OptionalValuesCommandType,
        290,
        CollectChickenCommandType,
        295,
        CollectSanctuaryAnimalCommandType,
        CollectGasStationCommandType,
        314,
    }.ToFrozenDictionary(static type => type, Create);

    /// <summary>
    /// Gets the optional values commands 284, 291 and 310 append.
    /// </summary>
    public ReadOnlyMemory<int>? OptionalValues { get; }

    /// <summary>
    /// Gets the task.
    /// </summary>
    public MapGameTask? Task { get; }

    /// <inheritdoc />
    public override int Type { get; }

    private static CommandCodec<MapGameTaskCommand> Create(int type)
    {
        ICodec<MapGameTask?> task = Codecs.Optional(MapGameTask.Codec);

        return Codecs.Command(
            type,
            HasOptionalValues(type)
                ? Codecs.Composite(
                    task,
                    static command => command.Task,
                    Codecs.OptionalValue(Codecs.Memory(Codecs.VarInt)),
                    static command => command.OptionalValues,
                    (taskValue, optionalValues) => new MapGameTaskCommand(type, taskValue, optionalValues)
                )
                : Codecs.Composite(task, static command => command.Task, taskValue => new MapGameTaskCommand(type, taskValue)),
            CommandLayout.MetadataFirst
        );
    }

    private static bool HasOptionalValues(int type)
    {
        return type is OptionalValuesCommandType or CollectChickenCommandType or CollectSanctuaryAnimalCommandType;
    }
}
