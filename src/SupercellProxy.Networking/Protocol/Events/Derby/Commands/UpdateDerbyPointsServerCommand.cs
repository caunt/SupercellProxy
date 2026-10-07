using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Supplies the current player contribution and neighborhood derby progress.</summary>
public sealed record UpdateDerbyPointsServerCommand(
    int PlayerPoints,
    int NeighborhoodPoints,
    int BingoCount,
    int BingoValue,
    int BunnyCount,
    int BingoLinesGained,
    LongId? InstanceId
) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.UpdateDerbyPointsServerCommandType;

    /// <summary>Decodes progress before its server-command metadata.</summary>
    public static UpdateDerbyPointsServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadOptionalLongId()
        );
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(PlayerPoints);
        stream.WriteVarInt(NeighborhoodPoints);
        stream.WriteVarInt(BingoCount);
        stream.WriteVarInt(BingoValue);
        stream.WriteVarInt(BunnyCount);
        stream.WriteVarInt(BingoLinesGained);
        stream.WriteOptionalLongId(InstanceId);
    }
}
