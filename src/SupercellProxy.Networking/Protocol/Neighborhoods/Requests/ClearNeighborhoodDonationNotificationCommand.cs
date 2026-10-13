using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Clears the pending neighborhood donation-received notification.</summary>
public sealed record ClearNeighborhoodDonationNotificationCommand : CodecCommand<ClearNeighborhoodDonationNotificationCommand>, ICommandCodecProvider<ClearNeighborhoodDonationNotificationCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClearNeighborhoodDonationNotificationCommand> Codec { get; } = Codecs.Command(type: 524, Codecs.Unit(static () => new ClearNeighborhoodDonationNotificationCommand()));
}
