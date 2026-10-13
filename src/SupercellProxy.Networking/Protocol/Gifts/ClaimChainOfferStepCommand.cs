using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>Claims one reward step from a chain offer.</summary>
public sealed record ClaimChainOfferStepCommand(int StepIndex, [property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventId) : CodecCommand<ClaimChainOfferStepCommand>, ICommandCodecProvider<ClaimChainOfferStepCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimChainOfferStepCommand> Codec { get; } = Codecs.Command(
        type: 691,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.StepIndex,
            Codecs.VarInt,
            static command => command.EventId,
            static (stepIndex, eventId) => new ClaimChainOfferStepCommand(stepIndex, eventId)
        ),
        CommandLayout.MetadataFirst
    );
}
