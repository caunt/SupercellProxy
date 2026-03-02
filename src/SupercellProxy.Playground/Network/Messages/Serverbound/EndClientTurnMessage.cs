using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

public record ClientCommand(long Type, Memory<byte> Data);

/// <summary>
/// Build EndClientTurnMessage (19132) payload
// 
// Format(from Ghidra vtable[2] decompilation of FUN_10060c660) :
//       writeVarInt(checksum)       # main state checksum (offset 0xCC)
//       writeVarInt(subtick)        # sub-tick counter (offset 0xC8)
//       writeVarInt(sub_chk[0..7])  # 8 sub-checksums (offsets 0x90-0xAC)
//       writeVarInt(commandCount)   # number of commands
//       [for each command:
//         writeVarInt(cmd_type)     # command type ID
//         bytes(cmd_data)           # command-specific encode
//       ]
// Note: DAT_101ac1d88=3 in this binary, so conditional sections are skipped
/// </summary>
public record EndClientTurnMessage : IMessage
{
    public long Checksum { get; init; }
    public long SubTick { get; init; }
    public Memory<long> SubChecksums { get; init; } = new long[8];
    public Memory<ClientCommand> Commands { get; init; }

    public static EndClientTurnMessage Create(MessageContainer messageContainer)
    {
        var checksum = messageContainer.Payload.ReadVarInt64();
        var subTick = messageContainer.Payload.ReadVarInt64();
        var subChecksums = new long[8];

        for (int i = 0; i < subChecksums.Length; i++)
            subChecksums[i] = messageContainer.Payload.ReadVarInt64();

        var commands = new ClientCommand[messageContainer.Payload.ReadVarInt64()];

        for (int i = 0; i < commands.Length; i++)
        {
            var commandType = messageContainer.Payload.ReadVarInt64();

            if (commands.Length is 1)
            {
                commands[i] = new ClientCommand(commandType, messageContainer.Payload.ReadToEnd());
            }
            else
            {
                var commandLength = commandType switch
                {
                    210 => 11,
                    355 => 5,
                    672 => 11,
                    _ => throw new NotSupportedException($"We do not know the size of the command data yet. Command count: {commands.Length}, Position: {messageContainer.Payload.Position}, Length: {messageContainer.Payload.Length}")
                };

                var commandData = new byte[commandLength];
                messageContainer.Payload.ReadExactly(commandData);

                commands[i] = new ClientCommand(commandType, commandData);
            }
        }

        return new EndClientTurnMessage
        {
            Checksum = checksum,
            SubTick = subTick,
            SubChecksums = subChecksums,
            Commands = commands
        };
    }

    public MessageContainer ToContainer(ushort id, ushort messageVersion = 5213)
    {
        var supercellStream = SupercellStream.Create();

        supercellStream.WriteVarInt64(Checksum);
        supercellStream.WriteVarInt64(SubTick);

        foreach (var subChecksum in SubChecksums.Span)
            supercellStream.WriteVarInt64(subChecksum);

        supercellStream.WriteVarInt64(Commands.Length);

        foreach (var clientCommand in Commands.Span)
        {
            supercellStream.WriteVarInt64(clientCommand.Type);
            supercellStream.Write(clientCommand.Data.Span);
        }

        return new MessageContainer(id, messageVersion, supercellStream);
    }
}
