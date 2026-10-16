using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;

/// <summary>One reaction on a stream entry, as StreamEntryReactionAddedMessage adds them.</summary>
/// <param name="Reaction">Which reaction was given; its values are not identified.</param>
/// <param name="AvatarId">The reacting avatar.</param>
public sealed record StreamEntryReaction(int Reaction, LongId? AvatarId) : ICodecProvider<StreamEntryReaction>
{
    /// <inheritdoc />
    public static ICodec<StreamEntryReaction> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Reaction,
        Codecs.OptionalLongId,
        static value => value.AvatarId,
        static (reaction, avatarId) => new StreamEntryReaction(reaction, avatarId)
    );
}
