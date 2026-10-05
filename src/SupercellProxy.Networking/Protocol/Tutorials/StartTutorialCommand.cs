using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Tutorials;

/// <summary>
/// <para>Starts the selected tutorial.</para>
/// </summary>
public sealed record StartTutorialCommand : Command
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
    public static StartTutorialCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int tutorialGlobalId = stream.ReadVarInt();

        return new StartTutorialCommand(tutorialGlobalId);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(TutorialGlobalId);
    }
}
