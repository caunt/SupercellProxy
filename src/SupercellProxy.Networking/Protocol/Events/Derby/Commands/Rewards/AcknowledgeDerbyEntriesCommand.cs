using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Events.Derby.Rewards;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Acknowledges leaderboard entries and optionally the derby start presentation.</summary>
public sealed record AcknowledgeDerbyEntriesCommand(DerbyEntryState[] Entries, int Mode) : CodecCommand<AcknowledgeDerbyEntriesCommand>, ICommandCodecProvider<AcknowledgeDerbyEntriesCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AcknowledgeDerbyEntriesCommand> Codec { get; } = Codecs.Command(
        type: 176,
        Codecs.Composite(
            Codecs.Array(DerbyEntryState.Codec, CollectionCount.Bounded),
            static command => command.Entries,
            Codecs.VarInt,
            static command => command.Mode,
            static (entries, mode) => new AcknowledgeDerbyEntriesCommand(entries, mode)
        )
    );
}
