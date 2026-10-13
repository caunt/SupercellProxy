using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// The JSON configurations of the native <c language="csharp">LogicLiveGlobalManager</c> globals GENERAL, SPINWHEEL and RENOVATOR.
/// </summary>
public sealed record LiveGlobalConfigurations(string? General, string? SpinWheel, string? Renovator) : ICodecProvider<LiveGlobalConfigurations>
{
    /// <summary>
    /// Initializes a new <see cref="LiveGlobalConfigurations"/> instance.
    /// </summary>
    public LiveGlobalConfigurations()
        : this(General: null, SpinWheel: null, Renovator: null) { }

    /// <inheritdoc />
    public static ICodec<LiveGlobalConfigurations> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static value => value.General,
        Codecs.OptionalString,
        static value => value.SpinWheel,
        Codecs.OptionalString,
        static value => value.Renovator,
        static (general, spinWheel, renovator) => new LiveGlobalConfigurations(general, spinWheel, renovator)
    );
}
