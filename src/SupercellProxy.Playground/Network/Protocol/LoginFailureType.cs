namespace SupercellProxy.Playground.Network.Protocol;

/// <summary>
/// Defines the supported <c language="csharp">LoginFailureType</c> values.
/// </summary>
internal enum LoginFailureType
{
    /// <summary>
    /// Identifies the <c language="csharp">InvalidCredentials</c> option.
    /// </summary>
    InvalidCredentials = 2,

    /// <summary>
    /// Identifies the <c language="csharp">OutdatedContent</c> option.
    /// </summary>
    OutdatedContent = 7,

    /// <summary>
    /// Identifies the <c language="csharp">OutdatedVersion</c> option.
    /// </summary>
    OutdatedVersion = 8,

    /// <summary>
    /// Identifies the <c language="csharp">Unknown1</c> option.
    /// </summary>
    Unknown1 = 9,

    /// <summary>
    /// Identifies the <c language="csharp">Maintenance</c> option.
    /// </summary>
    Maintenance = 10,

    /// <summary>
    /// Identifies the <c language="csharp">TemporarilyBanned</c> option.
    /// </summary>
    TemporarilyBanned = 11,

    /// <summary>
    /// Identifies the <c language="csharp">Redirection</c> option.
    /// </summary>
    Redirection = 12,

    /// <summary>
    /// Identifies the <c language="csharp">Locked</c> option.
    /// </summary>
    Locked = 13,

    /// <summary>
    /// Identifies the <c language="csharp">InvalidToken</c> option.
    /// </summary>
    InvalidToken = 15,

    /// <summary>
    /// Identifies the <c language="csharp">AccountNotBound</c> option.
    /// </summary>
    AccountNotBound = 16,
}
