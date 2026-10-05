using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Mail;

/// <summary>
/// <para>Applies the native command-694 postman state transition.</para>
/// </summary>
public sealed record PostmanStateCommand : Command
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 694;

    /// <summary>
    /// Defines the <c language="csharp">RequiredState</c> value.
    /// </summary>
    public const int RequiredState = 11;

    /// <summary>
    /// Defines the <c language="csharp">ResultState</c> value.
    /// </summary>
    public const int ResultState = 2;

    /// <summary>
    /// Initializes a new <see cref="PostmanStateCommand"/> instance.
    /// </summary>
    public PostmanStateCommand() { }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static PostmanStateCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        return new PostmanStateCommand();
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
    }
}
