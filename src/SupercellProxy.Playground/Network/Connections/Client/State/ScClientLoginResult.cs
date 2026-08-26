using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Network.Messages.Clientbound;

namespace SupercellProxy.Playground.Network.Connections.Client;

internal sealed record ScClientLoginResult(
    LoginOkMessage LoginOkMessage,
    GameAssetFingerprint? Fingerprint,
    GameAsset[] Resources
)
{
    public ScClientLoginResult(LoginOkMessage loginOkMessage)
        : this(loginOkMessage, Fingerprint: null, []) { }
}
