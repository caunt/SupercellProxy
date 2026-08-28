using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// A command whose native class adds no fields to <see cref="Command"/>.
/// </summary>
internal sealed record CommandWithNoFields : Command
{
    internal static readonly int[] CommandTypes =
    [
        84,
        97,
        391,
        503,
        505,
        507,
        508,
        510,
        513,
        515,
        517,
        524,
        526,
        527,
        528,
        529,
        CommandRegistry.HomeLoadedCommandType,
        533,
        536,
        537,
        539,
        541,
        546,
        547,
        548,
        551,
        553,
        554,
        555,
        557,
        562,
        564,
        566,
        571,
        572,
        575,
        578,
        580,
        582,
        583,
        587,
        590,
        592,
        593,
        595,
        596,
        598,
        604,
        613,
        614,
        615,
        616,
        622,
        628,
        630,
        631,
        633,
        635,
        637,
        639,
        640,
        644,
        645,
        647,
        652,
        653,
        659,
        660,
        662,
        663,
        664,
        668,
        671,
        674,
        676,
        677,
        678,
        681,
        682,
        683,
        684,
        685,
        688,
        695,
        697,
        698,
        699,
    ];

    /// <summary>
    /// Initializes a new <see cref="CommandWithNoFields"/> instance.
    /// </summary>
    public CommandWithNoFields(
        int type,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        Type = type;
    }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type { get; }

    internal static CommandWithNoFields Decode(
        int type,
        MessageStream stream,
        CommandEnvironment environment
    )
    {
        var fields = DecodeCommand(stream, environment);
        return new CommandWithNoFields(
            type,
            fields.ExecutionPhaseCounter,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
    }
}
