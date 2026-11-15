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
    /// <summary>Initializes a new instance of the <see cref="MapGameTaskCommand"/> record. Creates one of the Valley task commands.</summary>
    /// <param name="kind">Which Valley command this is.</param>
    /// <param name="task">The task the command acts on, as the client has it; <see langword="null"/> when not sent.</param>
    /// <param name="optionalValues">
    /// Extra numbers that only obstacle completion, chicken collection and sanctuary animal collection carry; for a sanctuary
    /// animal they are the route the car continues on.
    /// </param>
    /// <exception cref="NotSupportedException"><paramref name="kind"/> is not a defined <see cref="MapGameTaskKind"/> member.</exception>
    /// <exception cref="InvalidDataException"><paramref name="optionalValues"/> is given for a kind other than obstacle completion, chicken collection or sanctuary animal collection.</exception>
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
    /// <value>One codec per <see cref="MapGameTaskKind"/> member, each named after its kind and reading the optional task, plus the optional value array for the three kinds that carry one.</value>
    public static FrozenDictionary<MapGameTaskKind, CommandCodec<MapGameTaskCommand>> ContractCodecs { get; } = Enum.GetValues<MapGameTaskKind>()
        .ToFrozenDictionary(static kind => kind, Create);

    /// <inheritdoc />
    public override CommandContract Contract => ContractCodecs[Kind];

    /// <summary>
    /// Gets the native command this record carries.
    /// </summary>
    /// <value>A defined <see cref="MapGameTaskKind"/> member; it selects the wire contract and decides whether <see cref="OptionalValues"/> may be present.</value>
    public MapGameTaskKind Kind { get; }

    /// <summary>
    /// Gets the optional values that completing an obstacle, collecting a chicken and collecting a sanctuary animal append.
    /// </summary>
    /// <value>A copy of the numbers sent after the task, such as the route a car continues on after collecting a sanctuary animal; <see langword="null"/> when absent, and always <see langword="null"/> for the other kinds.</value>
    public ReadOnlyMemory<int>? OptionalValues { get; }

    /// <summary>Gets the task the command acts on, as the client has it; <see langword="null"/> when not sent.</summary>
    /// <value>The client's copy of the task with its states, the first field after the command metadata; <see langword="null"/> when the wire marks it absent.</value>
    public MapGameTask? Task { get; }

    private static CommandCodec<MapGameTaskCommand> Create(MapGameTaskKind kind)
    {
        ICodec<MapGameTask?> task = Codecs.Optional(MapGameTask.Codec);

        return Codecs.Command(string.Create(CultureInfo.InvariantCulture, $"{kind}Command"),
            HasOptionalValues(kind)
                ? Codecs.Composite(task,
                    static command => command.Task,
                    Codecs.OptionalValue(Codecs.Memory(Codecs.VarInt)),
                    static command => command.OptionalValues,
                    (taskValue, optionalValues) => new MapGameTaskCommand(kind, taskValue, optionalValues))
                : Codecs.Composite(task, static command => command.Task, taskValue => new MapGameTaskCommand(kind, taskValue)),
            CommandLayout.MetadataFirst);
    }

    private static bool HasOptionalValues(MapGameTaskKind kind)
    {
        return kind is MapGameTaskKind.CompleteObstacle or MapGameTaskKind.CollectChicken or MapGameTaskKind.CollectSanctuaryAnimal;
    }
}
