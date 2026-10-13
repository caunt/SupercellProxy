using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Sets selected state flags on one Mini Pass instance.</summary>
public sealed record SetMiniPassStateFlagsCommand(int Flags, bool SecondaryPass) : CodecCommand<SetMiniPassStateFlagsCommand>, ICommandCodecProvider<SetMiniPassStateFlagsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetMiniPassStateFlagsCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Flags,
            Codecs.Boolean,
            static command => command.SecondaryPass,
            static (flags, secondaryPass) => new SetMiniPassStateFlagsCommand(flags, secondaryPass)
        ),
        CommandLayout.MetadataFirst
    );
}
