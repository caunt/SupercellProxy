using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Defines the Construct Game Object Command contract.
/// </summary>
/// <summary>
/// Defines the Position X contract.
/// </summary>
/// <summary>
/// Defines the Variant contract.
/// </summary>
/// <summary>
/// Defines the Position Y contract.
/// </summary>
/// <summary>
/// Defines the Layout Mode contract.
/// </summary>
/// <summary>
/// Defines the Replaced Object Global Id contract.
/// </summary>
/// <summary>
/// Defines the Target Data Global Id contract.
/// </summary>
/// <summary>
/// Defines the Mirrored contract.
/// </summary>
public sealed record ConstructGameObjectCommand(
    int PositionX,
    int Variant,
    int PositionY,
    bool LayoutMode,
    [property: System.Text.Json.Serialization.JsonPropertyName("ReplacedObjectGlobalId")] int ReplacedObjectGlobalId,
    [property: System.Text.Json.Serialization.JsonPropertyName("TargetDataGlobalId")] int TargetDataGlobalId,
    bool Mirrored
) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.ConstructGameObjectCommandType;

    internal static Version ReorderedFieldsVersion { get; } = new(major: 1, minor: 73, build: 81);

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static ConstructGameObjectCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (stream.GameVersion >= ReorderedFieldsVersion)
        {
            bool layoutMode = stream.ReadBoolean();
            int variant = stream.ReadVarInt();
            int targetDataGlobalId = stream.ReadVarInt();
            int replacedObjectGlobalId = stream.ReadVarInt();
            int positionY = stream.ReadVarInt();
            int positionX = stream.ReadVarInt();

            return new ConstructGameObjectCommand(positionX, variant, positionY, layoutMode, replacedObjectGlobalId, targetDataGlobalId, stream.ReadBoolean());
        }

        return new ConstructGameObjectCommand(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadBoolean(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadBoolean()
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        if (stream.GameVersion >= ReorderedFieldsVersion)
        {
            stream.WriteBoolean(LayoutMode);
            stream.WriteVarInt(Variant);
            stream.WriteVarInt(TargetDataGlobalId);
            stream.WriteVarInt(ReplacedObjectGlobalId);
            stream.WriteVarInt(PositionY);
            stream.WriteVarInt(PositionX);
            stream.WriteBoolean(Mirrored);

            return;
        }

        stream.WriteVarInt(PositionX);
        stream.WriteVarInt(Variant);
        stream.WriteVarInt(PositionY);
        stream.WriteBoolean(LayoutMode);
        stream.WriteVarInt(ReplacedObjectGlobalId);
        stream.WriteVarInt(TargetDataGlobalId);
        stream.WriteBoolean(Mirrored);
    }
}
