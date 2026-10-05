using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>
/// <para>Updates the retained tutorial step for the active decoration event.</para>
/// </summary>
public sealed record DecorationEventTutorialCommand : Command
{
    /// <summary>
    /// <para>Initializes a decoration-event tutorial command.</para>
    /// </summary>
    public DecorationEventTutorialCommand(int lastIntroStep = 0)
    {
        LastIntroStep = lastIntroStep;
    }

    /// <summary>
    /// <para>Gets the tutorial step selected by the client.</para>
    /// </summary>
    public int LastIntroStep { get; }

    /// <inheritdoc />
    public override int Type => 654;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static DecorationEventTutorialCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new DecorationEventTutorialCommand(stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(LastIntroStep);
    }
}
