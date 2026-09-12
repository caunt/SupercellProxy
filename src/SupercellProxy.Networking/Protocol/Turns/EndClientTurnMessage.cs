using System.Globalization;

using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Turns;

/// <summary>
/// <para>EndClientTurnMessage (10224) wire representation.</para>
/// </summary>
public sealed record EndClientTurnMessage : IMessage
{
    /// <summary>
    /// Defines the <c language="csharp">CurrentVersion</c> value.
    /// </summary>
    public const ushort CurrentVersion = 10;

    /// <summary>
    /// Defines the <c language="csharp">MaxCommandCount</c> value.
    /// </summary>
    public const int MaximumCommandCount = 1024;

    /// <summary>
    /// Defines the <c language="csharp">SubChecksumCount</c> value.
    /// </summary>
    public const int SubChecksumCount = 8;

    /// <summary>
    /// Gets or sets the <c language="csharp">Checksum</c> value.
    /// </summary>
    public int Checksum { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Commands</c> value.
    /// </summary>
    public Memory<Command> Commands { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">DebugCommandData</c> value.
    /// </summary>
    public Memory<CommandData> DebugCommandData { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">DevelopmentByteArrays</c> value.
    /// </summary>
    public Memory<Memory<byte>> DevelopmentByteArrays { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">SubChecksums</c> value.
    /// </summary>
    public Memory<int> SubChecksums { get; init; } = new int[SubChecksumCount];

    /// <summary>
    /// Gets or sets the <c language="csharp">Environment</c> value.
    /// </summary>
    public CommandEnvironment Environment { get; init; } = CommandEnvironment.Production;

    /// <summary>
    /// Gets or sets the <c language="csharp">SubTick</c> value.
    /// </summary>
    public int SubTick { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">EndClientTurnMessage</c> from the supplied data.
    /// </summary>
    public static EndClientTurnMessage Create(MessageContainer messageContainer)
    {
        return Create(messageContainer, CommandEnvironment.Production);
    }

    /// <summary>
    /// Creates a <c language="csharp">EndClientTurnMessage</c> from the supplied data.
    /// </summary>
    public static EndClientTurnMessage Create(MessageContainer messageContainer, CommandEnvironment environment)
    {
        return Create(messageContainer, environment, dataResolver: null);
    }

    /// <summary>
    /// Creates a <c language="csharp">EndClientTurnMessage</c> from the supplied data.
    /// </summary>
    public static EndClientTurnMessage Create(MessageContainer messageContainer, CommandEnvironment environment, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(messageContainer);
        MessageStream stream = messageContainer.Payload;
        int checksum = stream.ReadVariableInt();
        int subTick = stream.ReadVariableInt();
        int[] subChecksums = new int[SubChecksumCount];

        for (int index = 0; index < subChecksums.Length; index++)
            subChecksums[index] = stream.ReadVariableInt();

        int commandCount = ReadCollectionCount(stream, MaximumCommandCount, name: "command");
        Command[] commands = new Command[commandCount];

        for (int index = 0; index < commands.Length; index++)
            commands[index] = CommandRegistry.Decode(stream, environment, dataResolver);

        CommandData[] debugCommandData = ReadDebugCommandData(stream, environment);
        Memory<byte>[] developmentByteArrays = ReadDevelopmentByteArrays(stream, environment);

        return stream.Position != stream.Length
            ? throw new InvalidDataException(
                string.Create(CultureInfo.InvariantCulture, $"Unexpected trailing EndClientTurnMessage data at position {stream.Position} of {stream.Length}.")
            )
            : new EndClientTurnMessage
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

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        if (SubChecksums.Length != SubChecksumCount)
            throw new InvalidDataException($"EndClientTurnMessage must contain exactly {SubChecksumCount} sub-checksums.");

        if (Commands.Length > MaximumCommandCount)
            throw new InvalidDataException($"Invalid command count: {Commands.Length}.");

        if (Environment is CommandEnvironment.Production && (DebugCommandData.Length > 0 || DevelopmentByteArrays.Length > 0))
            throw new InvalidDataException(message: "Production EndClientTurnMessage cannot contain diagnostic collections.");

        if (Environment is not CommandEnvironment.Development && DevelopmentByteArrays.Length > 0)
            throw new InvalidDataException(message: "Development byte arrays are only encoded in the development environment.");

        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(Checksum);
        stream.WriteVariableInt(SubTick);

        foreach (int subChecksum in SubChecksums.Span)
            stream.WriteVariableInt(subChecksum);

        stream.WriteVariableInt(Commands.Length);

        foreach (Command command in Commands.Span)
            CommandRegistry.Encode(stream, command, Environment);

        if (Environment is not CommandEnvironment.Production)
        {
            stream.WriteVariableInt(DebugCommandData.Length);

            foreach (CommandData commandData in DebugCommandData.Span)
                commandData.Encode(stream);
        }

        if (Environment is CommandEnvironment.Development)
        {
            stream.WriteVariableInt(DevelopmentByteArrays.Length);

            foreach (Memory<byte> byteArray in DevelopmentByteArrays.Span)
                stream.WriteVariableIntByteArray(byteArray.Span);
        }

        return new MessageContainer(identifier, version, stream);
    }

    private static int ReadCollectionCount(MessageStream stream, int maximum, string name)
    {
        int count = stream.ReadVariableInt();

        return uint.CreateTruncating(count) > maximum
            ? throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid {name} count: {count}."))
            : count;
    }

    private static CommandData[] ReadDebugCommandData(MessageStream stream, CommandEnvironment environment)
    {
        if (environment is CommandEnvironment.Production)
            return [];

        CommandData[] values = new CommandData[
            ReadPayloadBoundedCollectionCount(stream, name: "debug command data")
        ];

        for (int index = 0; index < values.Length; index++)
            values[index] = CommandData.Decode(stream);

        return values;
    }

    private static Memory<byte>[] ReadDevelopmentByteArrays(MessageStream stream, CommandEnvironment environment)
    {
        if (environment is not CommandEnvironment.Development)
            return [];

        Memory<byte>[] values = new Memory<byte>[
            ReadPayloadBoundedCollectionCount(stream, name: "development byte array")
        ];

        for (int index = 0; index < values.Length; index++)
            values[index] = stream.ReadVariableIntByteArray();

        return values;
    }

    private static int ReadPayloadBoundedCollectionCount(MessageStream stream, string name)
    {
        long remainingPayloadLength = stream.Length - stream.Position;
        int maximum = int.CreateTruncating(Math.Min(int.MaxValue, remainingPayloadLength));

        return ReadCollectionCount(stream, maximum, name);
    }
}
