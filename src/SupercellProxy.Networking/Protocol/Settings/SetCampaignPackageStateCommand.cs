using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings;

/// <summary>Stores the campaign package state.</summary>
/// <param name="State">The new campaign package state.</param>
public sealed record SetCampaignPackageStateCommand(int State) : CodecCommand<SetCampaignPackageStateCommand>, ICommandCodecProvider<SetCampaignPackageStateCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetCampaignPackageStateCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.State, static (state) => new SetCampaignPackageStateCommand(state)));
}
