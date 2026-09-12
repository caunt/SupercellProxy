using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>
/// <para>Settles a completed Town passenger service.</para>
/// </summary>
public sealed record PassengerServiceCompletionServerCommand : ServerCommand
{
    /// <summary>
    /// Provides the Passenger Service Completion Server Command value or operation.
    /// </summary>
    public PassengerServiceCompletionServerCommand(
        LongIdentifier sourceHomeIdentifier,
        LongIdentifier destinationHomeIdentifier,
        int passengerInstanceIdentifier,
        ReadOnlyMemory<CommandDataReferenceVariableIntPair> requiredItems,
        ReadOnlyMemory<CommandDataReferenceVariableIntPair> grantedRewards,
        int serverCommandIdentifier,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(serverCommandIdentifier, executionPhaseCounter, debugData0, debugData1)
    {
        SourceHomeIdentifier = sourceHomeIdentifier;
        DestinationHomeIdentifier = destinationHomeIdentifier;
        PassengerInstanceIdentifier = passengerInstanceIdentifier;
        RequiredItems = requiredItems.ToArray();
        GrantedRewards = grantedRewards.ToArray();
    }

    /// <summary>
    /// <para>Gets the home in which the passenger service was completed.</para>
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("DestinationHomeId")]
    public LongIdentifier DestinationHomeIdentifier { get; }

    /// <summary>
    /// <para>Gets the resource quantities granted for the completed service.</para>
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVariableIntPair> GrantedRewards { get; }

    /// <summary>
    /// <para>Gets the serviced passenger's instance identifier.</para>
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("PassengerInstanceId")]
    public int PassengerInstanceIdentifier { get; }

    /// <summary>
    /// <para>Gets the item quantities consumed by the completed service.</para>
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVariableIntPair> RequiredItems { get; }

    /// <summary>
    /// <para>Gets the home from which the serviced passenger originated.</para>
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("SourceHomeId")]
    public LongIdentifier SourceHomeIdentifier { get; }

    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.PassengerServiceCompletionServerCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static PassengerServiceCompletionServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongIdentifier sourceHomeIdentifier = stream.ReadLongIdentifier();
        LongIdentifier destinationHomeIdentifier = stream.ReadLongIdentifier();
        int passengerInstanceIdentifier = stream.ReadVariableInt();
        ReadOnlyMemory<CommandDataReferenceVariableIntPair> requiredItems = CommandDataReferenceVariableIntPairArrayField.Decode(stream).Values;
        ReadOnlyMemory<CommandDataReferenceVariableIntPair> grantedRewards = CommandDataReferenceVariableIntPairArrayField.Decode(stream).Values;
        (int serverCommandIdentifier, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields) = DecodeServerCommand(stream, environment);

        return new PassengerServiceCompletionServerCommand(
            sourceHomeIdentifier,
            destinationHomeIdentifier,
            passengerInstanceIdentifier,
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
        stream.WriteLongIdentifier(SourceHomeIdentifier);
        stream.WriteLongIdentifier(DestinationHomeIdentifier);
        stream.WriteVariableInt(PassengerInstanceIdentifier);
        new CommandDataReferenceVariableIntPairArrayField(RequiredItems).Encode(stream);
        new CommandDataReferenceVariableIntPairArrayField(GrantedRewards).Encode(stream);
        EncodeServerCommand(stream, environment);
    }
}
