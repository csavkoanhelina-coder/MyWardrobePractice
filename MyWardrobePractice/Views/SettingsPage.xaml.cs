using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MyWardrobe.Services;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Сторінка налаштувань програми.
    /// Дозволяє користувачеві змінювати тему оформлення (світла/темна) та мову інтерфейсу (українська/англійська).
    /// Налаштування автоматично зберігаються між сеансами роботи за допомогою SettingsService.
    /// </summary>
    public partial class SettingsPage : Page
    {
        /// <summary>
        /// Ініціалізує компоненти сторінки.
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обробник натискання кнопки "Світла тема".
        /// Застосовує світлу тему та зберігає вибір.
        /// </summary>
        private void Light_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme("Light");
            SettingsService.SaveTheme("Light");
        }

        /// <summary>
        /// Обробник натискання кнопки "Темна тема".
        /// Застосовує темну тему та зберігає вибір.
        /// </summary>
        private void Dark_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme("Dark");
            SettingsService.SaveTheme("Dark");
        }

        /// <summary>
        /// Застосовує вказану тему оформлення.
        /// Завантажує відповідний ResourceDictionary, видаляє стару тему та додає нову.
        /// </summary>
        private void ApplyTheme(string theme)
        {
            try
            {
                var dict = new ResourceDictionary();
                dict.Source = new Uri($"/Resources/{theme}Theme.xaml", UriKind.Relative);

                List<ResourceDictionary> toRemove = new List<ResourceDictionary>();
                foreach (var d in Application.Current.Resources.MergedDictionaries)
                {
                    if (d.Source != null)
                    {
                        string source = d.Source.ToString();
                        if (source.Contains("LightTheme") || source.Contains("DarkTheme"))
                        {
                            toRemove.Add(d);
                        }
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
        /// Обробник натискання кнопки "Українська мова".
        /// Змінює мову інтерфейсу на українську та зберігає вибір.
        /// </summary>
        private void UA_Click(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("UA");
            SettingsService.SaveLanguage("UA");
        }

        /// <summary>
        /// Обробник натискання кнопки "Англійська мова".
        /// Змінює мову інтерфейсу на англійську та зберігає вибір.
        /// </summary>
        private void EN_Click(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("EN");
            SettingsService.SaveLanguage("EN");
        }

        /// <summary>
        /// Змінює мову інтерфейсу програми.
        /// Завантажує відповідний ResourceDictionary з рядками перекладу,
        /// видаляє старий мовний ресурс та додає новий.
        /// Також оновлює заголовок головного вікна.
        /// </summary>
        private void ChangeLanguage(string lang)
        {
            try
            {
                var dict = new ResourceDictionary();
                dict.Source = new Uri($"/Resources/StringResources.{lang.ToLower()}.xaml", UriKind.Relative);

                List<ResourceDictionary> toRemove = new List<ResourceDictionary>();
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