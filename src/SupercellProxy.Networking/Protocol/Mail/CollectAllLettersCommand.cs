using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Mail;

/// <summary>
/// <para>Collects all eligible letters.</para>
/// </summary>
public sealed record CollectAllLettersCommand : CodecCommand<CollectAllLettersCommand>, ICommandCodecProvider<CollectAllLettersCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CollectAllLettersCommand"/> record. Creates the command; it carries no fields of its own.
    /// </summary>
    public CollectAllLettersCommand() { }

    /// <inheritdoc />
    public static CommandCodec<CollectAllLettersCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new CollectAllLettersCommand()), CommandLayout.MetadataFirst);
}
