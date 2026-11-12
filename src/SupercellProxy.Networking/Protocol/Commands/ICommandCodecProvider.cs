namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>A command type that declares its own codec.</summary>
/// <typeparam name="TSelf">The implementing command record.</typeparam>
public interface ICommandCodecProvider<TSelf> where TSelf : Command, ICommandCodecProvider<TSelf>
{
    /// <summary>Gets the codec that reads and writes the command and identifies its contract.</summary>
    /// <value>The single codec instance of <typeparamref name="TSelf"/>; it doubles as the <see cref="CommandContract"/> the registry looks the command up by.</value>
    static abstract CommandCodec<TSelf> Codec { get; }
}
