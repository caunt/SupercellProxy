using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>
/// Defines the Load Farm Layouts Server Command contract.
/// </summary>
/// <summary>
/// Defines the Game Mode contract.
/// </summary>
/// <summary>
/// Defines the Compressed Layouts contract.
/// </summary>
public sealed record LoadFarmLayoutsServerCommand(int GameMode, ReadOnlyMemory<byte>? CompressedLayouts) : ServerCommand
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.LoadFarmLayoutsCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static LoadFarmLayoutsServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        int mode = stream.ReadVarInt();
        ReadOnlyMemory<byte>? layouts = stream.ReadBoolean() ? stream.ReadByteArray() : null;

        return new LoadFarmLayoutsServerCommand(mode, layouts);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(GameMode);
        stream.WriteBoolean(CompressedLayouts is not null);

        if (CompressedLayouts is { } layouts)
            stream.WriteByteArray(layouts.Span);
    }
}
