using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Starts harvesting a ready crop field and credits its rewards.</para>
/// </summary>
public sealed record StartHarvestFieldCommand : Command
{
    /// <summary>
    /// Defines the <c>CommandType</c> value.
    /// </summary>
    public const int CommandType = 544;

    /// <summary>
    /// Initializes a new <see cref="StartHarvestFieldCommand"/> instance.
    /// </summary>
    public StartHarvestFieldCommand(
        int fieldGlobalId,
        int executeSubTick = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executeSubTick, debugData0, debugData1)
    {
        FieldGlobalId = fieldGlobalId;
    }

    /// <summary>
    /// Gets the <c>Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Gets the <c>FieldGlobalId</c> value.
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
            fields.ExecuteSubTick,
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
