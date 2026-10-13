using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Hall;

/// <summary>Selects a town hall setting in the player's own town.</summary>
/// <param name="SettingGlobalId">Gets the selected town hall setting data id.</param>
public sealed record SelectTownHallSettingCommand(int SettingGlobalId) : CodecCommand<SelectTownHallSettingCommand>, ICommandCodecProvider<SelectTownHallSettingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SelectTownHallSettingCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Int32,
            static command => command.SettingGlobalId,
            static (settingGlobalId) => new SelectTownHallSettingCommand(settingGlobalId)
        )
    );
}
