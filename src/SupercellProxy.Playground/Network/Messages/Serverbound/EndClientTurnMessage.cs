using SupercellProxy.Playground.Network.Streams;
using SupercellProxy.Playground.Supercell;
using SupercellProxy.Playground.Supercell.Commands;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

/// <summary>
/// EndClientTurnMessage (19132) wire representation.
/// </summary>
public record EndClientTurnMessage : IMessage
{
    public const int SubChecksumCount = 8;
    public const int MaxCommandCount = 1024;

    public int Checksum { get; init; }
    public int SubTick { get; init; }
    public Memory<int> SubChecksums { get; init; } = new int[SubChecksumCount];
    public Memory<LogicCommand> Commands { get; init; }
    public LogicEnvironment Environment { get; init; } = LogicEnvironment.Production;
    public Memory<LogicCommandData> DebugCommandData { get; init; }
    public Memory<Memory<byte>> DevelopmentByteArrays { get; init; }

    public static EndClientTurnMessage Create(MessageContainer messageContainer)
    {
        return Create(messageContainer, LogicEnvironment.Production);
    }

    public static EndClientTurnMessage Create(MessageContainer messageContainer, LogicEnvironment environment)
    {
        return Create(messageContainer, environment, null);
    }

    public static EndClientTurnMessage Create(MessageContainer messageContainer, LogicEnvironment environment, ILogicCommandDataResolver? dataResolver)
    {
        var stream = messageContainer.Payload;
        var checksum = stream.ReadVarInt();
        var subTick = stream.ReadVarInt();
        var subChecksums = new int[SubChecksumCount];

        for (var i = 0; i < subChecksums.Length; i++)
            subChecksums[i] = stream.ReadVarInt();

        var commandCount = ReadCollectionCount(stream, MaxCommandCount, "command");
        var commands = new LogicCommand[commandCount];

        for (var i = 0; i < commands.Length; i++)
            commands[i] = LogicCommandRegistry.Decode(stream, environment, dataResolver);

        var debugCommandData = Array.Empty<LogicCommandData>();

        if (environment is not LogicEnvironment.Production)
        {
            var debugCommandDataCount = ReadPayloadBoundedCollectionCount(stream, "debug command data");
            debugCommandData = new LogicCommandData[debugCommandDataCount];

            for (var i = 0; i < debugCommandData.Length; i++)
                debugCommandData[i] = LogicCommandData.Decode(stream);
        }

        var developmentByteArrays = Array.Empty<Memory<byte>>();

        if (environment is LogicEnvironment.Development)
        {
            var developmentByteArrayCount = ReadPayloadBoundedCollectionCount(stream, "development byte array");
            developmentByteArrays = new Memory<byte>[developmentByteArrayCount];

            for (var i = 0; i < developmentByteArrays.Length; i++)
                developmentByteArrays[i] = stream.ReadVarIntByteArray();
        }

        if (stream.Position != stream.Length)
            throw new InvalidDataException($"Unexpected trailing EndClientTurnMessage data at position {stream.Position} of {stream.Length}.");

        return new EndClientTurnMessage
        {
            Checksum = checksum,
            SubTick = subTick,
            SubChecksums = subChecksums,
            Commands = commands,
            Environment = environment,
            DebugCommandData = debugCommandData,
            DevelopmentByteArrays = developmentByteArrays
        };
    }

    public MessageContainer ToContainer(ushort id, ushort messageVersion = 5213)
    {
        if (SubChecksums.Length != SubChecksumCount)
            throw new InvalidDataException($"EndClientTurnMessage must contain exactly {SubChecksumCount} sub-checksums.");

        if (Commands.Length > MaxCommandCount)
            throw new InvalidDataException($"Invalid command count: {Commands.Length}.");

        if (Environment is LogicEnvironment.Production && (DebugCommandData.Length > 0 || DevelopmentByteArrays.Length > 0))
            throw new InvalidDataException("Production EndClientTurnMessage cannot contain diagnostic collections.");

        if (Environment is not LogicEnvironment.Development && DevelopmentByteArrays.Length > 0)
            throw new InvalidDataException("Development byte arrays are only encoded in the development environment.");

        using var stream = SupercellStream.Create();

        stream.WriteVarInt(Checksum);
        stream.WriteVarInt(SubTick);

        foreach (var subChecksum in SubChecksums.Span)
            stream.WriteVarInt(subChecksum);

        stream.WriteVarInt(Commands.Length);

        foreach (var command in Commands.Span)
            LogicCommandRegistry.Encode(stream, command, Environment);

        if (Environment is not LogicEnvironment.Production)
        {
            stream.WriteVarInt(DebugCommandData.Length);

            foreach (var commandData in DebugCommandData.Span)
                commandData.Encode(stream);
        }

        if (Environment is LogicEnvironment.Development)
        {
            stream.WriteVarInt(DevelopmentByteArrays.Length);

            foreach (var byteArray in DevelopmentByteArrays.Span)
                stream.WriteVarIntByteArray(byteArray.Span);
        }

        return new MessageContainer(id, messageVersion, stream);
    }

    private static int ReadCollectionCount(SupercellStream stream, int maximum, string name)
    {
        var count = stream.ReadVarInt();

        if ((uint)count > maximum)
            throw new InvalidDataException($"Invalid {name} count: {count}.");

        return count;
    }

    private static int ReadPayloadBoundedCollectionCount(SupercellStream stream, string name)
    {
        var remainingPayloadLength = stream.Length - stream.Position;
        var maximum = (int)Math.Min(int.MaxValue, remainingPayloadLength);
        return ReadCollectionCount(stream, maximum, name);
    }
}
