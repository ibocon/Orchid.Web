using System.Text;
using Garden.Razor;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Orchid.Web.Component;

/// <summary>
/// 웹 대시보드 환경을 위한 Garden 디자인 시스템 및 MudBlazor 테마 동기화 프로바이더입니다.
/// </summary>
public partial class OrchidWebDesignSystemProvider : DesignSystemProvider<DesignSystem>, IAsyncDisposable
{
    [Inject]
    internal DesignSystem DsgnSys { get; set; } = default!;

    /// <summary>
    /// Garden Scheme과 연동된 현재 MudBlazor 테마를 가져옵니다.
    /// </summary>
    public MudTheme CurrentMudTheme { get; private set; } = new();

    /// <summary>
    /// 현재 다크 모드 여부를 가져옵니다.
    /// </summary>
    public bool IsDarkMode => DsgnSys.Color.SchemeSet.Context == SchemeContext.Dark;

    /// <summary>
    /// 현재 적용된 색상 문맥을 가져옵니다.
    /// </summary>
    public SchemeContext CurrentSchemeContext => DsgnSys.Color.SchemeSet.Context;

    /// <summary>
    /// 라이트/다크 테마를 상호 전환합니다.
    /// </summary>
    public void ToggleTheme()
    {
        SetTheme(IsDarkMode ? SchemeContext.Light : SchemeContext.Dark);
    }

    /// <summary>
    /// 지정한 색상 문맥으로 테마를 변경합니다.
    /// </summary>
    /// <param name="context">변경할 색상 문맥입니다.</param>
    public void SetTheme(SchemeContext context)
    {
        DsgnSys.Color.SchemeSet.ChangeContext(context);
        ApplyMudThemeFromDesignSystem();
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        DsgnSys.Color.SchemeSet.ContextChanged -= OnSchemeContextChanged;
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        DsgnSys.Color.SchemeSet.ChangeContext(SchemeContext.Light);
        ApplyMudThemeFromDesignSystem();
        DsgnSys.Color.SchemeSet.ContextChanged += OnSchemeContextChanged;
    }

    /// <inheritdoc />
    protected override void BuildCustom(StringBuilder builder)
    {
        builder
            .AppendLine("body {")
            .AppendTab().AppendLine("background-color: var(--gdn-sys-color-surface);")
            .AppendTab().AppendLine("color: var(--gdn-sys-color-on-surface);")
            .AppendLine("}");
    }

    private void OnSchemeContextChanged(object? sender, SchemeContextChangedEventArgs e)
    {
        ApplyMudThemeFromDesignSystem();
        InvokeAsync(StateHasChanged);
    }

    private void ApplyMudThemeFromDesignSystem()
    {
        Scheme scheme = DsgnSys.Color.SchemeSet.Current;
        CurrentMudTheme = new MudTheme
        {
            PaletteLight = BuildMudPaletteLight(scheme),
            PaletteDark = BuildMudPaletteDark(scheme),
        };
    }

