using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Characters;

/// <summary>Changes whether Angus is interacting with the player.</summary>
public sealed record SetAngusInteractionCommand(bool Active, int AngusGlobalId) : CodecCommand<SetAngusInteractionCommand>, ICommandCodecProvider<SetAngusInteractionCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetAngusInteractionCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Boolean,
            static command => command.Active,
            Codecs.VarInt,
            static command => command.AngusGlobalId,
            static (active, angusGlobalId) => new SetAngusInteractionCommand(active, angusGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
