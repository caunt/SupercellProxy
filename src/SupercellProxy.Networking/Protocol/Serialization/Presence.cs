namespace SupercellProxy.Networking.Protocol.Serialization;

/// <summary>A value whose presence native records separately from the value itself, so a present value may still be empty.</summary>
/// <typeparam name="TValue">The value type.</typeparam>
/// <param name="IsPresent">Whether native wrote the value.</param>
/// <param name="Value">The value, or its default when absent.</param>
public readonly record struct Presence<TValue>(bool IsPresent, TValue Value);
