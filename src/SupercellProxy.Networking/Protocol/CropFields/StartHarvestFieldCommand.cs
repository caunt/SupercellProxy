using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CropFields;

/// <summary>
/// <para>Starts harvesting a ready crop field.</para>
/// </summary>
public sealed record StartHarvestFieldCommand : Command
{
    /// <summary>
    /// Initializes a new <see cref="StartHarvestFieldCommand"/> instance.
    /// </summary>
    public StartHarvestFieldCommand(int fieldGlobalId)
    {
        FieldGlobalId = fieldGlobalId;
    }

    /// <summary>
    /// Gets the <c language="csharp">FieldGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("FieldGlobalId")]
    public int FieldGlobalId { get; }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandRegistry.StartHarvestFieldCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static StartHarvestFieldCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new StartHarvestFieldCommand(stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(FieldGlobalId);
    }
}
