using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Characters;

/// <summary>Changes whether Angus is interacting with the player.</summary>
/// <param name="Active"><see langword="true"/> when the player starts interacting with Angus; <see langword="false"/> when the interaction ends. Written as a Boolean before the id.</param>
/// <param name="AngusGlobalId">The game object id of the Angus character the interaction applies to, written as a VarInt after the flag.</param>
public sealed record SetAngusInteractionCommand(bool Active, int AngusGlobalId) : CodecCommand<SetAngusInteractionCommand>, ICommandCodecProvider<SetAngusInteractionCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetAngusInteractionCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.Boolean,
            static command => command.Active,
            Codecs.VarInt,
            static command => command.AngusGlobalId,
            static (active, angusGlobalId) => new SetAngusInteractionCommand(active, angusGlobalId)),
        CommandLayout.MetadataFirst);
}
