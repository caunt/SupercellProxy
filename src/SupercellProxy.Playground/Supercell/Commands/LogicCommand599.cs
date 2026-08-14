using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Logic command 599. The stripped client does not expose semantic field names.
/// </summary>
public sealed record LogicCommand599 : LogicCommand
{
    public const int CommandType = 599;

    public LogicCommand599(
        int unknown0,
        int unknown1,
        ReadOnlyMemory<byte> payload,
        LogicCommandInt32Pair? optionalPair,
        int executeSubTick = -1,
        LogicCommandData? debugData0 = null,
        LogicCommandData? debugData1 = null)
        : base(executeSubTick, debugData0, debugData1)
    {
        Unknown0 = unknown0;
        Unknown1 = unknown1;
        Payload = payload.ToArray();
        OptionalPair = optionalPair;
    }

    public override int Type => CommandType;
    public int Unknown0 { get; }
    public int Unknown1 { get; }
    public ReadOnlyMemory<byte> Payload { get; }
    public LogicCommandInt32Pair? OptionalPair { get; }

    internal static LogicCommand599 Decode(SupercellStream stream, LogicEnvironment environment)
    {
        var commandFields = DecodeLogicCommand(stream, environment);
        var unknown0 = stream.ReadInt32();
        var unknown1 = stream.ReadInt32();
        var payloadLength = stream.ReadInt32();
        LogicCommandInt32Pair? optionalPair = stream.ReadBoolean()
            ? new LogicCommandInt32Pair(stream.ReadInt32(), stream.ReadInt32())
            : null;

        if (payloadLength < 0 || payloadLength > stream.Length - stream.Position)
            throw new InvalidDataException($"Invalid logic command 599 payload length: {payloadLength}.");

        var payload = new byte[payloadLength];
        stream.ReadExactly(payload);

        return new LogicCommand599(
            unknown0,
            unknown1,
            payload,
            optionalPair,
            commandFields.ExecuteSubTick,
            commandFields.DebugData0,
            commandFields.DebugData1);
    }

    internal override void EncodeBody(SupercellStream stream, LogicEnvironment environment)
    {
        EncodeLogicCommand(stream, environment);
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

public readonly record struct LogicCommandInt32Pair(int Value0, int Value1);
