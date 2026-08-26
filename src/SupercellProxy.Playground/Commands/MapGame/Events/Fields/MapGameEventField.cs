using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>One typed field in a polymorphic native map-game event.</para>
/// </summary>
public abstract record MapGameEventField
{
    internal abstract MapGameEventFieldType FieldType { get; }

    internal abstract void Encode(MessageStream stream);
}
