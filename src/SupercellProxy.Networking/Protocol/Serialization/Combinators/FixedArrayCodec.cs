using System.Collections.Immutable;
using System.Globalization;
using System.Runtime.InteropServices;

using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>A fixed number of elements without a count prefix.</summary>
internal sealed class FixedArrayCodec<TElement>(ICodec<TElement> element, int length) : ICodec<ImmutableArray<TElement>>
{
    public ImmutableArray<TElement> Read(MessageChannel stream)
    {
        TElement[] values = new TElement[length];

        for (int index = 0; index < values.Length; index++)
            values[index] = element.Read(stream);

        return ImmutableCollectionsMarshal.AsImmutableArray(values);
    }

    public void Write(ProtocolEncoder encoder, ImmutableArray<TElement> value)
    {
        if (value.IsDefault)
            throw new ArgumentException(message: "An uninitialized array cannot be written.", nameof(value));

        if (value.Length != length)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Expected exactly {length} elements, got {value.Length}."));

        foreach (TElement item in value)
            element.Write(encoder, item);
    }
}