    private static PaletteLight BuildMudPaletteLight(Scheme scheme)
    {
        return new()
        {
            Primary = new MudBlazor.Utilities.MudColor(scheme.Primary.ToHex()),
            PrimaryContrastText = new MudBlazor.Utilities.MudColor(scheme.OnPrimary.ToHex()),
            Secondary = new MudBlazor.Utilities.MudColor(scheme.Secondary.ToHex()),
            SecondaryContrastText = new MudBlazor.Utilities.MudColor(scheme.OnSecondary.ToHex()),
            Tertiary = new MudBlazor.Utilities.MudColor(scheme.Tertiary.ToHex()),
            TertiaryContrastText = new MudBlazor.Utilities.MudColor(scheme.OnTertiary.ToHex()),
            Error = new MudBlazor.Utilities.MudColor(scheme.Error.ToHex()),
            ErrorContrastText = new MudBlazor.Utilities.MudColor(scheme.OnError.ToHex()),
            Surface = new MudBlazor.Utilities.MudColor(scheme.Surface.ToHex()),
            Background = new MudBlazor.Utilities.MudColor(scheme.Surface.ToHex()),
            BackgroundGray = new MudBlazor.Utilities.MudColor(scheme.SurfaceContainerHighest.ToHex()),
            AppbarBackground = new MudBlazor.Utilities.MudColor(scheme.Background.ToHex()),
            TextPrimary = new MudBlazor.Utilities.MudColor(scheme.OnSurface.ToHex()),
            TextSecondary = new MudBlazor.Utilities.MudColor(scheme.OnSurfaceVariant.ToHex()),
            TextDisabled = new MudBlazor.Utilities.MudColor(scheme.Outline.ToHex()),
            ActionDefault = new MudBlazor.Utilities.MudColor(scheme.OnSurfaceVariant.ToHex()),
            ActionDisabled = new MudBlazor.Utilities.MudColor(scheme.Outline.ToHex()),
            Divider = new MudBlazor.Utilities.MudColor(scheme.OutlineVariant.ToHex()),
            LinesDefault = new MudBlazor.Utilities.MudColor(scheme.OutlineVariant.ToHex()),
            DrawerBackground = new MudBlazor.Utilities.MudColor(scheme.SurfaceContainer.ToHex()),
            DrawerText = new MudBlazor.Utilities.MudColor(scheme.OnSurface.ToHex()),
        };
    }

    private static PaletteDark BuildMudPaletteDark(Scheme scheme)
    {
        return new()
        {
            Primary = new MudBlazor.Utilities.MudColor(scheme.Primary.ToHex()),
            PrimaryContrastText = new MudBlazor.Utilities.MudColor(scheme.OnPrimary.ToHex()),
            Secondary = new MudBlazor.Utilities.MudColor(scheme.Secondary.ToHex()),
            SecondaryContrastText = new MudBlazor.Utilities.MudColor(scheme.OnSecondary.ToHex()),
            Tertiary = new MudBlazor.Utilities.MudColor(scheme.Tertiary.ToHex()),
            TertiaryContrastText = new MudBlazor.Utilities.MudColor(scheme.OnTertiary.ToHex()),
            Error = new MudBlazor.Utilities.MudColor(scheme.Error.ToHex()),
            ErrorContrastText = new MudBlazor.Utilities.MudColor(scheme.OnError.ToHex()),
            Surface = new MudBlazor.Utilities.MudColor(scheme.Surface.ToHex()),
            Background = new MudBlazor.Utilities.MudColor(scheme.Surface.ToHex()),
            BackgroundGray = new MudBlazor.Utilities.MudColor(scheme.SurfaceContainerHighest.ToHex()),
            AppbarBackground = new MudBlazor.Utilities.MudColor(scheme.Background.ToHex()),
            TextPrimary = new MudBlazor.Utilities.MudColor(scheme.OnSurface.ToHex()),
            TextSecondary = new MudBlazor.Utilities.MudColor(scheme.OnSurfaceVariant.ToHex()),
            TextDisabled = new MudBlazor.Utilities.MudColor(scheme.Outline.ToHex()),
            ActionDefault = new MudBlazor.Utilities.MudColor(scheme.OnSurfaceVariant.ToHex()),
            ActionDisabled = new MudBlazor.Utilities.MudColor(scheme.Outline.ToHex()),
            Divider = new MudBlazor.Utilities.MudColor(scheme.OutlineVariant.ToHex()),
            LinesDefault = new MudBlazor.Utilities.MudColor(scheme.OutlineVariant.ToHex()),
            DrawerBackground = new MudBlazor.Utilities.MudColor(scheme.SurfaceContainer.ToHex()),
            DrawerText = new MudBlazor.Utilities.MudColor(scheme.OnSurface.ToHex()),
        };
    }
}
