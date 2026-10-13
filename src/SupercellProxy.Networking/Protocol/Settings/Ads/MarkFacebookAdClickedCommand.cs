using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings.Ads;

/// <summary>Records that the Facebook advertisement was clicked.</summary>
public sealed record MarkFacebookAdClickedCommand : CodecCommand<MarkFacebookAdClickedCommand>, ICommandCodecProvider<MarkFacebookAdClickedCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkFacebookAdClickedCommand> Codec { get; } = Codecs.Command(type: 505, Codecs.Unit(static () => new MarkFacebookAdClickedCommand()));
}
