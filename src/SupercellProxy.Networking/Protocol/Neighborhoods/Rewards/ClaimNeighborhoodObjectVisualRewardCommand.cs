using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Claims the decoration for a one-based Neighborhood Nurture visual milestone.</summary>
public sealed record ClaimNeighborhoodObjectVisualRewardCommand(int VisualLevel) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ClaimNeighborhoodObjectVisualRewardCommandType;

    /// <summary>Decodes the one-based visual milestone.</summary>
    public static ClaimNeighborhoodObjectVisualRewardCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(VisualLevel);
    }
}
