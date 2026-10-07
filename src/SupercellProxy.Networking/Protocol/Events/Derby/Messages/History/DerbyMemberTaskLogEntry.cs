using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.History;

/// <summary>Preserves one player's derby task-log counters; bingo lists use the count and points for bingo progress.</summary>
public sealed record DerbyMemberTaskLogEntry(LongId PlayerId, string? PlayerName, int PlayerLevel, int TaskCount, int Points, int TaskLimit, int TrashedTaskCount)
{
    /// <summary>Decodes the player identity and native log values.</summary>
    public static DerbyMemberTaskLogEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(
            stream.ReadLongId(),
            stream.ReadOptionalString(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt()
        );
    }

    /// <summary>Encodes the player identity and native log values.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteLongId(PlayerId);
        stream.WriteOptionalString(PlayerName);
        stream.WriteVarInt(PlayerLevel);
        stream.WriteVarInt(TaskCount);
        stream.WriteVarInt(Points);
        stream.WriteVarInt(TaskLimit);
        stream.WriteVarInt(TrashedTaskCount);
    }

    /// <summary>Omits player details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DerbyMemberTaskLogEntry);
    }
}
