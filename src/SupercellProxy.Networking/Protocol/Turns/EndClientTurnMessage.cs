using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Commands.Registry;
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
    /// Most commands one turn may carry; the decoder rejects more as corrupt data.
    /// </summary>
    public const int MaximumCommandCount = 1024;

    /// <summary>
    /// How many values <see cref="SubChecksums"/> holds in every turn.
    /// </summary>
    public const int SubChecksumCount = 8;

    /// <inheritdoc />
    /// <remarks>Commands, debug data and development byte arrays follow the stream's command environment when read and the message's own environment when written.</remarks>
    public static ICodec<EndClientTurnMessage> Codec { get; } = Codecs.Complete(Codecs.Create(Read, Write), $"Unexpected trailing {nameof(EndClientTurnMessage)} data.");

    /// <summary>
    /// The turn checksum, the first field of the turn header.
    /// </summary>
    public int Checksum { get; init; }

    /// <summary>
    /// The player's actions in this turn, each stamped with the sub-tick it happened at, in order.
    /// </summary>
    public Memory<Command> Commands { get; init; }

    /// <summary>
    /// Diagnostic blocks, one per command, that only non-production builds send; empty in production.
    /// </summary>
    public Memory<CommandData> DebugCommandData { get; init; }

    /// <summary>
    /// Extra byte arrays that only development builds send; empty in production.
    /// </summary>
    public Memory<ReadOnlyMemory<byte>> DevelopmentByteArrays { get; init; }

    /// <summary>
    /// The <see cref="SubChecksumCount"/> sub-checksums the turn header sends after the sub-tick.
    /// </summary>
    public Memory<int> SubChecksums { get; init; } = new int[SubChecksumCount];

    /// <summary>
    /// Build environment whose layout the turn is written in; it decides whether the debug and development blocks are present.
    /// </summary>
    public CommandEnvironment Environment { get; init; } = CommandEnvironment.Production;

    /// <summary>
    /// Game sub-tick the turn ends at, sent in the turn header after the checksum.
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
