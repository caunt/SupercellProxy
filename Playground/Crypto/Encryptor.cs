namespace Playground.Crypto;

public abstract class Encryptor
{
    public abstract int Version { get; }

    public abstract void Encrypt(ref byte[] data);

    public abstract void Decrypt(ref byte[] data);
}