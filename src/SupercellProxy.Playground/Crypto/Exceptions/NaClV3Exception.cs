namespace SupercellProxy.Playground.Crypto.Exceptions;

/// <summary>
/// Represents <c language="csharp">NaClV3Exception</c>.
/// </summary>
internal class NaClV3Exception : Exception
{
    public NaClV3Exception() { }

    public NaClV3Exception(string? message)
        : base(message) { }

    public NaClV3Exception(string? message, Exception? innerException)
        : base(message, innerException) { }
}
