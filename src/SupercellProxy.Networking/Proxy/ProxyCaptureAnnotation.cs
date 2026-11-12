namespace SupercellProxy.Networking.Proxy;

/// <summary>Describes text appended beside a captured frame.</summary>
/// <param name="Directory">Subfolder of the connection's capture folder that holds the annotation file; must be a single path segment, not <c>.</c> or <c>..</c>.</param>
/// <param name="File">Name of the file inside <paramref name="Directory"/> the text is appended to, shared by every frame that names it; must be a single path segment.</param>
/// <param name="Text">UTF-8 text appended as-is after the frame is written, such as one JSON line ending in a newline; it counts toward the 64 MiB pending limit.</param>
public sealed record ProxyCaptureAnnotation(string Directory, string File, string Text);
