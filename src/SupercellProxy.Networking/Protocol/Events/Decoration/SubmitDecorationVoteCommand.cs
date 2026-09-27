using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Votes for one member of a received decoration-canvas candidate pair.</summary>
public sealed record SubmitDecorationVoteCommand(
    LongIdentifier? CandidateIdentifier,
    int EventIdentifier,
    int EventVariantIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>Gets the native command type.</summary>
    public override int Type => CommandRegistry.SubmitDecorationVoteCommandType;

    /// <summary>Decodes the candidate identity and event identity.</summary>
    public static SubmitDecorationVoteCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        LongIdentifier? candidate = stream.ReadBoolean()
            ? new LongIdentifier(stream.ReadInt32(), stream.ReadInt32())
            : null;

        return new SubmitDecorationVoteCommand(
            candidate,
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            fields.ExecutionPhaseCounter,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    /// <summary>Encodes the candidate identity and event identity.</summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteBoolean(CandidateIdentifier is not null);

        if (CandidateIdentifier is { } candidate)
        {
            stream.WriteInt32(candidate.HighInt32);
            stream.WriteInt32(candidate.LowInt32);
        }

        stream.WriteVariableInt(EventIdentifier);
        stream.WriteVariableInt(EventVariantIdentifier);
    }
}
