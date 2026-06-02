using MyWardrobe.Models;
using MyWardrobe.Views;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace MyWardrobe.Services
{
    /// <summary>
    /// Сервіс для роботи з даними конкретного користувача.
    /// Забезпечує ізольоване збереження даних кожного користувача в окремій папці.
    /// Відповідає за завантаження та збереження одягу та образів у файли формату JSON.
    /// </summary>
    public class DataService
    {
        private readonly string _userLogin;
        private readonly string _userDataFolder;

        /// <summary>
        /// Ініціалізує новий екземпляр сервісу даних для вказаного користувача.
        /// </summary>
        public DataService(string userLogin)
        {
            _userLogin = userLogin;
            _userDataFolder = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "Users",
                userLogin
            );

            if (!Directory.Exists(_userDataFolder))
            {
                Directory.CreateDirectory(_userDataFolder);
            }
        }

        /// <summary>
        /// Повертає повний шлях до файлу в папці користувача.
        /// </summary>
        private string GetFilePath(string fileName) => Path.Combine(_userDataFolder, fileName);

        /// <summary>
        /// Завантажує всі речі одягу користувача з файлу clothes.json.
        /// </summary>
        public ObservableCollection<Clothing> LoadClothes()
        {
            string path = GetFilePath("clothes.json");
            try
            {
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    return JsonConvert.DeserializeObject<ObservableCollection<Clothing>>(json)
                           ?? new ObservableCollection<Clothing>();
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError($"Помилка завантаження одягу: {ex.Message}");
            }
            return new ObservableCollection<Clothing>();
        }

        /// <summary>
        /// Зберігає всі речі одягу користувача у файл clothes.json.
        /// </summary>
        public void SaveClothes(ObservableCollection<Clothing> clothes)
        {
            try
            {
                string path = GetFilePath("clothes.json");
                string json = JsonConvert.SerializeObject(clothes, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError($"Помилка збереження одягу: {ex.Message}");
            }
        }

        /// <summary>
        /// Завантажує всі образи користувача з файлу outfits.json.
        /// </summary>
        public ObservableCollection<Outfit> LoadOutfits()
        {
            string path = GetFilePath("outfits.json");
            try
            {
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    return JsonConvert.DeserializeObject<ObservableCollection<Outfit>>(json)
                           ?? new ObservableCollection<Outfit>();
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError($"Помилка завантаження образів: {ex.Message}");
            }
            return new ObservableCollection<Outfit>();
        }

        /// <summary>
        /// Зберігає всі образи користувача у файл outfits.json.
        /// </summary>
        public void SaveOutfits(ObservableCollection<Outfit> outfits)
        {
            try
            {
                string path = GetFilePath("outfits.json");
                string json = JsonConvert.SerializeObject(outfits, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError($"Помилка збереження образів: {ex.Message}");
            }
        }
    }
}