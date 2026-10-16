using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Tutorials;

/// <summary>
/// <para>Marks a tutorial data entry complete.</para>
/// </summary>
public sealed record FinishTutorialCommand : CodecCommand<FinishTutorialCommand>, ICommandCodecProvider<FinishTutorialCommand>
{
    /// <summary>
    /// Marks one tutorial step finished.
    /// </summary>
    /// <param name="tutorialGlobalId">Global id of the step in the tutorials table.</param>
    public FinishTutorialCommand(int tutorialGlobalId)
    {
        TutorialGlobalId = tutorialGlobalId;
    }

    /// <inheritdoc />
    public static CommandCodec<FinishTutorialCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.TutorialGlobalId,
            static tutorialGlobalId => new FinishTutorialCommand(tutorialGlobalId)
        )
    );

    /// <summary>
    /// Global id of the finished step in the tutorials table; the server refuses steps that are already complete.
    /// </summary>
    [JsonPropertyName("TutorialGlobalId")]
    public int TutorialGlobalId { get; }
}
