using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// <para>One collected-chicken or delivered-sanctuary-animal contribution, saved natively in <c>ChickenLogEntries</c>; type 1 is chickens and 2 animals.</para>
/// </summary>
/// <param name="AvatarId">Account id of the player who made the contribution; <see langword="null"/> when the wire marks it absent.</param>
/// <param name="ChickenCount">How many chickens or animals the contribution brought in.</param>
/// <param name="CollectTimeSeconds">When the contribution was made, as a Unix timestamp in seconds.</param>
/// <param name="Type">Kind of contribution: 1 for collected chickens, 2 for delivered sanctuary animals.</param>
public sealed record MapGameChickenLogEntry(LongId? AvatarId, int ChickenCount, int CollectTimeSeconds, int Type) : ICodecProvider<MapGameChickenLogEntry>
{
    /// <inheritdoc />
    public static ICodec<MapGameChickenLogEntry> Codec { get; } = Codecs.Composite(Codecs.OptionalLongId,
        static value => value.AvatarId,
        Codecs.VarInt,
        static value => value.ChickenCount,
        Codecs.VarInt,
        static value => value.CollectTimeSeconds,
        Codecs.VarInt,
        static value => value.Type,
        static (avatarId, chickenCount, collectTimeSeconds, type) => new MapGameChickenLogEntry(avatarId, chickenCount, collectTimeSeconds, type));
}
