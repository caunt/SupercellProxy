namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

public static partial class CommandRegistry
{
    /// <summary>Assigns or clears the player's current derby instance.</summary>
    public const int AssignDerbyServerCommandType = 167;
    /// <summary>Reports the result of a derby request.</summary>
    public const int DerbyNotificationServerCommandType = 231;
    /// <summary>Reports a derby season notification.</summary>
    public const int DerbySeasonNotificationServerCommandType = 323;
    /// <summary>Ends a derby and retains its reward state.</summary>
    public const int EndDerbyServerCommandType = 168;
    /// <summary>Assigns the player to a neighborhood and resets the prior derby assignment.</summary>
    public const int JoinNeighborhoodServerCommandType = 134;
    /// <summary>Removes the player from the specified neighborhood.</summary>
    public const int LeaveNeighborhoodServerCommandType = 135;
    /// <summary>Replaces the current derby league index.</summary>
    public const int SetCurrentDerbyLeagueServerCommandType = 184;
    /// <summary>Updates the player's derby participation preference.</summary>
    public const int SetDerbyParticipationServerCommandType = 211;
    /// <summary>Updates player, neighborhood, bingo and bunny progress.</summary>
    public const int UpdateDerbyPointsServerCommandType = 174;
}
