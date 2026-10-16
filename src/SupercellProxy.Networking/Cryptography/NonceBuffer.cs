using System.Runtime.CompilerServices;

namespace SupercellProxy.Networking.Cryptography;

/// <summary>
/// Fixed 24 bytes stored inline, so a <see cref="Nonce"/> holds its bytes without a separate array allocation.
/// </summary>
[InlineArray(24)]
internal struct NonceBuffer
{
    private byte _element0;
}
