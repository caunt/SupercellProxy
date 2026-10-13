using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>Selects a whole wire layout by game version.</summary>
internal sealed class VersionSwitchCodec<TValue>(Version version, ICodec<TValue> current, ICodec<TValue> legacy) : ICodec<TValue>
{
    public TValue Read(MessageStream stream)
    {
        return Select(stream).Read(stream);
    }

    public void Write(ProtocolEncoder encoder, TValue value)
    {
        Select(encoder).Write(encoder, value);
    }

    private ICodec<TValue> Select(ProtocolEncoder encoder)
    {
        ArgumentNullException.ThrowIfNull(encoder);

        return encoder.GameVersion >= version ? current : legacy;
    }
}
