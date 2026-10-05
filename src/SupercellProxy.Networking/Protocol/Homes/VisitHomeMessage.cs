using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Represents the <c language="csharp">VisitHomeMessage</c> protocol message.
/// </summary>
public sealed record VisitHomeMessage : IMessage
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public required byte Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public required byte Unknown1 { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">VisitHomeMessage</c> from the supplied data.
    /// </summary>
    public static VisitHomeMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new VisitHomeMessage
        {
            Unknown0 = stream.ReadByte(),
            Unknown1 = stream.ReadByte(),
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">Encode</c> operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteByte(Unknown0);
        stream.WriteByte(Unknown1);
    }
}
