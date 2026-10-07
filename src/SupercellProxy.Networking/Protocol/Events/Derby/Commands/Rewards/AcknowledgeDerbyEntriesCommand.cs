using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Protocol.Events.Derby.Rewards;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Acknowledges leaderboard entries and optionally the derby start presentation.</summary>
public sealed record AcknowledgeDerbyEntriesCommand(DerbyEntryState[] Entries, int Mode) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.AcknowledgeDerbyEntriesCommandType;
    /// <summary>Decodes the ordered entry acknowledgements and presentation mode.</summary>
    public static AcknowledgeDerbyEntriesCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadArray(DerbyEntryState.Decode), stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteArray<DerbyEntryState>(Entries, static (writer, entry) => entry.Encode(writer));
        stream.WriteVarInt(Mode);
    }
}
