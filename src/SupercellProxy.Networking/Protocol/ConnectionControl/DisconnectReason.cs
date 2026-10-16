namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>
/// Why the server ended a session, as its disconnect message reports it. Values not listed here can arrive and are kept as numbers.
/// </summary>
public enum DisconnectReason
{
    /// <summary>
    /// No specific reason given.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The same account logged in on another connection, which took over the session; reconnecting would kick that one out in turn.
    /// </summary>
    SessionReplaced = 1,
}
