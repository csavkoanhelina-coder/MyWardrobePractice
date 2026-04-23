using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using MyWardrobe.Models;
using Newtonsoft.Json;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Сторінка для відображення речей, які користувач позначив як улюблені (IsFavorite = true).
    /// Дозволяє видаляти речі зі списку улюблених (скидає статус IsFavorite).
    /// </summary>
    public partial class FavoritesPage : Page
    {
        private ObservableCollection<Clothing> allClothes;
        private string dataPath;

        /// <summary>
        /// Ініціалізує компоненти сторінки, отримує колекцію одягу та завантажує список улюблених речей.
        /// </summary>
        /// <param name="clothes">Колекція всіх речей (передається з MainWindow).</param>
        public FavoritesPage(ObservableCollection<Clothing> clothes)
        {
            InitializeComponent();
            allClothes = clothes;
            dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "clothes.json");
            LoadFavorites();
        }

        /// <summary>
        /// Завантажує з файлу clothes.json речі, які мають IsFavorite = true та IsDeleted = false.
        /// Оновлює список на сторінці.
        /// </summary>
        private void LoadFavorites()
        {
            try
            {
                if (File.Exists(dataPath))
                {
                    string json = File.ReadAllText(dataPath);
                    allClothes = JsonConvert.DeserializeObject<ObservableCollection<Clothing>>(json);
                }

                ObservableCollection<Clothing> favorites = new ObservableCollection<Clothing>();

                foreach (var item in allClothes)
                {
                    if (item.IsFavorite == true && item.IsDeleted == false)
                    {
                        favorites.Add(item);
                    }
                }

                FavoritesList.ItemsSource = favorites;

                if (favorites.Count == 0)
                {
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError($"Помилка: {ex.Message}");
            }
        }

        /// <summary>
        /// Зберігає поточний стан колекції allClothes у файл clothes.json.
        /// </summary>
        private void SaveData()
        {
            try
            {
                string json = JsonConvert.SerializeObject(allClothes, Formatting.Indented);
                File.WriteAllText(dataPath, json);
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError($"Помилка: {ex.Message}");
            }
        }

        /// <summary>
        /// Обробляє натискання кнопки видалення з улюблених (❤️). 
        /// Змінює IsFavorite на false, зберігає зміни та оновлює список.
        /// </summary>
        private void RemoveFavorite_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Clothing item = button?.DataContext as Clothing;

            if (item != null)
            {
                item.IsFavorite = false;

                SaveData();

                LoadFavorites();

                CustomMessageBox.ShowInfo($"'{item.Name}' видалено з улюбленого!");
            }
        }
    }
}