using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>
/// <para>Reconciles a town service fulfilled by another farm.</para>
/// </summary>
public sealed record TownServiceHelpCompletedServerCommand : CodecServerCommand<TownServiceHelpCompletedServerCommand>, ICommandCodecProvider<TownServiceHelpCompletedServerCommand>
{
    /// <summary>
    /// Provides the Passenger Service Completion Server Command value or operation.
    /// </summary>
    public TownServiceHelpCompletedServerCommand(
        LongId ownerHomeId,
        LongId helperHomeId,
        int helpId,
        ReadOnlyMemory<CommandDataReferenceVarIntPair> requiredItems,
        ReadOnlyMemory<CommandDataReferenceVarIntPair> grantedRewards
    )
    {
        OwnerHomeId = ownerHomeId;
        HelperHomeId = helperHomeId;
        HelpId = helpId;
        RequiredItems = requiredItems.ToArray();
        GrantedRewards = grantedRewards.ToArray();
    }

    /// <inheritdoc />
    public static CommandCodec<TownServiceHelpCompletedServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.OwnerHomeId,
            Codecs.LongId,
            static command => command.HelperHomeId,
            Codecs.VarInt,
            static command => command.HelpId,
            Codecs.Memory(CommandDataReferenceVarIntPair.Codec, CommandVarIntPair.ArrayCount),
            static command => command.RequiredItems,
            Codecs.Memory(CommandDataReferenceVarIntPair.Codec, CommandVarIntPair.ArrayCount),
            static command => command.GrantedRewards,
            static (ownerHomeId, helperHomeId, helpId, requiredItems, grantedRewards) => new TownServiceHelpCompletedServerCommand(ownerHomeId, helperHomeId, helpId, requiredItems, grantedRewards)
        )
    );

    /// <summary>
    /// <para>Gets the resource quantities granted for the completed service.</para>
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair> GrantedRewards { get; }

    /// <summary>
    /// <para>Gets the unique service-help request id.</para>
    /// </summary>
    [JsonPropertyName("HelpId")]
    public int HelpId { get; }

    /// <summary>
    /// <para>Gets the helper's home id.</para>
    /// </summary>
    [JsonPropertyName("HelperHomeId")]
    public LongId HelperHomeId { get; }

    /// <summary>
    /// <para>Gets the home owning the service-help request.</para>
    /// </summary>
    [JsonPropertyName("OwnerHomeId")]
    public LongId OwnerHomeId { get; }

    /// <summary>
    /// <para>Gets the item quantities consumed by the completed service.</para>
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair> RequiredItems { get; }
}
