using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Sets selected state flags on one Mini Pass instance.</summary>
/// <param name="Flags">
/// Bit mask of flags to set, written as a VarInt: 1 marks the introduction seen, 2 the completion celebration seen and 4 the end screen
/// finished.
/// </param>
/// <param name="SecondaryPass">
/// <see langword="true"/> to apply the flags to the secondary pass instance, <see langword="false"/> for the current pass; written as a
/// Boolean after the flags.
/// </param>
public sealed record SetMiniPassStateFlagsCommand(int Flags, bool SecondaryPass) : CodecCommand<SetMiniPassStateFlagsCommand>, ICommandCodecProvider<SetMiniPassStateFlagsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetMiniPassStateFlagsCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
            static command => command.Flags,
            Codecs.Boolean,
            static command => command.SecondaryPass,
            static (flags, secondaryPass) => new SetMiniPassStateFlagsCommand(flags, secondaryPass)),
        CommandLayout.MetadataFirst);
}
