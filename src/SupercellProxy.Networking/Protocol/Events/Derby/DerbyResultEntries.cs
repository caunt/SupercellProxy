using System.Collections.Immutable;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Preserves the optional native result-list owner and its independently nullable list.</summary>
/// <param name="Entries">The result entries in wire order, or <see langword="null"/> when native wrote the -1 absent count; an empty array is a present but empty list. Up to 10000 entries are accepted, and a count below -1 is rejected as malformed.</param>
public sealed record DerbyResultEntries(ImmutableArray<DerbyResultEntry>? Entries) : ICodecProvider<DerbyResultEntries>
{
    /// <inheritdoc />
    public static ICodec<DerbyResultEntries> Codec { get; } = Codecs.Composite(Codecs.NullableArray(DerbyResultEntry.Codec, CollectionCount.AtMost(maximum: 10000).WithStrictAbsence()),
        static entries => entries.Entries,
        static entries => new DerbyResultEntries(entries));
}
