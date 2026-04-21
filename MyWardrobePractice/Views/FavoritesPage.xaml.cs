using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using MyWardrobe.Models;
using Newtonsoft.Json;

namespace MyWardrobe.Views
{
    public partial class FavoritesPage : Page
    {
        private ObservableCollection<Clothing> allClothes;
        private string dataPath;

        public FavoritesPage(ObservableCollection<Clothing> clothes)
        {
            InitializeComponent();
            allClothes = clothes;
            dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "clothes.json");
            LoadFavorites();
        }

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
                MessageBox.Show($"Помилка завантаження улюблених: {ex.Message}");
            }
        }

        private void SaveData()
        {
            try
            {
                string json = JsonConvert.SerializeObject(allClothes, Formatting.Indented);
                File.WriteAllText(dataPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}");
            }
        }

        private void RemoveFavorite_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Clothing item = button?.DataContext as Clothing;

            if (item != null)
            {
                item.IsFavorite = false;
                SaveData();
                LoadFavorites();
                MessageBox.Show($"💔 '{item.Name}' видалено з улюбленого!");
            }
        }
    }
}