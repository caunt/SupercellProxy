using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Tutorials;

/// <summary>
/// <para>Starts the selected tutorial.</para>
/// </summary>
public sealed record StartTutorialCommand : CodecCommand<StartTutorialCommand>, ICommandCodecProvider<StartTutorialCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StartTutorialCommand"/> record. Starts one tutorial step.
    /// </summary>
    /// <param name="tutorialGlobalId">Global id of the step in the tutorials table.</param>
    public StartTutorialCommand(int tutorialGlobalId)
    {
        TutorialGlobalId = tutorialGlobalId;
    }

    /// <inheritdoc />
    public static CommandCodec<StartTutorialCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.TutorialGlobalId, static tutorialGlobalId => new StartTutorialCommand(tutorialGlobalId)));

    /// <summary>
    /// Gets the global id of the started step in the tutorials table.
    /// </summary>
    /// <value>Data id of a row in the tutorials table, written as a variable-length integer.</value>
    [JsonPropertyName("TutorialGlobalId")]
    public int TutorialGlobalId { get; }
}
