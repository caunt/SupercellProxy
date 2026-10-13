using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MysteryBoxes;

/// <summary>Discards an unopened mystery box.</summary>
/// <param name="MysteryBoxGlobalId">The mystery box game object id.</param>
public sealed record DiscardMysteryBoxCommand(int MysteryBoxGlobalId) : CodecCommand<DiscardMysteryBoxCommand>, ICommandCodecProvider<DiscardMysteryBoxCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DiscardMysteryBoxCommand> Codec { get; } = Codecs.Command(
        type: 45,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.MysteryBoxGlobalId,
            static (mysteryBoxGlobalId) => new DiscardMysteryBoxCommand(mysteryBoxGlobalId)
        )
    );
}
