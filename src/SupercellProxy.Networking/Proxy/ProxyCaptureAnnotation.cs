namespace SupercellProxy.Networking.Proxy;

/// <summary>Describes text appended beside a captured frame.</summary>
public sealed record ProxyCaptureAnnotation(string Directory, string File, string Text);
