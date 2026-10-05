using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Preserves the native SendThankYouGiftCommand wire contract.</summary>
/// <param name="GiftKind">The GiftKind value carried by the native command.</param>
/// <param name="TargetIndex">The TargetIndex value carried by the native command.</param>
/// <param name="HelperId">The HelperId value carried by the native command.</param>
public sealed record SendThankYouGiftCommand(int GiftKind, int TargetIndex, LongId HelperId) : Command
{
    /// <summary>Gets the native command type.</summary>
    public override int Type => CommandRegistry.SendThankYouGiftCommandType;

    /// <summary>Decodes the command-specific fields.</summary>
    public static SendThankYouGiftCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int giftKind = stream.ReadVarInt();
        int targetIndex = stream.ReadVarInt();
        LongId helperId = stream.ReadLongId();

        return new(giftKind, targetIndex, helperId);
    }

    /// <summary>Encodes the unchanged native field order.</summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(GiftKind);
        stream.WriteVarInt(TargetIndex);
        stream.WriteLongId(HelperId);
    }
}
