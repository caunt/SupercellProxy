namespace SupercellProxy.Playground.Logic;

/// <summary>
/// <para>Runtime environments encoded by the native Hay Day client.</para>
/// </summary>
internal enum CommandEnvironment
{
    /// <summary>
    /// Identifies the <c language="csharp">Development</c> option.
    /// </summary>
    Development = 0,

    /// <summary>
    /// Identifies the <c language="csharp">Stage</c> option.
    /// </summary>
    Stage = 1,

    /// <summary>
    /// Identifies the <c language="csharp">Unknown</c> option.
    /// </summary>
    Unknown = 2,

    /// <summary>
    /// Identifies the <c language="csharp">Production</c> option.
    /// </summary>
    Production = 3,

    /// <summary>
    /// Identifies the <c language="csharp">LoadTest</c> option.
    /// </summary>
    LoadTest = 4,

    /// <summary>
    /// Identifies the <c language="csharp">ProductionStage</c> option.
    /// </summary>
    ProductionStage = 5,

    /// <summary>
    /// Identifies the <c language="csharp">ProductionStageExternal</c> option.
    /// </summary>
    ProductionStageExternal = 6,
}
