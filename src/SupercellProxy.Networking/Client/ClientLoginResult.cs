using SupercellProxy.Networking.Assets;
using SupercellProxy.Networking.Protocol.Authentication;


namespace SupercellProxy.Networking.Client;

/// <summary>
/// What a successful login hands back: the server's login confirmation and the asset bundle the session was opened against.
/// </summary>
/// <param name="LoginOkMessage">The server's login confirmation: account and home ids, pass token, server build and time, content URLs.</param>
/// <param name="Fingerprint">Manifest of the asset bundle used for the login; null when the login did not involve assets.</param>
/// <param name="Resources">The bundle's downloaded files; empty when the login did not involve assets.</param>
public sealed record ClientLoginResult(LoginOkMessage LoginOkMessage, GameAssetFingerprint? Fingerprint, GameAsset[] Resources)
{
    /// <summary>
    /// Wraps a login confirmation that came without asset information.
    /// </summary>
    /// <param name="loginOkMessage">The server's login confirmation.</param>
    public ClientLoginResult(LoginOkMessage loginOkMessage)
        : this(loginOkMessage, Fingerprint: null, []) { }
}
