namespace SupercellProxy.Playground.Network.Messages;

/// <summary>
/// Defines the <c language="csharp">IMessage</c> contract.
/// </summary>
internal interface IMessage
{
    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0);
}
