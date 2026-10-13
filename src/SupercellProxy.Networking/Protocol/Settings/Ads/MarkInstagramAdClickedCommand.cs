using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings.Ads;

/// <summary>Records that the Instagram advertisement was clicked.</summary>
public sealed record MarkInstagramAdClickedCommand : CodecCommand<MarkInstagramAdClickedCommand>, ICommandCodecProvider<MarkInstagramAdClickedCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkInstagramAdClickedCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new MarkInstagramAdClickedCommand()));
}
