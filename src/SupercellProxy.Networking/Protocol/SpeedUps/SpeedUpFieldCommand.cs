using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.SpeedUps;

/// <summary>Finishes a growing field immediately for diamonds.</summary>
/// <param name="FieldGlobalId">The field game object id.</param>
/// <param name="IncludeOtherGrowingFields">Whether every other still-growing field is finished too.</param>
public sealed record SpeedUpFieldCommand(int FieldGlobalId, bool IncludeOtherGrowingFields) : CodecCommand<SpeedUpFieldCommand>, ICommandCodecProvider<SpeedUpFieldCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpFieldCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.FieldGlobalId,
            Codecs.Boolean,
            static command => command.IncludeOtherGrowingFields,
            static (fieldGlobalId, includeOtherGrowingFields) => new SpeedUpFieldCommand(fieldGlobalId, includeOtherGrowingFields)
        )
    );
}
