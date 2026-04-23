using System;
using System.Windows;
using System.Windows.Controls;
using MyWardrobe.Services;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Сторінка налаштувань, яка дозволяє користувачеві змінювати тему оформлення (світла/темна)
    /// та мову інтерфейсу (українська/англійська). Налаштування зберігаються між сеансами.
    /// </summary>
    public partial class SettingsPage : Page
    {
        /// <summary>
        /// Ініціалізує компоненти сторінки та підписує обробники подій для кнопок.
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();

            LightThemeButton.Click += Light_Click;
            DarkThemeButton.Click += Dark_Click;
            UkrainianButton.Click += UA_Click;
            EnglishButton.Click += EN_Click;
        }

        /// <summary>
        /// Застосовує світлу тему та зберігає вибір.
        /// </summary>
        private void Light_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme("Light");
            SettingsService.SaveTheme("Light");
        }

        /// <summary>
        /// Застосовує темну тему та зберігає вибір.
        /// </summary>
        private void Dark_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme("Dark");
            SettingsService.SaveTheme("Dark");
        }

        /// <summary>
        /// Завантажує відповідний словник ресурсів теми, видаляє стару тему та додає нову.
        /// </summary>
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
                CustomMessageBox.ShowSuccess(theme == "Light" ? "Світла тема активована!" : "Темна тема активована!");
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError($"Помилка зміни теми: {ex.Message}");
            }
        }

        /// <summary>
        /// Встановлює українську мову та зберігає вибір.
        /// </summary>
        private void UA_Click(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("UA");
            SettingsService.SaveLanguage("UA");
        }

        /// <summary>
        /// Встановлює англійську мову та зберігає вибір.
        /// </summary>
        private void EN_Click(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("EN");
            SettingsService.SaveLanguage("EN");
        }

        /// <summary>
        /// Завантажує відповідний словник ресурсів мови, видаляє старий та оновлює заголовок вікна.
        /// </summary>
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

                CustomMessageBox.ShowSuccess(lang == "EN" ? "Language changed!" : "Мову змінено!");
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError($"Помилка зміни мови: {ex.Message}");
            }
        }
    }
}