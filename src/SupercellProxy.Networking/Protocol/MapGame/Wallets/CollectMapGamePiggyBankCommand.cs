using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Wallets;

/// <summary>Collects the coins saved in the valley piggy bank.</summary>
/// <param name="MapGameId">Id of the Valley board whose piggy bank is emptied.</param>
public sealed record CollectMapGamePiggyBankCommand(LongId MapGameId) : CodecCommand<CollectMapGamePiggyBankCommand>, ICommandCodecProvider<CollectMapGamePiggyBankCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectMapGamePiggyBankCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.LongId, static command => command.MapGameId, static mapGameId => new CollectMapGamePiggyBankCommand(mapGameId)),
        CommandLayout.MetadataFirst
    );
}
