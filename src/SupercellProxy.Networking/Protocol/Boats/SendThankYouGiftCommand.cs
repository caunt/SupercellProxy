using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Preserves the native SendThankYouGiftCommand wire contract.</summary>
/// <param name="GiftKind">The GiftKind value carried by the native command.</param>
/// <param name="TargetIndex">The TargetIndex value carried by the native command.</param>
/// <param name="HelperIdentifier">The HelperIdentifier value carried by the native command.</param>
/// <param name="ExecutionPhaseCounter"></param>
/// <param name="DebugData0"></param>
/// <param name="DebugData1"></param>
public sealed record SendThankYouGiftCommand(
    int GiftKind,
    int TargetIndex,
    LongIdentifier HelperIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>Gets the native command type.</summary>
    public override int Type => CommandRegistry.SendThankYouGiftCommandType;

    /// <summary>Decodes the body followed by its command header.</summary>
    public static SendThankYouGiftCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int giftKind = stream.ReadVariableInt();
        int targetIndex = stream.ReadVariableInt();
        LongIdentifier helperIdentifier = stream.ReadLongIdentifier();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new(giftKind, targetIndex, helperIdentifier, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <summary>Encodes the unchanged native field order.</summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(GiftKind);
        stream.WriteVariableInt(TargetIndex);
        stream.WriteLongIdentifier(HelperIdentifier);
        EncodeCommand(stream, environment);
    }
}
