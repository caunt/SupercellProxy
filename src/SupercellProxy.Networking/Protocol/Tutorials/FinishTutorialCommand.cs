using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Tutorials;

/// <summary>
/// <para>Marks a tutorial data entry complete.</para>
/// </summary>
public sealed record FinishTutorialCommand : CodecCommand<FinishTutorialCommand>, ICommandCodecProvider<FinishTutorialCommand>
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 34;

    /// <summary>
    /// Initializes a new <see cref="FinishTutorialCommand"/> instance.
    /// </summary>
    public FinishTutorialCommand(int tutorialGlobalId)
    {
        TutorialGlobalId = tutorialGlobalId;
    }

    /// <inheritdoc />
    public static CommandCodec<FinishTutorialCommand> Codec { get; } = Codecs.Command(
        type: 34,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.TutorialGlobalId,
            static (tutorialGlobalId) => new FinishTutorialCommand(tutorialGlobalId)
        )
    );

    /// <summary>
    /// Gets the <c language="csharp">TutorialGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("TutorialGlobalId")]
    public int TutorialGlobalId { get; }
}
