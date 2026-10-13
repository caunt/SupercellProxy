using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Mail;

/// <summary>
/// <para>Applies the saved postman state transition.</para>
/// </summary>
public sealed record PostmanStateCommand : CodecCommand<PostmanStateCommand>, ICommandCodecProvider<PostmanStateCommand>
{
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

    /// <inheritdoc />
    public static CommandCodec<PostmanStateCommand> Codec { get; } = Codecs.Command(type: 694, Codecs.Unit(static () => new PostmanStateCommand()), CommandLayout.MetadataFirst);
}
