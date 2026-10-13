using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Claims one Mini Pass reward from the free or premium track.</summary>
/// <param name="Track">Reward track: 1 selects premium, any other value the free track.</param>
/// <param name="Index">Zero-based reward index on the track.</param>
public sealed record ClaimMiniPassRewardCommand(int Track, int Index) : CodecCommand<ClaimMiniPassRewardCommand>, ICommandCodecProvider<ClaimMiniPassRewardCommand>
{
    /// <summary>Track value selecting the free rewards.</summary>
    public const int FreeTrack = 0;

    /// <summary>Track value selecting the premium rewards.</summary>
    public const int PremiumTrack = 1;

    /// <inheritdoc />
    public static CommandCodec<ClaimMiniPassRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Track,
            Codecs.VarInt,
            static command => command.Index,
            static (track, index) => new ClaimMiniPassRewardCommand(track, index)
        ),
        CommandLayout.MetadataFirst
    );
}
