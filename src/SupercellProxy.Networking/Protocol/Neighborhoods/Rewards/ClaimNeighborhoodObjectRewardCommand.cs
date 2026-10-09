using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Claims a free reward tier from the neighborhood event.</summary>
public sealed record ClaimNeighborhoodObjectRewardCommand(int TierIndex) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ClaimNeighborhoodObjectRewardCommandType;

    /// <summary>Decodes the zero-based reward-tier index.</summary>
    public static ClaimNeighborhoodObjectRewardCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(TierIndex);
    }
}
