using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Represents the <c language="csharp">VisitHomeTargetMessage</c> protocol message.
/// </summary>
public sealed record VisitHomeTargetMessage : IMessage
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Target</c> value.
    /// </summary>
    public required LongId Target { get; init; }
    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public required byte Unknown0 { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">VisitHomeTargetMessage</c> from the supplied data.
    /// </summary>
    public static VisitHomeTargetMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new VisitHomeTargetMessage
        {
            Unknown0 = stream.ReadByte(),
            Target = stream.ReadLongId(),
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">Encode</c> operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteByte(Unknown0);
        stream.WriteLongId(Target);
    }
}
