using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>A counted array that native writes as a negative count when it is absent.</summary>
internal sealed class NullableArrayCodec<TElement>(ICodec<TElement> element, CollectionCount count) : ICodec<TElement[]?>
{
    public TElement[]? Read(MessageStream stream)
    {
        int length = count.ReadNullable(stream);

        if (length < 0)
            return null;

        TElement[] values = new TElement[length];

        for (int index = 0; index < values.Length; index++)
            values[index] = element.Read(stream);

        return values;
    }

    public void Write(ProtocolEncoder encoder, TElement[]? value)
    {
        if (value is null)
        {
            count.Write(encoder, value: -1);

            return;
        }

        count.Write(encoder, value.Length);

        foreach (TElement item in value)
            element.Write(encoder, item);
    }
}
