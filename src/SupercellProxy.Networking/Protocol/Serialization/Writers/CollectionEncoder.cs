using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Writers;

/// <summary>Writes a count followed by every element.</summary>
internal sealed class CollectionEncoder<TElement>(IEncoder<TElement> element, CollectionCount count) : IEncoder<IReadOnlyCollection<TElement>>
{
    public void Write(ProtocolEncoder encoder, IReadOnlyCollection<TElement> value)
    {
        ArgumentNullException.ThrowIfNull(value);
        count.Write(encoder, value.Count);

        foreach (TElement item in value)
            element.Write(encoder, item);
    }
}
