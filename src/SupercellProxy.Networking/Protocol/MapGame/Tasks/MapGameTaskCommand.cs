using System.Collections.Frozen;
using System.Globalization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// <para>Native Valley commands whose bodies are the shared optional map-game task structure; <see cref="Kind"/> selects the command.</para>
/// <para>Completing an obstacle, collecting a chicken and collecting a sanctuary animal append an optional var-int array.</para>
/// </summary>
public sealed record MapGameTaskCommand : Command
{
    /// <summary>
    /// Initializes a new <see cref="MapGameTaskCommand"/> instance.
    /// </summary>
    public MapGameTaskCommand(MapGameTaskKind kind, MapGameTask? task, ReadOnlyMemory<int>? optionalValues = null)
    {
        if (!Enum.IsDefined(kind))
            throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Valley task command {kind} is not supported."));

        if (!HasOptionalValues(kind) && optionalValues is not null)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Valley task command {kind} has no optional value array."));

        Kind = kind;
        Task = task;
        OptionalValues = optionalValues is null ? null : (ReadOnlyMemory<int>?)optionalValues.Value.ToArray();
    }

    /// <summary>
    /// Gets the contract of each native command that carries the task structure.
    /// </summary>
    public static FrozenDictionary<MapGameTaskKind, CommandCodec<MapGameTaskCommand>> ContractCodecs { get; } = Enum.GetValues<MapGameTaskKind>().ToFrozenDictionary(static kind => kind, Create);

    /// <inheritdoc />
    public override CommandContract Contract => ContractCodecs[Kind];

    /// <summary>
    /// Gets the native command this record carries.
    /// </summary>
    public MapGameTaskKind Kind { get; }

    /// <summary>
    /// Gets the optional values that completing an obstacle, collecting a chicken and collecting a sanctuary animal append.
    /// </summary>
    public ReadOnlyMemory<int>? OptionalValues { get; }

    /// <summary>
    /// Gets the task.
    /// </summary>
    public MapGameTask? Task { get; }

    private static CommandCodec<MapGameTaskCommand> Create(MapGameTaskKind kind)
    {
        ICodec<MapGameTask?> task = Codecs.Optional(MapGameTask.Codec);

        return Codecs.Command(
            string.Create(CultureInfo.InvariantCulture, $"{kind}Command"),
            HasOptionalValues(kind)
                ? Codecs.Composite(
                    task,
                    static command => command.Task,
                    Codecs.OptionalValue(Codecs.Memory(Codecs.VarInt)),
                    static command => command.OptionalValues,
                    (taskValue, optionalValues) => new MapGameTaskCommand(kind, taskValue, optionalValues)
                )
                : Codecs.Composite(task, static command => command.Task, taskValue => new MapGameTaskCommand(kind, taskValue)),
            CommandLayout.MetadataFirst
        );
    }

    private static bool HasOptionalValues(MapGameTaskKind kind)
    {
        return kind is MapGameTaskKind.CompleteObstacle or MapGameTaskKind.CollectChicken or MapGameTaskKind.CollectSanctuaryAnimal;
    }
}
