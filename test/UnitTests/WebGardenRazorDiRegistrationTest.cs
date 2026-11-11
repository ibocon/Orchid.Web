using FluentAssertions;
using Garden.Razor;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Xunit;

namespace Orchid.UnitTests.Web;

public sealed class WebGardenRazorDiRegistrationTest
{
    [Fact]
    public void GivenWebServiceCollection_WhenRegisteringGardenRazor_ThenServicesAreResolvable()
    {
        // Given
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddLocalization();
        services.AddMudServices();
        services.AddGardenRazor();
        services.AddDesignSystem();

        // When
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
        using var scope = provider.CreateScope();

        // Then
        var dialogService = scope.ServiceProvider.GetService<MudBlazor.IDialogService>();
        var presenter = scope.ServiceProvider.GetService<ExceptionPresenter>();
        var designSystem = scope.ServiceProvider.GetService<DesignSystem>();

        dialogService.Should().NotBeNull();
        presenter.Should().NotBeNull();
        designSystem.Should().NotBeNull();
    }

    [Fact]
    public void IndexHtmlShouldDeclareOrchidSubpathBaseHref()
    {
        // Given
        string repoRoot = FindRepositoryRoot();
        string html = File.ReadAllText(Path.Combine(repoRoot, "src/wwwroot/index.html"));

        // When & Then
        html.Should().Contain("<base href=\"/orchid/\" />");
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
