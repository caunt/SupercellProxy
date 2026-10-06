using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Discards a booster awaiting a keep, swap, or discard decision.</summary>
public sealed record DiscardPendingBoosterCommand(int BoosterDataGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.DiscardPendingBoosterCommandType;

    /// <summary>Decodes the pending booster's data id.</summary>
    public static DiscardPendingBoosterCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new DiscardPendingBoosterCommand(stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(BoosterDataGlobalId);
    }
}
