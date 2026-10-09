using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>Carries another player's Town, its owner, and the visiting player.</summary>
public sealed record OtherTownDataMessage : OtherHomeDataMessage
{
    private OtherTownDataMessage(OtherHomeDataMessage data) : base(data)
    {
    }

    /// <summary>Decodes the shared visited-home payload for a Town visit.</summary>
    public static new OtherTownDataMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new OtherTownDataMessage(OtherHomeDataMessage.Decode(stream));
    }
}
