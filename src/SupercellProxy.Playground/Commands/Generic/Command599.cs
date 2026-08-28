using System.Globalization;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Logic command 599. The stripped client does not expose semantic field names.</para>
/// </summary>
internal sealed record Command599 : Command
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
    /// Gets the <c language="csharp">Payload</c> value.
    /// </summary>
    public ReadOnlyMemory<byte> Payload { get; }

    /// <summary>
    /// Gets the <c language="csharp">OptionalPair</c> value.
    /// </summary>
    public CommandInt32Pair? OptionalPair { get; }

    internal static Command599 Decode(MessageStream stream, CommandEnvironment environment)
    {
        var commandFields = DecodeCommand(stream, environment);
        var unknown0 = stream.ReadInt32();
        var unknown1 = stream.ReadInt32();
        var payloadLength = stream.ReadInt32();
        CommandInt32Pair? optionalPair = stream.ReadBoolean()
            ? new CommandInt32Pair(stream.ReadInt32(), stream.ReadInt32())
            : null;

        if (payloadLength < 0 || payloadLength > stream.Length - stream.Position)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Invalid logic command 599 payload length: {payloadLength}."
                )
            );

        var payload = new byte[payloadLength];
        stream.ReadExactly(payload);

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

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
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
