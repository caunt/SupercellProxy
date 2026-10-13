using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Turns;

/// <summary>
/// <para>EndClientTurnMessage (10224) wire representation.</para>
/// </summary>
public sealed record EndClientTurnMessage : CodecMessage<EndClientTurnMessage>, ICodecProvider<EndClientTurnMessage>
{
    /// <summary>
    /// Defines the <c language="csharp">MaxCommandCount</c> value.
    /// </summary>
    public const int MaximumCommandCount = 1024;

    /// <summary>
    /// Defines the <c language="csharp">SubChecksumCount</c> value.
    /// </summary>
    public const int SubChecksumCount = 8;

    /// <inheritdoc />
    /// <remarks>Commands, debug data and development byte arrays follow the stream's command environment when read and the message's own environment when written.</remarks>
    public static ICodec<EndClientTurnMessage> Codec { get; } = Codecs.Complete(Codecs.Create(Read, Write), $"Unexpected trailing {nameof(EndClientTurnMessage)} data.");

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
    public Memory<ReadOnlyMemory<byte>> DevelopmentByteArrays { get; init; }

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

    private static ICodec<CommandData[]> DebugCommandDataCodec => field ??= Codecs.Array(CommandData.Codec);

    private static ICodec<ReadOnlyMemory<byte>[]> DevelopmentByteArraysCodec => field ??= Codecs.Array(Codecs.VarIntByteArray);

    private static EndClientTurnMessage Read(MessageStream stream)
    {
        ClientTurnHeader header = ClientTurnHeader.Codec.Read(stream);
        Command[] commands = new Command[header.CommandCount];

        for (int index = 0; index < commands.Length; index++)
            commands[index] = CommandRegistry.Codec.Read(stream);

        CommandEnvironment environment = stream.CommandEnvironment;

        return new EndClientTurnMessage
        {
            Checksum = header.Checksum,
            SubTick = header.SubTick,
            SubChecksums = header.SubChecksums,
            Commands = commands,
            Environment = environment,
            DebugCommandData = environment is CommandEnvironment.Production ? [] : DebugCommandDataCodec.Read(stream),
            DevelopmentByteArrays = environment is CommandEnvironment.Development ? DevelopmentByteArraysCodec.Read(stream) : [],
        };
    }

    private static void Write(ProtocolEncoder encoder, EndClientTurnMessage message)
    {
        if (message.SubChecksums.Length != SubChecksumCount)
            throw new InvalidDataException($"EndClientTurnMessage must contain exactly {SubChecksumCount} sub-checksums.");

        if (message.Commands.Length > MaximumCommandCount)
            throw new InvalidDataException($"Invalid command count: {message.Commands.Length}.");

        bool hasDiagnostics = message.DebugCommandData.Length > 0 || message.DevelopmentByteArrays.Length > 0;

        if (message.Environment is CommandEnvironment.Production && hasDiagnostics)
            throw new InvalidDataException(message: "Production EndClientTurnMessage cannot contain diagnostic collections.");

        if (message.Environment is not CommandEnvironment.Development && message.DevelopmentByteArrays.Length > 0)
            throw new InvalidDataException(message: "Development byte arrays are only encoded in the development environment.");

        CommandEnvironment streamEnvironment = encoder.CommandEnvironment;
        encoder.CommandEnvironment = message.Environment;

        try
        {
            ClientTurnHeader.Codec.Write(encoder, new ClientTurnHeader(message.Checksum, message.SubTick, message.SubChecksums.ToArray(), message.Commands.Length));

            foreach (Command command in message.Commands.Span)
                CommandRegistry.Codec.Write(encoder, command);

            if (message.Environment is not CommandEnvironment.Production)
                DebugCommandDataCodec.Write(encoder, message.DebugCommandData.ToArray());

            if (message.Environment is CommandEnvironment.Development)
                DevelopmentByteArraysCodec.Write(encoder, message.DevelopmentByteArrays.ToArray());
        }
        finally
        {
            encoder.CommandEnvironment = streamEnvironment;
        }
    }
}
