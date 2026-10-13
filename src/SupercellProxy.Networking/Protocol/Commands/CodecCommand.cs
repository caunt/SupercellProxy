namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>A client command whose contract and wire form come from its declared codec.</summary>
/// <typeparam name="TSelf">The command record.</typeparam>
public abstract record CodecCommand<TSelf> : Command where TSelf : CodecCommand<TSelf>, ICommandCodecProvider<TSelf>
{
    /// <inheritdoc />
    public sealed override CommandContract Contract => TSelf.Codec;
}
