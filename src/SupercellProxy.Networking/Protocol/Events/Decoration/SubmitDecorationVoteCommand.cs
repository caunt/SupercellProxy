using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Votes for one member of a received decoration-canvas candidate pair.</summary>
public sealed record SubmitDecorationVoteCommand(LongId? CandidateId, int EventId, int EventVariantId) : Command
{
    /// <summary>Gets the native command type.</summary>
    public override int Type => CommandRegistry.SubmitDecorationVoteCommandType;

    /// <summary>Decodes the candidate identity and event identity.</summary>
    public static SubmitDecorationVoteCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        LongId? candidate = stream.ReadBoolean()
            ? new LongId(stream.ReadInt32(), stream.ReadInt32())
            : null;

        return new SubmitDecorationVoteCommand(candidate, stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <summary>Encodes the candidate identity and event identity.</summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteBoolean(CandidateId is not null);

        if (CandidateId is { } candidate)
        {
            stream.WriteInt32(candidate.HighInt32);
            stream.WriteInt32(candidate.LowInt32);
        }

        stream.WriteVarInt(EventId);
        stream.WriteVarInt(EventVariantId);
    }
}
