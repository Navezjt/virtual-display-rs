using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Virtual_Display_Driver_Control.Common;
using Virtual_Display_Driver_Control.Helpers;
using Windows.System;

namespace Virtual_Display_Driver_Control.Views;

public sealed partial class SettingsView : Page {
    public string AppInfo {
        get {
            var appTitle = Application.Current.Resources["AppTitleName"] as string;

            #if DEBUG
                var versionString = $"{GitVersionInformation.Major}.{GitVersionInformation.Minor}.{GitVersionInformation.Patch}.dev-{GitVersionInformation.ShortSha}";
            #else
                var versionString = $"{GitVersionInformation.Major}.{GitVersionInformation.Minor}.{GitVersionInformation.Patch}-{GitVersionInformation.ShortSha}";
            #endif

            return $"{appTitle} - v{versionString}";
        }
    }

    public SettingsView() {
        InitializeComponent();

        themeMode_load();
        themeMaterial_load();
    }

    private void themeMode_SelectionChanged(object sender, RoutedEventArgs e) {
        var themeString = ((ComboBoxItem)themeMode.SelectedItem)?.Tag?.ToString();

        if (themeString != null) {
            ElementTheme theme;

            switch (themeString) {
                case "Light":
                    theme = ElementTheme.Light;
                    break;
                case "Dark":
                    theme = ElementTheme.Dark;
                    break;
                default:
                    theme = ElementTheme.Default;
                    break;
            }

            ThemeHelper.SetTheme(theme);
        }
    }

    private void themeMode_load() {
        // do not fire callback when we change the index here
        themeMode.SelectionChanged -= themeMode_SelectionChanged;

        var theme = App.Settings.Theme;
        if (theme == ElementTheme.Light) {
            themeMode.SelectedIndex = 0;
        } else if (theme == ElementTheme.Dark) {
            themeMode.SelectedIndex = 1;
        } else {
            themeMode.SelectedIndex = 2;
        }

        themeMode.SelectionChanged += themeMode_SelectionChanged;
    }

    private void themeMaterial_load() {
        if (!MicaController.IsSupported()) {
            ((ComboBoxItem)themeMaterial.Items[0]).IsEnabled = false;
        }

        if (!DesktopAcrylicController.IsSupported()) {
            ((ComboBoxItem)themeMaterial.Items[1]).IsEnabled = false;
        }

        // do not fire callback when we change the index here
        themeMaterial.SelectionChanged -= themeMaterial_SelectionChanged;

        var material = App.Settings.Material;
        if (material == Material.Mica && MicaController.IsSupported()) {
            themeMaterial.SelectedIndex = 0;
        } else if (material == Material.Acrylic && DesktopAcrylicController.IsSupported()) {
            themeMaterial.SelectedIndex = 1;
        } else {
            themeMaterial.SelectedIndex = 2;
        }

        themeMaterial.SelectionChanged += themeMaterial_SelectionChanged;
    }

    private void themeMaterial_SelectionChanged(object sender, RoutedEventArgs e) {
        var selectedMaterial = ((ComboBoxItem)themeMaterial.SelectedItem)?.Tag?.ToString();

        if (selectedMaterial != null) {
            Material material;
            switch (selectedMaterial) {
                case "Mica":
                    material = Material.Mica;
                    break;
                case "Acrylic":
                    material = Material.Acrylic;
                    break;
                default:
                    material = Material.None;
                    break;
            }

            MaterialHelper.SetMaterial(material);
        }
    }

    private async void donate_Click(object sender, RoutedEventArgs e) {
        await Launcher.LaunchUriAsync(new Uri("https://github.com/sponsors/MolotovCherry"));
    }

    private async void homepage_Click(object sender, RoutedEventArgs e) {
        await Launcher.LaunchUriAsync(new Uri("https://github.com/MolotovCherry/virtual-display-rs/"));
    }

    private async void bugFeatureCard_Click(object sender, RoutedEventArgs e) {
        await Launcher.LaunchUriAsync(new Uri("https://github.com/MolotovCherry/virtual-display-rs/issues/new/choose"));
    }
}
