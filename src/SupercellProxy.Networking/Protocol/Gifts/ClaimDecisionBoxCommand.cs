using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>
/// Defines the Claim Decision Box Command contract.
/// </summary>
/// <summary>
/// Defines the Choice Index contract.
/// </summary>
public sealed record ClaimDecisionBoxCommand(int ChoiceIndex) : CodecCommand<ClaimDecisionBoxCommand>, ICommandCodecProvider<ClaimDecisionBoxCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimDecisionBoxCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.ChoiceIndex, static (choiceIndex) => new ClaimDecisionBoxCommand(choiceIndex)),
        CommandLayout.MetadataFirst
    );
}
