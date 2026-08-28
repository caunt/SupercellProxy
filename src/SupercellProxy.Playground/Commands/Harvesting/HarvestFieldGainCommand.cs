using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Starts the gain phase of harvesting a crop from a field.</para>
/// </summary>
internal sealed record HarvestFieldGainCommand : Command
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 657;

    /// <summary>
    /// Initializes a new <see cref="HarvestFieldGainCommand"/> instance.
    /// </summary>
    public HarvestFieldGainCommand(
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

    internal static HarvestFieldGainCommand Decode(
        MessageStream stream,
        CommandEnvironment environment
    )
    {
        var fields = DecodeCommand(stream, environment);
        return new HarvestFieldGainCommand(
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
