using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Tutorials;

/// <summary>
/// <para>Marks a tutorial data entry complete.</para>
/// </summary>
public sealed record FinishTutorialCommand : Command
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

    /// <summary>
    /// Gets the <c language="csharp">TutorialGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("TutorialGlobalId")]
    public int TutorialGlobalId { get; }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static FinishTutorialCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int tutorialGlobalId = stream.ReadVarInt();

        return new FinishTutorialCommand(tutorialGlobalId);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(TutorialGlobalId);
    }
}
