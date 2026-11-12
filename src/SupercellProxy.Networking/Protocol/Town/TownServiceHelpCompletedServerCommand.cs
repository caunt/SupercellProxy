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
    /// Initializes a new instance of the <see cref="TownServiceHelpCompletedServerCommand"/> record. Provides the Passenger Service Completion Server Command value or operation.
    /// </summary>
    /// <param name="ownerHomeId">Home id of the town that asked for help with the service.</param>
    /// <param name="helperHomeId">Home id of the farm that served the passenger.</param>
    /// <param name="helpId">Id of the owner's service-help request that was fulfilled.</param>
    /// <param name="requiredItems">Goods and amounts the helper hands over for the service; copied into a new array.</param>
    /// <param name="grantedRewards">Resources and amounts the helper earns for the service; copied into a new array.</param>
    public TownServiceHelpCompletedServerCommand(LongId ownerHomeId,
        LongId helperHomeId,
        int helpId,
        ReadOnlyMemory<CommandDataReferenceVarIntPair> requiredItems,
        ReadOnlyMemory<CommandDataReferenceVarIntPair> grantedRewards)
    {
        OwnerHomeId = ownerHomeId;
        HelperHomeId = helperHomeId;
        HelpId = helpId;
        RequiredItems = requiredItems.ToArray();
        GrantedRewards = grantedRewards.ToArray();
    }

    /// <inheritdoc />
    public static CommandCodec<TownServiceHelpCompletedServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.LongId,
        static command => command.OwnerHomeId,
        Codecs.LongId,
        static command => command.HelperHomeId,
        Codecs.VarInt,
        static command => command.HelpId,
        Codecs.Memory(CommandDataReferenceVarIntPair.Codec, CommandVarIntPair.ArrayCount),
        static command => command.RequiredItems,
        Codecs.Memory(CommandDataReferenceVarIntPair.Codec, CommandVarIntPair.ArrayCount),
        static command => command.GrantedRewards,
        static (ownerHomeId, helperHomeId, helpId, requiredItems, grantedRewards) => new TownServiceHelpCompletedServerCommand(ownerHomeId, helperHomeId, helpId, requiredItems, grantedRewards)));

    /// <summary>
    /// Gets the resource quantities granted for the completed service.
    /// </summary>
    /// <value>Pairs of resource data id and amount for the helper's farm, written as a counted list after the required items.</value>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair> GrantedRewards { get; }

    /// <summary>
    /// Gets the unique service-help request id.
    /// </summary>
    /// <value>Help id the owner's town assigned when it asked for help, written as a VarInt after the two home ids.</value>
    [JsonPropertyName("HelpId")]
    public int HelpId { get; }

    /// <summary>
    /// Gets the helper's home id.
    /// </summary>
    /// <value>Home id of the farm that served the passenger, written right after the owner's home id.</value>
    [JsonPropertyName("HelperHomeId")]
    public LongId HelperHomeId { get; }

    /// <summary>
    /// Gets the home owning the service-help request.
    /// </summary>
    /// <value>Home id of the town whose passenger was served, the first field on the wire.</value>
    [JsonPropertyName("OwnerHomeId")]
    public LongId OwnerHomeId { get; }

    /// <summary>
    /// Gets the item quantities consumed by the completed service.
    /// </summary>
    /// <value>Pairs of good data id and amount from the helper's inventory, written as a counted list after the help id.</value>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair> RequiredItems { get; }
}
