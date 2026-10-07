using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Preserves native reward-choice indices for ordinary, bingo and bunny thresholds.</summary>
public sealed record DerbyRewardSelection(int[] ThresholdIndices, int[] BingoIndices, int[] BunnyIndices)
{
    /// <summary>Decodes the three ordered index arrays.</summary>
    public static DerbyRewardSelection Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(
            stream.ReadArray(static reader => reader.ReadVarInt()),
            stream.ReadArray(static reader => reader.ReadVarInt()),
            stream.ReadArray(static reader => reader.ReadVarInt())
        );
    }

    /// <summary>Encodes the three ordered index arrays.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteArray<int>(ThresholdIndices, static (writer, value) => writer.WriteVarInt(value));
        stream.WriteArray<int>(BingoIndices, static (writer, value) => writer.WriteVarInt(value));
        stream.WriteArray<int>(BunnyIndices, static (writer, value) => writer.WriteVarInt(value));
    }
}
