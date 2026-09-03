namespace SupercellProxy.Keys.Tests;

public sealed class AppVersionTests
{
    [Theory]
    [InlineData("15.535.22", "15.535.22")]
    [InlineData("v15.535.22", "15.535.22")]
    [InlineData("VV15.535.22", "15.535.22")]
    [InlineData("version-15.535.22", "version-15.535.22")]
    [InlineData("v15.535.22-beta", "v15.535.22-beta")]
    public void NormalizeRemovesOnlyNumericVersionPrefixes(string source, string expected)
    {
        Assert.Equal(expected, AppVersion.Normalize(source));
    }

    [Fact]
    public void CreateManyGroupsAliasesAndPrefersTheCanonicalSourceName()
    {
        var versions = AppVersion.CreateMany(["v15.535.3", "15.535.3", "v15.535.3"]);

        var version = Assert.Single(versions);
        Assert.Equal("15.535.3", version.Value);
        Assert.Equal(["15.535.3", "v15.535.3"], version.SourceNames);
    }

    [Fact]
    public void ValueComparerOrdersNumericComponentsRatherThanText()
    {
        string[] versions = ["15.535.3", "16.402.2", "15.535.22", "15.535.29"];

        var ordered = versions.OrderDescending(AppVersion.ValueComparer);

        Assert.Equal(
            ["16.402.2", "15.535.29", "15.535.22", "15.535.3"],
            ordered,
            StringComparer.Ordinal
        );
    }
}
