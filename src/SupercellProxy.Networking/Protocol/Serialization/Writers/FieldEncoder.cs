using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Writers;

/// <summary>Writes one field of a value through the field's encoder.</summary>
internal sealed class FieldEncoder<TValue, TField>(IEncoder<TField> field, Func<TValue, TField> getter) : IEncoder<TValue>
{
    public void Write(ProtocolEncoder encoder, TValue value)
    {
        field.Write(encoder, getter(value));
    }
}
