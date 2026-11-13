// keep: FluentAssertions assertions
using FluentAssertions;
// keep: xUnit Fact attribute
using Xunit;

namespace Orchid.UnitTests.Web;

/// <summary>
/// Orchid.Web의 모든 Razor 뷰가 원시 MudBlazor UI 컴포넌트 대신 Garden.Razor 컴포넌트를 사용하는지 검증합니다.
/// </summary>
public sealed class WebGardenRazorComponentAdoptionTest
{
    [Fact]
    public void WebRazorViewsShouldUseGardenRazorComponentsInsteadOfRawMudUiTags()
    {
        // Given
        string repoRoot = FindRepositoryRoot();
        string appRazor = File.ReadAllText(Path.Combine(repoRoot, "src/App.razor"));
        string indexRazor = File.ReadAllText(Path.Combine(repoRoot, "src/Feature/History/Index.razor"));
        string gridRazor = File.ReadAllText(Path.Combine(repoRoot, "src/Component/VirtualizedHistoryGrid/VirtualizedHistoryGrid.razor"));

        // When & Then
        // ✅ Garden.Razor 표준 컴포넌트가 사용되고 원시 MudPaper/MudText/MudTable/MudChip/MudAlert/MudSnackbarProvider가 제거된다
        appRazor.Should().Contain("<GardenSnackbarProvider");
        appRazor.Should().NotContain("<MudSnackbarProvider");
        appRazor.Should().NotContain("<MudAlert");

        indexRazor.Should().Contain("<GardenTopAppBar");
        indexRazor.Should().Contain("<GardenSearch");
        indexRazor.Should().Contain("<GardenAccountMenu");
        indexRazor.Should().Contain("<GardenCard");
        indexRazor.Should().Contain("<GardenTypography");
        indexRazor.Should().NotContain("<MudPaper");
        indexRazor.Should().NotContain("<MudText");

        gridRazor.Should().Contain("<GardenTable");
        gridRazor.Should().Contain("<GardenChipAssist");
        gridRazor.Should().Contain("<GardenTypography");
        gridRazor.Should().NotContain("<MudTable");
        gridRazor.Should().NotContain("<MudChip");
        gridRazor.Should().NotContain("<MudText");
    }

    [Fact]
    public void IndexHtmlShouldIncludeBlazorScopedCssBundleForGardenRazor()
    {
        // Given
        string repoRoot = FindRepositoryRoot();
        string indexHtml = File.ReadAllText(Path.Combine(repoRoot, "src/wwwroot/index.html"));

        // When & Then
        // ✅ Garden.Razor.bundle.scp.css를 로드하는 Orchid.Web.styles.css 링크가 존재한다
        indexHtml.Should().Contain("Orchid.Web.styles.css");
        indexHtml.Should().Contain("_content/Garden.Razor/Garden.Razor.min.css");
    }

    private static string FindRepositoryRoot()
    {
        DirectoryInfo? dir = new(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "Orchid.Web.slnx")))
        {
            dir = dir.Parent;
        }

        return dir?.FullName ?? throw new InvalidOperationException("Orchid.Web.slnx not found.");
    }
}
