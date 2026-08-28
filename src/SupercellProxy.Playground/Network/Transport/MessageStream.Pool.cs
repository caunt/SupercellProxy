namespace SupercellProxy.Playground.Network.Transport;

internal sealed partial class MessageStream
{
    private readonly byte[] _buffer = new byte[65536];

    /// <summary>
    /// Executes the <c language="csharp">RentExactly</c> operation.
    /// </summary>
    public Memory<byte> RentExactly(int length)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(length, _buffer.Length);
        return _buffer.AsMemory(0, length);
    }
}
