namespace SupercellProxy.Networking.Protocol.Serialization;

/// <summary>A type that declares its own wire codec.</summary>
/// <typeparam name="TSelf">The implementing type.</typeparam>
public interface ICodecProvider<TSelf> where TSelf : ICodecProvider<TSelf>
{
    /// <summary>Gets the codec that reads and writes the type.</summary>
    /// <value>The one codec for <typeparamref name="TSelf"/>: the message registry calls its <see cref="ICodec{TValue}.Read"/> to decode a payload of that type, and codec-backed messages and commands write themselves through it.</value>
    static abstract ICodec<TSelf> Codec { get; }
}
