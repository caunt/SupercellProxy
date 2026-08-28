using SupercellProxy.Playground.Network.Messages.Clientbound;

namespace SupercellProxy.Playground.Social;

internal sealed class Friends
{
    public Memory<byte> FriendMetaRecords { get; private set; }

    public void Apply(Clientbound26199Message message)
    {
        ArgumentNullException.ThrowIfNull(message);
        FriendMetaRecords = message.FriendMetaRecords.ToArray();
    }
}
