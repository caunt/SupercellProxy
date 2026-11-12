using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Events.Derby.Rewards;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Acknowledges leaderboard entries and optionally the derby start presentation.</summary>
/// <param name="Entries">The leaderboard entries whose scores and order the player has now been shown, written first with a count prefix.</param>
/// <param name="Mode">1 to also acknowledge the derby start presentation; any other value acknowledges the entries only.</param>
public sealed record AcknowledgeDerbyEntriesCommand(DerbyEntryState[] Entries, int Mode) : CodecCommand<AcknowledgeDerbyEntriesCommand>, ICommandCodecProvider<AcknowledgeDerbyEntriesCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AcknowledgeDerbyEntriesCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.Array(DerbyEntryState.Codec, CollectionCount.Bounded),
        static command => command.Entries,
        Codecs.VarInt,
        static command => command.Mode,
        static (entries, mode) => new AcknowledgeDerbyEntriesCommand(entries, mode)));
}
