using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>
/// <para>Reconciles a town service fulfilled by another farm.</para>
/// </summary>
public sealed record TownServiceHelpCompletedServerCommand : ServerCommand
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

    /// <summary>
    /// <para>Gets the resource quantities granted for the completed service.</para>
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair> GrantedRewards { get; }

    /// <summary>
    /// <para>Gets the unique service-help request id.</para>
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("HelpId")]
    public int HelpId { get; }

    /// <summary>
    /// <para>Gets the helper's home id.</para>
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("HelperHomeId")]
    public LongId HelperHomeId { get; }

    /// <summary>
    /// <para>Gets the home owning the service-help request.</para>
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("OwnerHomeId")]
    public LongId OwnerHomeId { get; }

    /// <summary>
    /// <para>Gets the item quantities consumed by the completed service.</para>
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair> RequiredItems { get; }

    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.TownServiceHelpCompletedServerCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static TownServiceHelpCompletedServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongId ownerHomeId = stream.ReadLongId();
        LongId helperHomeId = stream.ReadLongId();
        int helpId = stream.ReadVarInt();
        ReadOnlyMemory<CommandDataReferenceVarIntPair> requiredItems = CommandDataReferenceVarIntPairArrayField.Decode(stream).Values;
        ReadOnlyMemory<CommandDataReferenceVarIntPair> grantedRewards = CommandDataReferenceVarIntPairArrayField.Decode(stream).Values;


        return new TownServiceHelpCompletedServerCommand(ownerHomeId, helperHomeId, helpId, requiredItems, grantedRewards);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongId(OwnerHomeId);
        stream.WriteLongId(HelperHomeId);
        stream.WriteVarInt(HelpId);
        new CommandDataReferenceVarIntPairArrayField(RequiredItems).Encode(stream);
        new CommandDataReferenceVarIntPairArrayField(GrantedRewards).Encode(stream);
    }
}
