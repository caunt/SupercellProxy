using System.Collections.Immutable;
using System.Runtime.InteropServices;

using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>A counted array that native writes as a negative count when it is absent.</summary>
internal sealed class NullableArrayCodec<TElement>(ICodec<TElement> element, CollectionCount count) : ICodec<ImmutableArray<TElement>?>
{
    public ImmutableArray<TElement>? Read(MessageChannel stream)
    {
        int length = count.ReadNullable(stream);

        if (length < 0)
            return null;

        TElement[] values = new TElement[length];

        for (int index = 0; index < values.Length; index++)
            values[index] = element.Read(stream);

        return ImmutableCollectionsMarshal.AsImmutableArray(values);
    }

    public void Write(ProtocolEncoder encoder, ImmutableArray<TElement>? value)
    {
        if (value is not { IsDefault: false } items)
        {
            count.Write(encoder, value: -1);

            return;
        }

        count.Write(encoder, items.Length);

        foreach (TElement item in items)
            element.Write(encoder, item);
    }
}
