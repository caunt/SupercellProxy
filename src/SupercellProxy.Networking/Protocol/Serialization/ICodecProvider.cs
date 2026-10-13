namespace SupercellProxy.Networking.Protocol.Serialization;

/// <summary>A type that declares its own wire codec.</summary>
/// <typeparam name="TSelf">The implementing type.</typeparam>
public interface ICodecProvider<TSelf> where TSelf : ICodecProvider<TSelf>
{
    /// <summary>Gets the codec that reads and writes the type.</summary>
    static abstract ICodec<TSelf> Codec { get; }
}
