using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>Claims the mystery-box prize for one completed Valley quest-progression milestone.</summary>
public sealed record ClaimMapGameQuestProgressionPrizeCommand(int ProgressionPrizeGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ClaimMapGameQuestProgressionPrizeCommandType;

    /// <summary>Decodes the progression data reference.</summary>
    public static ClaimMapGameQuestProgressionPrizeCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int id = stream.ReadVarInt();

        return new ClaimMapGameQuestProgressionPrizeCommand(id);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(ProgressionPrizeGlobalId);
    }
}
