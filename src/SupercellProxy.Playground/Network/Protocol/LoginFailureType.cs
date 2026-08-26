namespace SupercellProxy.Playground.Network.Protocol;

/// <summary>
/// Defines the supported <c>LoginFailureType</c> values.
/// </summary>
public enum LoginFailureType
{
    /// <summary>
    /// Identifies the <c>InvalidCredentials</c> option.
    /// </summary>
    InvalidCredentials = 2,

    /// <summary>
    /// Identifies the <c>OutdatedContent</c> option.
    /// </summary>
    OutdatedContent = 7,

    /// <summary>
    /// Identifies the <c>OutdatedVersion</c> option.
    /// </summary>
    OutdatedVersion = 8,

    /// <summary>
    /// Identifies the <c>Unknown1</c> option.
    /// </summary>
    Unknown1 = 9,

    /// <summary>
    /// Identifies the <c>Maintenance</c> option.
    /// </summary>
    Maintenance = 10,

    /// <summary>
    /// Identifies the <c>TemporarilyBanned</c> option.
    /// </summary>
    TemporarilyBanned = 11,

    /// <summary>
    /// Identifies the <c>Redirection</c> option.
    /// </summary>
    Redirection = 12,

    /// <summary>
    /// Identifies the <c>Locked</c> option.
    /// </summary>
    Locked = 13,

    /// <summary>
    /// Identifies the <c>InvalidToken</c> option.
    /// </summary>
    InvalidToken = 15,

    /// <summary>
    /// Identifies the <c>AccountNotBound</c> option.
    /// </summary>
    AccountNotBound = 16,
}
