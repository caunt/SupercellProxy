namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>
/// Defines the Message Registry Entry contract.
/// </summary>
/// <summary>
/// Defines the Type contract.
/// </summary>
/// <summary>
/// Defines the Factory contract.
/// </summary>
public sealed record MessageRegistryEntry(Type Type, Func<MessageContainer, IMessage> Factory)
{
    /// <summary>Gets the stable message label used in persisted capture filenames.</summary>
    public string CaptureName { get; init; } = Type.Name;
}
