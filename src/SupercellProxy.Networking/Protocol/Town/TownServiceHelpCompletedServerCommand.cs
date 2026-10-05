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
        LongIdentifier ownerHomeIdentifier,
        LongIdentifier helperHomeIdentifier,
        int helpIdentifier,
        ReadOnlyMemory<CommandDataReferenceVariableIntPair> requiredItems,
        ReadOnlyMemory<CommandDataReferenceVariableIntPair> grantedRewards,
        int serverCommandIdentifier,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(serverCommandIdentifier, executionPhaseCounter, debugData0, debugData1)
    {
        OwnerHomeIdentifier = ownerHomeIdentifier;
        HelperHomeIdentifier = helperHomeIdentifier;
        HelpIdentifier = helpIdentifier;
        RequiredItems = requiredItems.ToArray();
        GrantedRewards = grantedRewards.ToArray();
    }

    /// <summary>
    /// <para>Gets the resource quantities granted for the completed service.</para>
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVariableIntPair> GrantedRewards { get; }

    /// <summary>
    /// <para>Gets the unique service-help request identifier.</para>
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("HelpId")]
    public int HelpIdentifier { get; }

    /// <summary>
    /// <para>Gets the helper's home identifier.</para>
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("HelperHomeId")]
    public LongIdentifier HelperHomeIdentifier { get; }

    /// <summary>
    /// <para>Gets the home owning the service-help request.</para>
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("OwnerHomeId")]
    public LongIdentifier OwnerHomeIdentifier { get; }

    /// <summary>
    /// <para>Gets the item quantities consumed by the completed service.</para>
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVariableIntPair> RequiredItems { get; }

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
        LongIdentifier ownerHomeIdentifier = stream.ReadLongIdentifier();
        LongIdentifier helperHomeIdentifier = stream.ReadLongIdentifier();
        int helpIdentifier = stream.ReadVariableInt();
        ReadOnlyMemory<CommandDataReferenceVariableIntPair> requiredItems = CommandDataReferenceVariableIntPairArrayField.Decode(stream).Values;
        ReadOnlyMemory<CommandDataReferenceVariableIntPair> grantedRewards = CommandDataReferenceVariableIntPairArrayField.Decode(stream).Values;
        (int serverCommandIdentifier, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields) = DecodeServerCommand(stream, environment);

        return new TownServiceHelpCompletedServerCommand(
            ownerHomeIdentifier,
            helperHomeIdentifier,
            helpIdentifier,
            requiredItems,
            grantedRewards,
            serverCommandIdentifier,
            commandFields.ExecutionPhaseCounter,
            commandFields.DebugData0,
            commandFields.DebugData1
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongIdentifier(OwnerHomeIdentifier);
        stream.WriteLongIdentifier(HelperHomeIdentifier);
        stream.WriteVariableInt(HelpIdentifier);
        new CommandDataReferenceVariableIntPairArrayField(RequiredItems).Encode(stream);
        new CommandDataReferenceVariableIntPairArrayField(GrantedRewards).Encode(stream);
        EncodeServerCommand(stream, environment);
    }
}
