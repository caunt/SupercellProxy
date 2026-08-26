using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Completes harvesting a crop from a field.</para>
/// </summary>
public sealed record HarvestFieldCommand : Command
{
    /// <summary>
    /// Defines the <c>CommandType</c> value.
    /// </summary>
    public const int CommandType = 506;

    /// <summary>
    /// Initializes a new <see cref="HarvestFieldCommand"/> instance.
    /// </summary>
    public HarvestFieldCommand(
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

    internal static HarvestFieldCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        var fields = DecodeCommand(stream, environment);
        return new HarvestFieldCommand(
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
