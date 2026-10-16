using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers;

/// <summary>Stores the state of the helper hire offer.</summary>
/// <param name="State">The offer state.</param>
public sealed record SetHelperHireOfferStateCommand(int State) : CodecCommand<SetHelperHireOfferStateCommand>, ICommandCodecProvider<SetHelperHireOfferStateCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetHelperHireOfferStateCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.State, static state => new SetHelperHireOfferStateCommand(state)));
}
