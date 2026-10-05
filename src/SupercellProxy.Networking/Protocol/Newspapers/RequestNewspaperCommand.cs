using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>
/// Defines the Request Newspaper Command contract.
/// </summary>
/// <summary>
/// Defines the Spend Diamonds contract.
/// </summary>
/// <summary>
/// Defines the Country Code contract.
/// </summary>
/// <summary>
/// Defines the Alternate contract.
/// </summary>
public sealed record RequestNewspaperCommand(bool SpendDiamonds, string CountryCode, bool Alternate) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.RequestNewspaperCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RequestNewspaperCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new RequestNewspaperCommand(stream.ReadBoolean(), stream.ReadString(), stream.ReadBoolean());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteBoolean(SpendDiamonds);
        stream.WriteString(CountryCode);
        stream.WriteBoolean(Alternate);
    }
}
