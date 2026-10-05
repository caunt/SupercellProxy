using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CropFields;

/// <summary>
/// Defines the Plant Field Command contract.
/// </summary>
/// <summary>
/// Defines the Crop Global Id contract.
/// </summary>
/// <summary>
/// Defines the Buy Seeds contract.
/// </summary>
/// <summary>
/// Defines the Field Global Id contract.
/// </summary>
public sealed record PlantFieldCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("CropGlobalId")] int CropGlobalId,
    bool BuySeeds,
    [property: System.Text.Json.Serialization.JsonPropertyName("FieldGlobalId")] int FieldGlobalId
) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.PlantFieldCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static PlantFieldCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new PlantFieldCommand(stream.ReadVarInt(), stream.ReadBoolean(), stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(CropGlobalId);
        stream.WriteBoolean(BuySeeds);
        stream.WriteVarInt(FieldGlobalId);
    }
}
