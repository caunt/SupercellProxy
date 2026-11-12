namespace SupercellProxy.Networking.Cryptography;

/// <summary>Supplies the public key used to authenticate an upstream server.</summary>
public interface IServerPublicKeySource
{
    /// <summary>Loads the current server public key.</summary>
    /// <param name="cancellationToken">Cancels a key lookup that has to go to the network or wait on another lookup.</param>
    /// <returns>The server's raw 32-byte public key, which the upstream handshake combines with the client's private key to seal the login.</returns>
    ValueTask<byte[]> GetServerPublicKeyAsync(CancellationToken cancellationToken = default);
}
