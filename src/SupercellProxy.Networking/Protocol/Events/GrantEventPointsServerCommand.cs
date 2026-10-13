using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Grants points to the current event.</summary>
/// <param name="Amount">The amount.</param>
public sealed record GrantEventPointsServerCommand(int Amount) : CodecServerCommand<GrantEventPointsServerCommand>, ICommandCodecProvider<GrantEventPointsServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<GrantEventPointsServerCommand> Codec { get; } = Codecs.Command(
        type: 226,
        Codecs.Composite(Codecs.Int32, static command => command.Amount, static (amount) => new GrantEventPointsServerCommand(amount)),
        CommandLayout.MetadataFirst
    );
}
