using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Acknowledges the introduction for an available chapter of a chain offer.</summary>
public sealed record AcknowledgeChainOfferChapterIntroCommand(int ChapterIndex, int EventId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.AcknowledgeChainOfferChapterIntroCommandType;

    /// <summary>Decodes the chapter index followed by the event identifier.</summary>
    public static AcknowledgeChainOfferChapterIntroCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(ChapterIndex);
        stream.WriteVarInt(EventId);
    }
}
