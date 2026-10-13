using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>Collects the result of help given to a town service.</summary>
/// <param name="HelpId">The help id.</param>
/// <param name="ServiceObjectGlobalId">The service building object id.</param>
public sealed record CollectTownServiceHelpCommand(int HelpId, int ServiceObjectGlobalId) : CodecCommand<CollectTownServiceHelpCommand>, ICommandCodecProvider<CollectTownServiceHelpCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectTownServiceHelpCommand> Codec { get; } = Codecs.Command(
        type: 255,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.HelpId,
            Codecs.VarInt,
            static command => command.ServiceObjectGlobalId,
            static (helpId, serviceObjectGlobalId) => new CollectTownServiceHelpCommand(helpId, serviceObjectGlobalId)
        )
    );
}
