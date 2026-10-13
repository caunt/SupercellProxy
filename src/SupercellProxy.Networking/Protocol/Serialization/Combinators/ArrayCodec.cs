using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>A counted array whose count prefix and bound come from <see cref="CollectionCount"/>.</summary>
internal sealed class ArrayCodec<TElement>(ICodec<TElement> element, CollectionCount count) : ICodec<TElement[]>
{
    public TElement[] Read(MessageStream stream)
    {
        TElement[] values = new TElement[count.Read(stream)];

        for (int index = 0; index < values.Length; index++)
            values[index] = element.Read(stream);

        return values;
    }

    public void Write(ProtocolEncoder encoder, TElement[] value)
    {
        ArgumentNullException.ThrowIfNull(value);
        count.Write(encoder, value.Length);

        foreach (TElement item in value)
            element.Write(encoder, item);
    }
}
