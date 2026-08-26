namespace SupercellProxy.Playground.Logic;

/// <summary>
/// <para>Runtime environments encoded by the native Hay Day client.</para>
/// </summary>
public enum CommandEnvironment
{
    /// <summary>
    /// Identifies the <c>Development</c> option.
    /// </summary>
    Development = 0,

    /// <summary>
    /// Identifies the <c>Stage</c> option.
    /// </summary>
    Stage = 1,

    /// <summary>
    /// Identifies the <c>Unknown</c> option.
    /// </summary>
    Unknown = 2,

    /// <summary>
    /// Identifies the <c>Production</c> option.
    /// </summary>
    Production = 3,

    /// <summary>
    /// Identifies the <c>LoadTest</c> option.
    /// </summary>
    LoadTest = 4,

    /// <summary>
    /// Identifies the <c>ProductionStage</c> option.
    /// </summary>
    ProductionStage = 5,

    /// <summary>
    /// Identifies the <c>ProductionStageExternal</c> option.
    /// </summary>
    ProductionStageExternal = 6,
}
