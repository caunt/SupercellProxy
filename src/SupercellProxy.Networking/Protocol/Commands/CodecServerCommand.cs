namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>A server command whose contract and wire form come from its declared codec.</summary>
/// <typeparam name="TSelf">The command record.</typeparam>
public abstract record CodecServerCommand<TSelf> : ServerCommand where TSelf : CodecServerCommand<TSelf>, ICommandCodecProvider<TSelf>
{
    /// <inheritdoc />
    public sealed override CommandContract Contract => TSelf.Codec;
}
