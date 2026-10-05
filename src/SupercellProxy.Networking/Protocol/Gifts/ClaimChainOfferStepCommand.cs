using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>Claims one reward step from a chain offer.</summary>
public sealed record ClaimChainOfferStepCommand(int StepIndex, [property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ClaimChainOfferStepCommandType;

    /// <summary>Decodes a chain-offer step claim.</summary>
    public static ClaimChainOfferStepCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new ClaimChainOfferStepCommand(stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(StepIndex);
        stream.WriteVarInt(EventId);
    }
}
