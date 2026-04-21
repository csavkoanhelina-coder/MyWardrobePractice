using System;
using System.Windows;
using System.Windows.Controls;
using MyWardrobe.Services;

namespace MyWardrobe.Views
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            LightThemeButton.Click += Light_Click;
            DarkThemeButton.Click += Dark_Click;
            UkrainianButton.Click += UA_Click;
            EnglishButton.Click += EN_Click;
        }

        private void Light_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme("Light");
            SettingsService.SaveTheme("Light");
        }

        private void Dark_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme("Dark");
            SettingsService.SaveTheme("Dark");
        }

        private void ApplyTheme(string theme)
        {
            try
            {
                var dict = new ResourceDictionary();
                dict.Source = new Uri($"/Resources/{theme}Theme.xaml", UriKind.Relative);

                var toRemove = new System.Collections.Generic.List<ResourceDictionary>();
                foreach (var d in Application.Current.Resources.MergedDictionaries)
                {
                    if (d.Source != null && (d.Source.ToString().Contains("LightTheme") || d.Source.ToString().Contains("DarkTheme")))
                    {
                        toRemove.Add(d);
                    }
                }
                foreach (var d in toRemove)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(d);
                }

                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка зміни теми: {ex.Message}");
            }
        }

        private void UA_Click(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("UA");
            SettingsService.SaveLanguage("UA");
        }

        private void EN_Click(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("EN");
            SettingsService.SaveLanguage("EN");
        }

        private void ChangeLanguage(string lang)
        {
            try
            {
                var dict = new ResourceDictionary();
                dict.Source = new Uri($"/Resources/StringResources.{lang.ToLower()}.xaml", UriKind.Relative);

                var toRemove = new System.Collections.Generic.List<ResourceDictionary>();
                foreach (var d in Application.Current.Resources.MergedDictionaries)
                {
                    if (d.Source != null && d.Source.ToString().Contains("StringResources"))
                    {
                        toRemove.Add(d);
                    }
                }
                foreach (var d in toRemove)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(d);
                }

                Application.Current.Resources.MergedDictionaries.Add(dict);

                if (Application.Current.MainWindow != null)
                {
                    Application.Current.MainWindow.Title = lang == "EN" ? "My Wardrobe" : "Мій гардероб";
                }

                MessageBox.Show(lang == "EN" ? "Language changed!" : "Мову змінено!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка зміни мови: {ex.Message}");
            }
        }
    }
}