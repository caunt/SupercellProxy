using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Accounts.Permissions;

/// <summary>Provides player feature permissions and the optional signed player token used by onboarding services.</summary>
public sealed record PlayerPermissionsMessage(int State, PlayerPermission[] Permissions, bool UnknownFlag, Memory<byte>? CompressedPlayerToken) : IMessage
{
    /// <summary>Decodes the native permission list and compressed token.</summary>
    public static PlayerPermissionsMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt(), stream.ReadArray(PlayerPermission.Decode), stream.ReadBoolean(), stream.ReadOptionalByteArray());
    }

    /// <summary>Encodes the native permission list and compressed token.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(State);
        stream.WriteArray<PlayerPermission>(Permissions, static (writer, permission) => permission.Encode(writer));
        stream.WriteBoolean(UnknownFlag);
        stream.WriteOptionalByteArray(CompressedPlayerToken is { } token ? token : null);
    }

    /// <summary>Keeps the signed player token out of diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(PlayerPermissionsMessage);
    }
}
