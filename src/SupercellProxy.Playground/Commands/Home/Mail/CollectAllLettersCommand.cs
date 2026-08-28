using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Collects all eligible letters.</para>
/// </summary>
internal sealed record CollectAllLettersCommand : Command
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 672;

    /// <summary>
    /// Initializes a new <see cref="CollectAllLettersCommand"/> instance.
    /// </summary>
    public CollectAllLettersCommand(
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1) { }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    internal static CollectAllLettersCommand Decode(
        MessageStream stream,
        CommandEnvironment environment
    )
    {
        var fields = DecodeCommand(stream, environment);
        return new CollectAllLettersCommand(
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
