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
    /// Gets or sets the turn checksum, the first field of the turn header.
    /// </summary>
    /// <value>Signed 32-bit value written as a variable-length integer; the server compares it against its own result for the same turn and answers a mismatch with an <see cref="OutOfSyncMessage"/>. 0 by default.</value>
    public int Checksum { get; init; }

    /// <summary>
    /// Gets or sets the player's actions in this turn, each stamped with the sub-tick it happened at, in order.
    /// </summary>
    /// <value>Commands written after the header in execution order, at most <see cref="MaximumCommandCount"/>; empty by default, which makes a turn that only advances time.</value>
    public Memory<Command> Commands { get; init; }

    /// <summary>
    /// Gets or sets diagnostic blocks, one per command, that only non-production builds send; empty in production.
    /// </summary>
    /// <value>Array written after the commands whenever <see cref="Environment"/> is not production; writing a non-empty value with the production environment throws <see cref="InvalidDataException"/>.</value>
    public Memory<CommandData> DebugCommandData { get; init; }

    /// <summary>
    /// Gets or sets extra byte arrays that only development builds send; empty in production.
    /// </summary>
    /// <value>Length-prefixed byte arrays written last, only in the development environment; any other environment rejects a non-empty value when the message is written.</value>
    public Memory<ReadOnlyMemory<byte>> DevelopmentByteArrays { get; init; }

    /// <summary>
    /// Gets or sets the <see cref="SubChecksumCount"/> sub-checksums the turn header sends after the sub-tick.
    /// </summary>
    /// <value>Exactly <see cref="SubChecksumCount"/> variable-length integers with no count prefix; all zeros by default, and any other length fails when the message is written.</value>
    public Memory<int> SubChecksums { get; init; } = new int[SubChecksumCount];

    /// <summary>
    /// Gets or sets the build environment whose layout the turn is written in; it decides whether the debug and development blocks are present.
    /// </summary>
    /// <value>Taken from the stream's command environment when the message is read, and swapped in for the encoder's environment while it is written; <see cref="CommandEnvironment.Production"/> by default.</value>
    public CommandEnvironment Environment { get; init; } = CommandEnvironment.Production;

    /// <summary>
    /// Gets or sets the game sub-tick the turn ends at, sent in the turn header after the checksum.
    /// </summary>
    /// <value>Sub-tick counter of the client's game clock at the end of the turn, written as a variable-length integer; 0 by default.</value>
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
