using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ScalarPayloads;

/// <summary>
/// <para>Server command 210. The native class and field names are not present in the stripped client.</para>
/// </summary>
public sealed record ServerCommand210 : ServerCommand
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
        LongIdentifier unknown1,
        int serverCommandIdentifier,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(serverCommandIdentifier, executionPhaseCounter, debugData0, debugData1)
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
    public LongIdentifier Unknown1 { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static ServerCommand210 Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int unknown0 = stream.ReadVariableInt();
        LongIdentifier unknown1 = stream.ReadLongIdentifier();
        (int serverCommandIdentifier, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields) = DecodeServerCommand(stream, environment);

        return new ServerCommand210(
            unknown0,
            unknown1,
            serverCommandIdentifier,
            commandFields.ExecutionPhaseCounter,
            commandFields.DebugData0,
            commandFields.DebugData1
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(Unknown0);
        stream.WriteLongIdentifier(Unknown1);
        EncodeServerCommand(stream, environment);
    }
}
