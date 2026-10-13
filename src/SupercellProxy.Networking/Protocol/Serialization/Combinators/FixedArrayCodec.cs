using System.Globalization;

using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>A fixed number of elements without a count prefix.</summary>
internal sealed class FixedArrayCodec<TElement>(ICodec<TElement> element, int length) : ICodec<TElement[]>
{
    public TElement[] Read(MessageStream stream)
    {
        TElement[] values = new TElement[length];

        for (int index = 0; index < values.Length; index++)
            values[index] = element.Read(stream);

        return values;
    }

    public void Write(ProtocolEncoder encoder, TElement[] value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length != length)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Expected exactly {length} elements, got {value.Length}."));

        foreach (TElement item in value)
            element.Write(encoder, item);
    }
}
