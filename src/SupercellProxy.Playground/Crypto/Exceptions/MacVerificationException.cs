namespace SupercellProxy.Playground.Crypto.Exceptions;

public class MacVerificationException(string? message = null, Exception? innerException = null) : NaClV3Exception(message, innerException)
{
    public bool IsPublicKeyBox { get; set; }

    public MacVerificationException(bool isPublicKeyBox, string? message = null, Exception? innerException = null) : this(message, innerException)
    {
        IsPublicKeyBox = isPublicKeyBox;
    }
}
