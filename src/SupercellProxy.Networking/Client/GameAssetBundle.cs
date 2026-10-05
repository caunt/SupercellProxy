using SupercellProxy.Networking.Assets;

namespace SupercellProxy.Networking.Client;

/// <summary>Named result returned by DiscoverAssetsAsync.</summary>
internal readonly record struct GameAssetBundle(GameAssetFingerprint Fingerprint, GameAsset[] Resources);
