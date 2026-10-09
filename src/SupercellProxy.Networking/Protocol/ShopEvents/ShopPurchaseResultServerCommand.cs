using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>Reports a shop purchase request outcome without granting the purchased package again.</summary>
public sealed record ShopPurchaseResultServerCommand(int EventId, int Variant, int PackageGlobalId, int Index, bool Succeeded, int ErrorCode, string? ErrorText) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ShopPurchaseResultServerCommandType;

    /// <summary>Decodes the result; failure details are present only when the success flag is clear.</summary>
    public static ShopPurchaseResultServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int eventId = stream.ReadInt32();
        int state = stream.ReadInt32();
        int resource = stream.ReadInt32();
        int amount = stream.ReadVarInt();
        bool succeeded = stream.ReadBoolean();
        int error = succeeded ? 0 : stream.ReadInt32();
        string? text = succeeded ? null : stream.ReadString();

        return new(eventId, state, resource, amount, succeeded, error, text);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteInt32(EventId);
        stream.WriteInt32(Variant);
        stream.WriteInt32(PackageGlobalId);
        stream.WriteVarInt(Index);
        stream.WriteBoolean(Succeeded);

        if (Succeeded) return;

        stream.WriteInt32(ErrorCode);
        stream.WriteString(ErrorText ?? string.Empty);
    }
}
