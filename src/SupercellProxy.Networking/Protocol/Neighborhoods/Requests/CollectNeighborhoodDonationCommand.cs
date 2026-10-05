using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Collects a received donation of the selected good; zero selects the first donation.</summary>
public sealed record CollectNeighborhoodDonationCommand(int ItemGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CollectNeighborhoodDonationCommandType;

    /// <summary>Decodes the command fields followed by the donated item id.</summary>
    public static CollectNeighborhoodDonationCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(ItemGlobalId);
    }
}
