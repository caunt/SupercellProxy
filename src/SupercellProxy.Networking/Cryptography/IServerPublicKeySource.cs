namespace SupercellProxy.Networking.Cryptography;

/// <summary>Supplies the public key used to authenticate an upstream server.</summary>
public interface IServerPublicKeySource
{
    /// <summary>Loads the current server public key.</summary>
    ValueTask<byte[]> GetServerPublicKeyAsync(CancellationToken cancellationToken = default);
}
