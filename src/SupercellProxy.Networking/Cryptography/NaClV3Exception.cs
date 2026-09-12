namespace SupercellProxy.Networking.Cryptography;

/// <summary>
/// Represents <c language="csharp">NaClV3Exception</c>.
/// </summary>
public class NaClV3Exception : Exception
{
    /// <summary>
    /// Provides the Na Cl V3 Exception value or operation.
    /// </summary>
    public NaClV3Exception() { }

    /// <summary>
    /// Provides the Na Cl V3 Exception value or operation.
    /// </summary>
    public NaClV3Exception(string? message)
        : base(message) { }

    /// <summary>
    /// Provides the Na Cl V3 Exception value or operation.
    /// </summary>
    public NaClV3Exception(string? message, Exception? innerException)
        : base(message, innerException) { }
}
