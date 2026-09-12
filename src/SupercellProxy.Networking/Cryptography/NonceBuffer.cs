using System.Runtime.CompilerServices;

namespace SupercellProxy.Networking.Cryptography;

/// <summary>
/// Defines the Nonce Buffer contract.
/// </summary>
[InlineArray(24)]
internal struct NonceBuffer
{
    private byte _element0;
}
