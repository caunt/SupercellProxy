using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Events.Fields;

/// <summary>
/// <para>One typed field in a polymorphic native map-game event.</para>
/// </summary>
public abstract record MapGameEventField
{
    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public abstract MapGameEventFieldType FieldType { get; }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public abstract void Encode(MessageStream stream);
}
