using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Records when the neighborhood chat was last read.</summary>
/// <param name="Timestamp">The time the chat was read, in seconds.</param>
public sealed record MarkNeighborhoodChatReadCommand(int Timestamp) : CodecCommand<MarkNeighborhoodChatReadCommand>, ICommandCodecProvider<MarkNeighborhoodChatReadCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkNeighborhoodChatReadCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.Timestamp, static timestamp => new MarkNeighborhoodChatReadCommand(timestamp))
    );
}
