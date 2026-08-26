using System.Globalization;
using SupercellProxy.Playground.Commands;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

/// <summary>
/// <para>EndClientTurnMessage (10224) wire representation.</para>
/// </summary>
public record EndClientTurnMessage : IMessage
{
    /// <summary>
    /// Defines the <c>CurrentVersion</c> value.
    /// </summary>
    public const ushort CurrentVersion = 10;

    /// <summary>
    /// Defines the <c>SubChecksumCount</c> value.
    /// </summary>
    public const int SubChecksumCount = 8;

    /// <summary>
    /// Defines the <c>MaxCommandCount</c> value.
    /// </summary>
    public const int MaxCommandCount = 1024;

    /// <summary>
    /// Gets or sets the <c>Checksum</c> value.
    /// </summary>
    public int Checksum { get; init; }

    /// <summary>
    /// Gets or sets the <c>SubTick</c> value.
    /// </summary>
    public int SubTick { get; init; }

    /// <summary>
    /// Gets or sets the <c>SubChecksums</c> value.
    /// </summary>
    public Memory<int> SubChecksums { get; init; } = new int[SubChecksumCount];

    /// <summary>
    /// Gets or sets the <c>Commands</c> value.
    /// </summary>
    public Memory<Command> Commands { get; init; }

    /// <summary>
    /// Gets or sets the <c>Environment</c> value.
    /// </summary>
    public CommandEnvironment Environment { get; init; } = CommandEnvironment.Production;

    /// <summary>
    /// Gets or sets the <c>DebugCommandData</c> value.
    /// </summary>
    public Memory<CommandData> DebugCommandData { get; init; }

    /// <summary>
    /// Gets or sets the <c>DevelopmentByteArrays</c> value.
    /// </summary>
    public Memory<Memory<byte>> DevelopmentByteArrays { get; init; }

    /// <summary>
    /// Creates a <c>EndClientTurnMessage</c> from the supplied data.
    /// </summary>
    public static EndClientTurnMessage Create(MessageContainer messageContainer)
    {
        return Create(messageContainer, CommandEnvironment.Production);
    }

    /// <summary>
    /// Creates a <c>EndClientTurnMessage</c> from the supplied data.
    /// </summary>
    public static EndClientTurnMessage Create(
        MessageContainer messageContainer,
        CommandEnvironment environment
    )
    {
        return Create(messageContainer, environment, dataResolver: null);
    }

    /// <summary>
    /// Creates a <c>EndClientTurnMessage</c> from the supplied data.
    /// </summary>
    public static EndClientTurnMessage Create(
        MessageContainer messageContainer,
        CommandEnvironment environment,
        ICommandDataResolver? dataResolver
    )
    {
        var stream = messageContainer.Payload;
        var checksum = stream.ReadVarInt();
        var subTick = stream.ReadVarInt();
        var subChecksums = new int[SubChecksumCount];

        for (var i = 0; i < subChecksums.Length; i++)
            subChecksums[i] = stream.ReadVarInt();

        var commandCount = ReadCollectionCount(stream, MaxCommandCount, "command");
        var commands = new Command[commandCount];

        for (var i = 0; i < commands.Length; i++)
            commands[i] = CommandRegistry.Decode(stream, environment, dataResolver);

        var debugCommandData = ReadDebugCommandData(stream, environment);
        var developmentByteArrays = ReadDevelopmentByteArrays(stream, environment);

        if (stream.Position != stream.Length)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unexpected trailing EndClientTurnMessage data at position {stream.Position} of {stream.Length}."
                )
            );

        return new EndClientTurnMessage
        {
            Checksum = checksum,
            SubTick = subTick,
            SubChecksums = subChecksums,
            Commands = commands,
            Environment = environment,
            DebugCommandData = debugCommandData,
            DevelopmentByteArrays = developmentByteArrays,
        };
    }

    private static CommandData[] ReadDebugCommandData(
        MessageStream stream,
        CommandEnvironment environment
    )
    {
        if (environment is CommandEnvironment.Production)
            return [];

        var values = new CommandData[
            ReadPayloadBoundedCollectionCount(stream, "debug command data")
        ];
        for (var index = 0; index < values.Length; index++)
            values[index] = CommandData.Decode(stream);
        return values;
    }

    private static Memory<byte>[] ReadDevelopmentByteArrays(
        MessageStream stream,
        CommandEnvironment environment
    )
    {
        if (environment is not CommandEnvironment.Development)
            return [];

        var values = new Memory<byte>[
            ReadPayloadBoundedCollectionCount(stream, "development byte array")
        ];
        for (var index = 0; index < values.Length; index++)
            values[index] = stream.ReadVarIntByteArray();
        return values;
    }

    /// <summary>
    /// Executes the <c>ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort messageVersion = 0)
    {
        if (SubChecksums.Length != SubChecksumCount)
            throw new InvalidDataException(
                $"EndClientTurnMessage must contain exactly {SubChecksumCount} sub-checksums."
            );

        if (Commands.Length > MaxCommandCount)
            throw new InvalidDataException($"Invalid command count: {Commands.Length}.");

        if (
            Environment is CommandEnvironment.Production
            && (DebugCommandData.Length > 0 || DevelopmentByteArrays.Length > 0)
        )
            throw new InvalidDataException(
                "Production EndClientTurnMessage cannot contain diagnostic collections."
            );

        if (Environment is not CommandEnvironment.Development && DevelopmentByteArrays.Length > 0)
            throw new InvalidDataException(
                "Development byte arrays are only encoded in the development environment."
            );

        using var stream = MessageStream.Create();

        stream.WriteVarInt(Checksum);
        stream.WriteVarInt(SubTick);

        foreach (var subChecksum in SubChecksums.Span)
            stream.WriteVarInt(subChecksum);

        stream.WriteVarInt(Commands.Length);

        foreach (var command in Commands.Span)
            CommandRegistry.Encode(stream, command, Environment);

        if (Environment is not CommandEnvironment.Production)
        {
            stream.WriteVarInt(DebugCommandData.Length);

            foreach (var commandData in DebugCommandData.Span)
                commandData.Encode(stream);
        }

        if (Environment is CommandEnvironment.Development)
        {
            stream.WriteVarInt(DevelopmentByteArrays.Length);

            foreach (var byteArray in DevelopmentByteArrays.Span)
                stream.WriteVarIntByteArray(byteArray.Span);
        }

        return new MessageContainer(id, messageVersion, stream);
    }

    private static int ReadCollectionCount(MessageStream stream, int maximum, string name)
    {
        var count = stream.ReadVarInt();

        if (uint.CreateTruncating(count) > maximum)
            throw new InvalidDataException(
                string.Create(CultureInfo.InvariantCulture, $"Invalid {name} count: {count}.")
            );

        return count;
    }

    private static int ReadPayloadBoundedCollectionCount(MessageStream stream, string name)
    {
        var remainingPayloadLength = stream.Length - stream.Position;
        var maximum = int.CreateTruncating(Math.Min(int.MaxValue, remainingPayloadLength));
        return ReadCollectionCount(stream, maximum, name);
    }
}
