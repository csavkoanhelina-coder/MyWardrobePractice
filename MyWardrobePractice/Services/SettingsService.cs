using System;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

namespace MyWardrobe.Services
{
    /// <summary>
    /// Сервіс для збереження та завантаження налаштувань програми (тема, мова).
    /// Дані зберігаються у файлі settings.json у папці з виконуваним файлом.
    /// </summary>
    public class SettingsService
    {
        private static string SettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        /// <summary>Зберігає вибрану тему оформлення.</summary>
        /// <param name="theme">Назва теми ("Light" або "Dark").</param>
        public static void SaveTheme(string theme)
        {
            var settings = LoadSettings();
            settings.Theme = theme;
            SaveSettings(settings);
        }

        /// <summary>Зберігає вибрану мову інтерфейсу.</summary>
        public static void SaveLanguage(string language)
        {
            var settings = LoadSettings();
            settings.Language = language;
            SaveSettings(settings);
        }

        /// <summary>Завантажує збережену тему (за замовчуванням "Light").</summary>
        public static string LoadTheme()
        {
            return LoadSettings().Theme;
        }

        /// <summary>Завантажує збережену мову (за замовчуванням "UA").</summary>
        public static string LoadLanguage()
        {
            return LoadSettings().Language;
        }

        /// <summary>Завантажує об'єкт налаштувань із файлу settings.json.</summary>
        private static AppSettings LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    return JsonConvert.DeserializeObject<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch { }
            return new AppSettings();
        }

        /// <summary>Зберігає об'єкт налаштувань у файл settings.json.</summary>
        private static void SaveSettings(AppSettings settings)
        {
            try
            {
                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(SettingsPath, json);
            }
            catch { }
        }
    }

    /// <summary>
    /// Клас, що представляє структуру налаштувань програми.
    /// </summary>
    public class AppSettings
    {
        /// <summary>Тема оформлення ("Light" або "Dark").</summary>
        public string Theme { get; set; } = "Light";

        /// <summary>Мова інтерфейсу ("UA" або "EN").</summary>
        public string Language { get; set; } = "UA";
    }
}