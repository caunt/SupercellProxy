using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>Claims one reward step from a chain offer.</summary>
/// <param name="StepIndex">Zero-based index of the step in the offer's step list, written first as a VarInt.</param>
/// <param name="EventId">Id of the running chain-offer live event that owns the step, written as a VarInt after the step index.</param>
public sealed record ClaimChainOfferStepCommand(int StepIndex, [property: JsonPropertyName("EventId")] int EventId) : CodecCommand<ClaimChainOfferStepCommand>, ICommandCodecProvider<ClaimChainOfferStepCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimChainOfferStepCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
            static command => command.StepIndex,
            Codecs.VarInt,
            static command => command.EventId,
            static (stepIndex, eventId) => new ClaimChainOfferStepCommand(stepIndex, eventId)),
        CommandLayout.MetadataFirst);
}
