using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Mail;

/// <summary>
/// <para>Collects all eligible letters.</para>
/// </summary>
public sealed record CollectAllLettersCommand : Command
{
    /// <summary>
    /// Initializes a new <see cref="CollectAllLettersCommand"/> instance.
    /// </summary>
    public CollectAllLettersCommand() { }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandRegistry.CollectAllLettersCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CollectAllLettersCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        return new CollectAllLettersCommand();
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
    }
}
