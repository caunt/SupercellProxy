using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>
/// Exchanges one booster held in the player's booster storage for another booster.
/// The native purpose of the trailing modifier field is unestablished; every retained input uses
/// <c>false</c>.
/// </summary>
/// <param name="RemovedBoosterDataGlobalId">The consumed booster's data row in the boosters table.</param>
/// <param name="AddedBoosterDataGlobalId">The granted booster's data row in the boosters table.</param>
/// <param name="Unknown0">Retained native modifier; only false is proven.</param>
public sealed record ExchangeBoosterCommand(int RemovedBoosterDataGlobalId, int AddedBoosterDataGlobalId, bool Unknown0) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ExchangeBoosterCommandType;

    /// <summary>Decodes a booster exchange command.</summary>
    public static ExchangeBoosterCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int removedBoosterDataGlobalId = stream.ReadVarInt();
        int addedBoosterDataGlobalId = stream.ReadInt32();
        bool unknown0 = stream.ReadBoolean();

        return new ExchangeBoosterCommand(removedBoosterDataGlobalId, addedBoosterDataGlobalId, unknown0);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(RemovedBoosterDataGlobalId);
        stream.WriteInt32(AddedBoosterDataGlobalId);
        stream.WriteBoolean(Unknown0);
    }
}
