using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>A field native sends only from (or only before) a game version; otherwise it takes a fixed value.</summary>
internal sealed class VersionGateCodec<TValue>(Version version, bool fromVersion, ICodec<TValue> codec, TValue absent) : ICodec<TValue>
{
    public TValue Read(MessageStream stream)
    {
        return IsPresent(stream) ? codec.Read(stream) : absent;
    }

    public void Write(ProtocolEncoder encoder, TValue value)
    {
        if (IsPresent(encoder))
            codec.Write(encoder, value);
    }

    private bool IsPresent(ProtocolEncoder encoder)
    {
        ArgumentNullException.ThrowIfNull(encoder);

        return (encoder.GameVersion >= version) == fromVersion;
    }
}
