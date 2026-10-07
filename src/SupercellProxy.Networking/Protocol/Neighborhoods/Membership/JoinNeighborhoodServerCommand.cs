using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Confirms neighborhood creation or membership, including badge and next-league state.</summary>
public sealed record JoinNeighborhoodServerCommand(
    LongId NeighborhoodId,
    string? NeighborhoodName,
    bool Created,
    int BadgeUnknown0,
    int BadgeUnknown1,
    int BadgeUnknown2,
    int Source,
    int NextLeagueIndex,
    bool CompleteNeighborhoodTutorials,
    int Unknown0,
    int Unknown1,
    int Unknown2
) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.JoinNeighborhoodServerCommandType;

    /// <summary>Decodes the native membership payload before server-command metadata.</summary>
    public static JoinNeighborhoodServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(
            stream.ReadLongId(),
            stream.ReadOptionalString(),
            stream.ReadBoolean(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadBoolean(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt()
        );
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongId(NeighborhoodId);
        stream.WriteOptionalString(NeighborhoodName);
        stream.WriteBoolean(Created);
        stream.WriteVarInt(BadgeUnknown0);
        stream.WriteVarInt(BadgeUnknown1);
        stream.WriteVarInt(BadgeUnknown2);
        stream.WriteVarInt(Source);
        stream.WriteVarInt(NextLeagueIndex);
        stream.WriteBoolean(CompleteNeighborhoodTutorials);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
    }
}
