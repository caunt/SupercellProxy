using SupercellProxy.Networking.Protocol.Authentication;

namespace SupercellProxy.Networking.Cryptography;

/// <summary>One published game version and its matching server key.</summary>
internal sealed record HayDayServerKey(Version GameVersion, int? KeyVersion, byte[] PublicKey) : IServerPublicKeySource
{
    public ValueTask<byte[]> GetServerPublicKeyAsync(CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(PublicKey);
    }

    public ProtocolConfiguration ToProtocol()
    {
        return new ProtocolConfiguration(
            GameVersion.Major,
            GameVersion.Minor,
            GameVersion.Build,
            ProtocolConfiguration.WireProtocolVersion,
            KeyVersion ?? throw new InvalidDataException($"KEYS.md has no key version for Hay Day {GameVersion}.")
        );
    }
}
