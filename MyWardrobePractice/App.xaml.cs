using System.Windows;
using MyWardrobe.Services;

namespace MyWardrobe
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string savedTheme = SettingsService.LoadTheme();
            ApplyTheme(savedTheme);

            string savedLanguage = SettingsService.LoadLanguage();
            ApplyLanguage(savedLanguage);
        }

        private void ApplyTheme(string theme)
        {
            try
            {
                var dict = new ResourceDictionary();
                dict.Source = new Uri($"/Resources/{theme}Theme.xaml", UriKind.Relative);

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
            catch { }
        }

        private void ApplyLanguage(string lang)
        {
            try
            {
                var dict = new ResourceDictionary();
                dict.Source = new Uri($"/Resources/StringResources.{lang.ToLower()}.xaml", UriKind.Relative);

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
            catch { }
        }
    }
}