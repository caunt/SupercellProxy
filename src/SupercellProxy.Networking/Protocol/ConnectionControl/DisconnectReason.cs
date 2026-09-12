namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>
/// Defines the Disconnect Reason contract.
/// </summary>
public enum DisconnectReason
{
    /// <summary>
    /// Identifies the Unknown wire value.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Identifies the Session Replaced wire value.
    /// </summary>
    SessionReplaced = 1,
}
