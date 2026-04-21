using System;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

namespace MyWardrobe.Services
{
    public class SettingsService
    {
        private static string SettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        public static void SaveTheme(string theme)
        {
            var settings = LoadSettings();
            settings.Theme = theme;
            SaveSettings(settings);
        }

        public static void SaveLanguage(string language)
        {
            var settings = LoadSettings();
            settings.Language = language;
            SaveSettings(settings);
        }

        public static string LoadTheme()
        {
            return LoadSettings().Theme;
        }

        public static string LoadLanguage()
        {
            return LoadSettings().Language;
        }

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

    public class AppSettings
    {
        public string Theme { get; set; } = "Light";
        public string Language { get; set; } = "UA";
    }
}