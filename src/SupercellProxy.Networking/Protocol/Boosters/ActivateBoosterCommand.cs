using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>
/// Activates a booster held in the player's booster storage.
/// The native purpose of the trailing modifier fields is unestablished; every retained input uses
/// <c>0, false, false</c>.
/// </summary>
/// <param name="BoosterDataGlobalId">The activated booster's data row in the boosters table.</param>
/// <param name="Unknown0">Retained native modifier; only zero is proven.</param>
/// <param name="Unknown1">Retained native modifier; only false is proven.</param>
/// <param name="Unknown2">Retained native modifier; only false is proven.</param>
public sealed record ActivateBoosterCommand(int BoosterDataGlobalId, int Unknown0, bool Unknown1, bool Unknown2) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ActivateBoosterCommandType;

    /// <summary>Decodes a booster activation command.</summary>
    public static ActivateBoosterCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int boosterDataGlobalId = stream.ReadVarInt();
        int unknown0 = stream.ReadVarInt();
        bool unknown1 = stream.ReadBoolean();
        bool unknown2 = stream.ReadBoolean();

        return new ActivateBoosterCommand(boosterDataGlobalId, unknown0, unknown1, unknown2);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(BoosterDataGlobalId);
        stream.WriteVarInt(Unknown0);
        stream.WriteBoolean(Unknown1);
        stream.WriteBoolean(Unknown2);
    }
}
