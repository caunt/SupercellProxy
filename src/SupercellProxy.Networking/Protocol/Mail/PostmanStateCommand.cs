using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Mail;

/// <summary>
/// <para>Applies the saved postman state transition.</para>
/// </summary>
public sealed record PostmanStateCommand : CodecCommand<PostmanStateCommand>, ICommandCodecProvider<PostmanStateCommand>
{
    /// <summary>
    /// Postman state the command applies to: the state an idle postman enters when a new day starts. In any other state the
    /// command is refused.
    /// </summary>
    public const int RequiredState = 11;

    /// <summary>
    /// Postman state the command moves the postman to.
    /// </summary>
    public const int ResultState = 2;

    /// <summary>
    /// Creates the command; it carries no fields of its own.
    /// </summary>
    public PostmanStateCommand() { }

    /// <inheritdoc />
    public static CommandCodec<PostmanStateCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new PostmanStateCommand()), CommandLayout.MetadataFirst);
}
