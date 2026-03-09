using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

public record ClientCommand(int Type, Memory<byte> Data);

/// <summary>
/// Build EndClientTurnMessage (19132) payload
// 
// Format(from Ghidra vtable[2] decompilation of FUN_10060c660) of v1.69.89:
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
    public int Checksum { get; init; }
    public int SubTick { get; init; }
    public Memory<int> SubChecksums { get; init; } = new int[8];
    public Memory<ClientCommand> Commands { get; init; }
    public Memory<byte> Fallback { get; init; }

    public static EndClientTurnMessage Create(MessageContainer messageContainer)
    {
        var position = messageContainer.Payload.Position;
        var checksum = messageContainer.Payload.ReadVarInt();
        var subTick = messageContainer.Payload.ReadVarInt();
        var subChecksums = new int[8];

        for (int i = 0; i < subChecksums.Length; i++)
            subChecksums[i] = messageContainer.Payload.ReadVarInt();

        var commands = new ClientCommand[messageContainer.Payload.ReadVarInt()];

        for (int i = 0; i < commands.Length; i++)
        {
            var commandType = messageContainer.Payload.ReadVarInt();

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
                    _ => -1
                };

                if (commandLength is -1)
                {
                    Console.WriteLine($"[{DateTime.Now:T}] We do not know the size of the {nameof(EndClientTurnMessage)} command data yet. Command count: {commands.Length}, Position: {messageContainer.Payload.Position}, Length: {messageContainer.Payload.Length}");
                    messageContainer.Payload.Position = position;
                    return new EndClientTurnMessage { Fallback = messageContainer.Payload.ReadToEnd() };
                }

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

        if (Fallback.Length > 0)
        {
            supercellStream.Write(Fallback.Span);
            return new MessageContainer(id, messageVersion, supercellStream);
        }

        supercellStream.WriteVarInt(Checksum);
        supercellStream.WriteVarInt(SubTick);

        foreach (var subChecksum in SubChecksums.Span)
            supercellStream.WriteVarInt(subChecksum);

        supercellStream.WriteVarInt(Commands.Length);

        foreach (var clientCommand in Commands.Span)
        {
            supercellStream.WriteVarInt(clientCommand.Type);
            supercellStream.Write(clientCommand.Data.Span);
        }

        return new MessageContainer(id, messageVersion, supercellStream);
    }
}
