using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Decorations.Modular;

/// <summary>Sets the chosen options of a modular decoration.</summary>
/// <param name="ObjectGlobalId">The modular decoration object id.</param>
/// <param name="OptionGlobalIds">The chosen option data ids.</param>
public sealed record SetModularDecorationOptionsCommand(int ObjectGlobalId, ReadOnlyMemory<int> OptionGlobalIds) : CodecCommand<SetModularDecorationOptionsCommand>, ICommandCodecProvider<SetModularDecorationOptionsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetModularDecorationOptionsCommand> Codec { get; } = Codecs.Command(
        type: 322,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ObjectGlobalId,
            Codecs.Memory(Codecs.VarInt),
            static command => command.OptionGlobalIds,
            static (objectGlobalId, optionGlobalIds) => new SetModularDecorationOptionsCommand(objectGlobalId, optionGlobalIds)
        )
    );
}
