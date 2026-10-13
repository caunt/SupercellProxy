namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>Where native places the shared command metadata relative to a command's own fields.</summary>
public enum CommandLayout
{
    /// <summary>The command's fields come first and the metadata follows them.</summary>
    FieldsFirst,

    /// <summary>The metadata comes first and the command's fields follow it.</summary>
    MetadataFirst,
}
