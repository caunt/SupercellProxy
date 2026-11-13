using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// The JSON configurations of the native <c language="csharp">LogicLiveGlobalManager</c> globals GENERAL, SPINWHEEL and RENOVATOR.
/// </summary>
/// <param name="General">The JSON text of the GENERAL live global, written first as an optional string; <see langword="null"/> when the server sent none.</param>
/// <param name="SpinWheel">The JSON text of the SPINWHEEL live global that configures the spin wheel, written second; <see langword="null"/> when the server sent none.</param>
/// <param name="Renovator">The JSON text of the RENOVATOR live global that configures the renovator stand, written last; <see langword="null"/> when the server sent none.</param>
public sealed record LiveGlobalConfigurations(string? General, string? SpinWheel, string? Renovator) : ICodecProvider<LiveGlobalConfigurations>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LiveGlobalConfigurations"/> record. Creates the empty set an avatar has when the server sent no live globals: all three configurations absent.
    /// </summary>
    public LiveGlobalConfigurations()
        : this(General: null, SpinWheel: null, Renovator: null) { }

    /// <inheritdoc />
    public static ICodec<LiveGlobalConfigurations> Codec { get; } = Codecs.Composite(Codecs.OptionalUtf8String,
        static value => value.General,
        Codecs.OptionalUtf8String,
        static value => value.SpinWheel,
        Codecs.OptionalUtf8String,
        static value => value.Renovator,
        static (general, spinWheel, renovator) => new LiveGlobalConfigurations(general, spinWheel, renovator));
}
