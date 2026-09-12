using System.Globalization;

using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.OpaquePayloads;

/// <summary>
/// <para>Logic command 599. The stripped client does not expose semantic field names.</para>
/// </summary>
public sealed record Command599 : Command
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 599;

    /// <summary>
    /// Initializes a new <see cref="Command599"/> instance.
    /// </summary>
    public Command599(
        int unknown0,
        int unknown1,
        ReadOnlyMemory<byte> payload,
        CommandInt32Pair? optionalPair,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        Unknown0 = unknown0;
        Unknown1 = unknown1;
        Payload = payload.ToArray();
        OptionalPair = optionalPair;
    }

    /// <summary>
    /// Gets the <c language="csharp">OptionalPair</c> value.
    /// </summary>
    public CommandInt32Pair? OptionalPair { get; }

    /// <summary>
    /// Gets the <c language="csharp">Payload</c> value.
    /// </summary>
    public ReadOnlyMemory<byte> Payload { get; }

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
    public int Unknown1 { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static Command599 Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields = DecodeCommand(stream, environment);
        int unknown0 = stream.ReadInt32();
        int unknown1 = stream.ReadInt32();
        int payloadLength = stream.ReadInt32();

        CommandInt32Pair? optionalPair = stream.ReadBoolean()
            ? new CommandInt32Pair(stream.ReadInt32(), stream.ReadInt32())
            : null;

        if (payloadLength < 0 || payloadLength > stream.Length - stream.Position)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid logic command 599 payload length: {payloadLength}."));

        byte[] payload = stream.ReadBytes(payloadLength);

        return new Command599(
            unknown0,
            unknown1,
            payload,
            optionalPair,
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
        EncodeCommand(stream, environment);
        stream.WriteInt32(Unknown0);
        stream.WriteInt32(Unknown1);
        stream.WriteInt32(Payload.Length);
        stream.WriteBoolean(OptionalPair is not null);

        if (OptionalPair is not null)
        {
            stream.WriteInt32(OptionalPair.Value.Value0);
            stream.WriteInt32(OptionalPair.Value.Value1);
        }

        stream.Write(Payload.Span);
    }
}
