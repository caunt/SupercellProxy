namespace SupercellProxy.Networking.Sessions.Anonymous;

/// <summary>The most recently observed public farm information for an anonymous account.</summary>
public sealed record AnonymousAccountProfile(string? Name, int Level, int Experience);
