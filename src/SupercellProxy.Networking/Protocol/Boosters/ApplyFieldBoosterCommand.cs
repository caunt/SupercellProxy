using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Applies a booster to a field; native allows it only outside the production environment.</summary>
/// <param name="BoosterGlobalId">The booster data id.</param>
/// <param name="FieldGlobalId">The field game object id.</param>
public sealed record ApplyFieldBoosterCommand(int BoosterGlobalId, int FieldGlobalId) : CodecCommand<ApplyFieldBoosterCommand>, ICommandCodecProvider<ApplyFieldBoosterCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ApplyFieldBoosterCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BoosterGlobalId,
            Codecs.VarInt,
            static command => command.FieldGlobalId,
            static (boosterGlobalId, fieldGlobalId) => new ApplyFieldBoosterCommand(boosterGlobalId, fieldGlobalId)
        ),
        CommandLayout.FieldsFirst,
        productionAllowed: false
    );
}
