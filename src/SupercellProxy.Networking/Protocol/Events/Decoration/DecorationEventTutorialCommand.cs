using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>
/// <para>Updates the retained tutorial step for the active decoration event.</para>
/// </summary>
public sealed record DecorationEventTutorialCommand : CodecCommand<DecorationEventTutorialCommand>, ICommandCodecProvider<DecorationEventTutorialCommand>
{
    /// <summary>
    /// Records the intro screen the player has reached.
    /// </summary>
    /// <param name="lastIntroStep">Index of that intro screen.</param>
    public DecorationEventTutorialCommand(int lastIntroStep = 0)
    {
        LastIntroStep = lastIntroStep;
    }

    /// <inheritdoc />
    public static CommandCodec<DecorationEventTutorialCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.LastIntroStep,
            static lastIntroStep => new DecorationEventTutorialCommand(lastIntroStep)
        ),
        CommandLayout.MetadataFirst
    );

    /// <summary>
    /// <para>Gets the tutorial step selected by the client.</para>
    /// </summary>
    public int LastIntroStep { get; }
}
