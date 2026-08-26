namespace SupercellProxy.Playground.Network.Messages;

/// <summary>
/// Defines the <c>IMessage</c> contract.
/// </summary>
public interface IMessage
{
    /// <summary>
    /// Executes the <c>ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0);
}
