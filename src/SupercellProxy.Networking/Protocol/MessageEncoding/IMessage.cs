namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>
/// Defines the <c language="csharp">IMessage</c> contract.
/// </summary>
public interface IMessage
{
    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    MessageContainer ToContainer(ushort identifier, ushort version = 0);
}
