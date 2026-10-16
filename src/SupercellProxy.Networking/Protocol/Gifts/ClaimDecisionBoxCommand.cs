using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>
/// Serverbound command picking one reward from a decision box, a gift that lets the player choose between several rewards.
/// </summary>
/// <param name="ChoiceIndex">Position of the chosen reward among those the box offers.</param>
public sealed record ClaimDecisionBoxCommand(int ChoiceIndex) : CodecCommand<ClaimDecisionBoxCommand>, ICommandCodecProvider<ClaimDecisionBoxCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimDecisionBoxCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.ChoiceIndex, static choiceIndex => new ClaimDecisionBoxCommand(choiceIndex)),
        CommandLayout.MetadataFirst
    );
}
