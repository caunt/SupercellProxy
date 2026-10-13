using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Tutorials;

/// <summary>
/// <para>Starts the selected tutorial.</para>
/// </summary>
public sealed record StartTutorialCommand : CodecCommand<StartTutorialCommand>, ICommandCodecProvider<StartTutorialCommand>
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 35;

    /// <summary>
    /// Initializes a new <see cref="StartTutorialCommand"/> instance.
    /// </summary>
    public StartTutorialCommand(int tutorialGlobalId)
    {
        TutorialGlobalId = tutorialGlobalId;
    }

    /// <inheritdoc />
    public static CommandCodec<StartTutorialCommand> Codec { get; } = Codecs.Command(
        type: 35,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.TutorialGlobalId,
            static (tutorialGlobalId) => new StartTutorialCommand(tutorialGlobalId)
        )
    );

    /// <summary>
    /// Gets the <c language="csharp">TutorialGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("TutorialGlobalId")]
    public int TutorialGlobalId { get; }
}
