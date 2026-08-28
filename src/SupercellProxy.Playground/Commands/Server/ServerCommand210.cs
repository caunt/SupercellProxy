using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Server command 210. The native class and field names are not present in the stripped client.</para>
/// </summary>
internal sealed record ServerCommand210 : ServerCommand
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 210;

    /// <summary>
    /// Initializes a new <see cref="ServerCommand210"/> instance.
    /// </summary>
    public ServerCommand210(
        int unknown0,
        LongId unknown1,
        int serverCommandId,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(serverCommandId, executionPhaseCounter, debugData0, debugData1)
    {
        Unknown0 = unknown0;
        Unknown1 = unknown1;
    }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public LongId Unknown1 { get; }

    internal static ServerCommand210 Decode(MessageStream stream, CommandEnvironment environment)
    {
        var unknown0 = stream.ReadVarInt();
        var unknown1 = stream.ReadLongId();
        var fields = DecodeServerCommand(stream, environment);

        return new ServerCommand210(
            unknown0,
            unknown1,
            fields.ServerCommandId,
            fields.CommandFields.ExecutionPhaseCounter,
            fields.CommandFields.DebugData0,
            fields.CommandFields.DebugData1
        );
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteLongId(Unknown1);
        EncodeServerCommand(stream, environment);
    }
}
