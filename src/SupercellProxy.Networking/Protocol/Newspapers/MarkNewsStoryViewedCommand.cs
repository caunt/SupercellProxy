using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>Records the last viewed news story.</summary>
/// <param name="StoryId">The viewed news story id.</param>
public sealed record MarkNewsStoryViewedCommand(int StoryId) : CodecCommand<MarkNewsStoryViewedCommand>, ICommandCodecProvider<MarkNewsStoryViewedCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkNewsStoryViewedCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.StoryId, static storyId => new MarkNewsStoryViewedCommand(storyId)));
}
