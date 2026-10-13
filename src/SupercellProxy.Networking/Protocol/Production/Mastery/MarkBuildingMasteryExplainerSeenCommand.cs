using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Production.Mastery;

/// <summary>Marks the building mastery explainer as seen.</summary>
public sealed record MarkBuildingMasteryExplainerSeenCommand : CodecCommand<MarkBuildingMasteryExplainerSeenCommand>, ICommandCodecProvider<MarkBuildingMasteryExplainerSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkBuildingMasteryExplainerSeenCommand> Codec { get; } = Codecs.Command(type: 2655, Codecs.Unit(static () => new MarkBuildingMasteryExplainerSeenCommand()));
}
