using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings.Ads;

/// <summary>Records that the Weibo advertisement was clicked.</summary>
public sealed record MarkWeiboAdClickedCommand : CodecCommand<MarkWeiboAdClickedCommand>, ICommandCodecProvider<MarkWeiboAdClickedCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkWeiboAdClickedCommand> Codec { get; } = Codecs.Command(type: 635, Codecs.Unit(static () => new MarkWeiboAdClickedCommand()));
}
