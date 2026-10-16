using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Support;

/// <summary>Sends a support request.</summary>
/// <param name="Category">Topic the player picked in the support form.</param>
/// <param name="Issue">Specific problem picked within that topic.</param>
/// <param name="Email">Address support should answer to.</param>
/// <param name="Subject">Title the player gave the request.</param>
/// <param name="Description">The player's own explanation of the problem.</param>
public sealed record ReportSupportTicketMessage(int Category, int Issue, string? Email, string? Subject, string? Description) : CodecMessage<ReportSupportTicketMessage>, ICodecProvider<ReportSupportTicketMessage>
{
    /// <inheritdoc />
    public static ICodec<ReportSupportTicketMessage> Codec { get; } = Codecs.Composite(
        Codecs.Int32,
        static message => message.Category,
        Codecs.Int32,
        static message => message.Issue,
        Codecs.OptionalString,
        static message => message.Email,
        Codecs.OptionalString,
        static message => message.Subject,
        Codecs.OptionalString,
        static message => message.Description,
        static (category, issue, email, subject, description) => new ReportSupportTicketMessage(category, issue, email, subject, description)
    );
}
