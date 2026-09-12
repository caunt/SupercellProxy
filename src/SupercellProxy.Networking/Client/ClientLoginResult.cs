using SupercellProxy.Networking.Assets;
using SupercellProxy.Networking.Protocol.Authentication;


namespace SupercellProxy.Networking.Client;

/// <summary>
/// Defines the Sc Client Login Result contract.
/// </summary>
/// <summary>
/// Defines the Login Ok Message contract.
/// </summary>
/// <summary>
/// Defines the Fingerprint contract.
/// </summary>
/// <summary>
/// Defines the Resources contract.
/// </summary>
public sealed record ClientLoginResult(LoginOkMessage LoginOkMessage, GameAssetFingerprint? Fingerprint, GameAsset[] Resources)
{
    /// <summary>
    /// Provides the Sc Client Login Result value or operation.
    /// </summary>
    public ClientLoginResult(LoginOkMessage loginOkMessage)
        : this(loginOkMessage, Fingerprint: null, []) { }
}
