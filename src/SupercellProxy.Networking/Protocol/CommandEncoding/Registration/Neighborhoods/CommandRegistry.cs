namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

public static partial class CommandRegistry
{
    /// <summary>Claims a Neighborhood Nurture reward tier.</summary>
    public const int ClaimNeighborhoodObjectRewardCommandType = 706;
    /// <summary>Claims a Neighborhood Nurture milestone decoration.</summary>
    public const int ClaimNeighborhoodObjectVisualRewardCommandType = 708;
    /// <summary>Requests automatic neighborhood creation from the player's farm.</summary>
    public const int RequestAutomaticNeighborhoodCreationCommandType = 267;
}
