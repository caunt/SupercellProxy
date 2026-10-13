using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>Warns that the server is shutting down; the client shows the shutdown dialog with the remaining minutes.</summary>
/// <param name="SecondsUntilShutdown">The seconds until shutdown.</param>
public sealed record ShutdownStartedMessage(int SecondsUntilShutdown) : CodecMessage<ShutdownStartedMessage>, ICodecProvider<ShutdownStartedMessage>
{
    /// <inheritdoc />
    public static ICodec<ShutdownStartedMessage> Codec { get; } = Codecs.Composite(
        Codecs.Int32,
        static message => message.SecondsUntilShutdown,
        static (secondsUntilShutdown) => new ShutdownStartedMessage(secondsUntilShutdown)
    );
}
