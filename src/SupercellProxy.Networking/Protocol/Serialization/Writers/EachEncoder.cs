using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Writers;

/// <summary>Writes every element without a count.</summary>
internal sealed class EachEncoder<TElement>(IEncoder<TElement> element) : IEncoder<IEnumerable<TElement>>
{
    public void Write(ProtocolEncoder encoder, IEnumerable<TElement> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        foreach (TElement item in value)
            element.Write(encoder, item);
    }
}
