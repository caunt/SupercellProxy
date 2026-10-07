using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Accounts.Permissions;

/// <summary>A player feature permission and its accompanying native restriction value.</summary>
public sealed record PlayerPermission(bool Enabled, int RestrictionValue)
{
    /// <summary>Decodes one permission entry.</summary>
    public static PlayerPermission Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadBoolean(), stream.ReadVarInt());
    }

    /// <summary>Encodes one permission entry.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteBoolean(Enabled);
        stream.WriteVarInt(RestrictionValue);
    }
}
