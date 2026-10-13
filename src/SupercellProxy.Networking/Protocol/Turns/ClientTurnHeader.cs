using System.Globalization;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Turns;

/// <summary>The leading fields of an <see cref="EndClientTurnMessage"/>: checksums, sub-tick and command count.</summary>
/// <param name="Checksum">The turn checksum.</param>
/// <param name="SubTick">The sub-tick the turn ends on.</param>
/// <param name="SubChecksums">The eight sub-checksums.</param>
/// <param name="CommandCount">The number of commands that follow.</param>
public readonly record struct ClientTurnHeader(int Checksum, int SubTick, int[] SubChecksums, int CommandCount) : ICodecProvider<ClientTurnHeader>
{
    /// <inheritdoc />
    public static ICodec<ClientTurnHeader> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static header => header.Checksum,
        Codecs.VarInt,
        static header => header.SubTick,
        Codecs.FixedArray(Codecs.VarInt, EndClientTurnMessage.SubChecksumCount),
        static header => header.SubChecksums,
        Codecs.Map(
            Codecs.VarInt,
            static count => uint.CreateTruncating(count) > EndClientTurnMessage.MaximumCommandCount
                ? throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid command count: {count}."))
                : count,
            static count => count
        ),
        static header => header.CommandCount,
        static (checksum, subTick, subChecksums, commandCount) => new ClientTurnHeader(checksum, subTick, subChecksums, commandCount)
    );
}
