using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing.Fishes;

/// <summary>Records that the fishing hut has shown the given fish.</summary>
/// <param name="FishGlobalId">The fish data id.</param>
public sealed record MarkFishSeenCommand(int FishGlobalId) : CodecCommand<MarkFishSeenCommand>, ICommandCodecProvider<MarkFishSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkFishSeenCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.FishGlobalId, static fishGlobalId => new MarkFishSeenCommand(fishGlobalId))
    );
}
