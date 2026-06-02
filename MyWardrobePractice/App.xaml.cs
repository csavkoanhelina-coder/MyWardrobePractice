using System.Windows;
using MyWardrobe.Services;
using MyWardrobe.Views;

namespace MyWardrobe
{
    /// <summary>
    /// Головний клас додатку.
    /// Завантажує збережені налаштування теми та мови,
    /// запускає вікно входу замість головного вікна.
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string savedTheme = SettingsService.LoadTheme();
            ApplyTheme(savedTheme);

            string savedLanguage = SettingsService.LoadLanguage();
            ApplyLanguage(savedLanguage);

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        /// <summary>
        /// Застосовує вказану тему оформлення (світлу або темну).
        /// </summary>
        private void ApplyTheme(string theme)
        {
            try
            {
                var dict = new ResourceDictionary();
                dict.Source = new System.Uri($"/Resources/{theme}Theme.xaml", System.UriKind.Relative);

                var toRemove = new System.Collections.Generic.List<ResourceDictionary>();
                foreach (var d in Resources.MergedDictionaries)
                {
                    if (d.Source != null && (d.Source.ToString().Contains("LightTheme") || d.Source.ToString().Contains("DarkTheme")))
                    {
                        toRemove.Add(d);
                    }
                }
                foreach (var d in toRemove)
                {
                    Resources.MergedDictionaries.Remove(d);
                }

                Resources.MergedDictionaries.Add(dict);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Застосовує вказану мову інтерфейсу (українську або англійську).
        /// </summary>
        private void ApplyLanguage(string lang)
        {
            try
            {
                var dict = new ResourceDictionary();
                dict.Source = new System.Uri($"/Resources/StringResources.{lang.ToLower()}.xaml", System.UriKind.Relative);

                var toRemove = new System.Collections.Generic.List<ResourceDictionary>();
                foreach (var d in Resources.MergedDictionaries)
                {
                    if (d.Source != null && d.Source.ToString().Contains("StringResources"))
                    {
                        toRemove.Add(d);
                    }
                }
                foreach (var d in toRemove)
                {
                    Resources.MergedDictionaries.Remove(d);
                }

                Resources.MergedDictionaries.Add(dict);
            }
            catch
            {
            }
        }
    }
}