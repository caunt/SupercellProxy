using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Starts harvesting a ready crop field and credits its rewards.</para>
/// </summary>
internal sealed record StartHarvestFieldCommand : Command
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 544;

    /// <summary>
    /// Initializes a new <see cref="StartHarvestFieldCommand"/> instance.
    /// </summary>
    public StartHarvestFieldCommand(
        int fieldGlobalId,
        int executionPhaseCounter = 0,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        FieldGlobalId = fieldGlobalId;
    }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Gets the <c language="csharp">FieldGlobalId</c> value.
    /// </summary>
    public int FieldGlobalId { get; }

    internal static StartHarvestFieldCommand Decode(
        MessageStream stream,
        CommandEnvironment environment
    )
    {
        var fields = DecodeCommand(stream, environment);
        return new StartHarvestFieldCommand(
            stream.ReadVarInt(),
            fields.ExecutionPhaseCounter,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteVarInt(FieldGlobalId);
    }
}
