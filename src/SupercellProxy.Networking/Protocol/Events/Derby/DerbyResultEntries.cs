using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Preserves the optional native result-list owner and its independently nullable list.</summary>
public sealed record DerbyResultEntries(DerbyResultEntry[]? Entries) : ICodecProvider<DerbyResultEntries>
{
    /// <inheritdoc />
    public static ICodec<DerbyResultEntries> Codec { get; } = Codecs.Composite(
        Codecs.NullableArray(DerbyResultEntry.Codec, CollectionCount.AtMost(maximum: 10000).WithStrictAbsence()),
        static entries => entries.Entries,
        static entries => new DerbyResultEntries(entries)
    );
}
