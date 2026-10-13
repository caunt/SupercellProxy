using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Requests the selected waiting passengers from the currently visited Town.</summary>
public sealed record RequestTownPassengerPickupMessage(int[]? HelpIds) : CodecMessage<RequestTownPassengerPickupMessage>, ICodecProvider<RequestTownPassengerPickupMessage>
{
    private const int MaximumHelpIdCount = 10_000;

    /// <inheritdoc />
    /// <remarks>Native treats a negative count, or one above 10000, as no list and stops reading.</remarks>
    public static ICodec<RequestTownPassengerPickupMessage> Codec { get; } = Codecs.Create(
        static stream =>
        {
            int count = stream.ReadVarInt();

            if (count is < 0 or > MaximumHelpIdCount)
                return new RequestTownPassengerPickupMessage(HelpIds: null);

            int[] helpIds = new int[count];

            for (int index = 0; index < helpIds.Length; index++)
                helpIds[index] = stream.ReadVarInt();

            return new RequestTownPassengerPickupMessage(helpIds);
        },
        static (stream, message) => HelpIdsCodec.Write(stream, message.HelpIds)
    );

    private static ICodec<int[]?> HelpIdsCodec => field ??= Codecs.NullableArray(Codecs.VarInt, CollectionCount.Unbounded);
}
