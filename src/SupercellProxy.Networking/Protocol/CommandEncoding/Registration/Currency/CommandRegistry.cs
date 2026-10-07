namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

public static partial class CommandRegistry
{
    /// <summary>Credits or debits the player's diamonds from a server command.</summary>
    public const int AdjustDiamondsServerCommandType = 378;
    /// <summary>Debits the player's diamonds from a server command.</summary>
    public const int SpendDiamondsServerCommandType = 379;
}
