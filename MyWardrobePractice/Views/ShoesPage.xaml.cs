using MyWardrobe.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MyWardrobe.Views
{
    public partial class ShoesPage : Page
    {
        private ObservableCollection<Clothing> shoes = new ObservableCollection<Clothing>();
        private string dataPath;

        public ShoesPage()
        {
            InitializeComponent();
            dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "clothes.json");
            LoadData();
            ShoesList.ItemsSource = shoes;
        }

        public void LoadData()
        {
            try
            {
                if (File.Exists(dataPath))
                {
                    string json = File.ReadAllText(dataPath);
                    var allItems = JsonConvert.DeserializeObject<ObservableCollection<Clothing>>(json);

                    shoes.Clear();

                    foreach (var item in allItems)
                    {
                        if (item.Type == "взуття" && item.IsDeleted == false)
                        {
                            shoes.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження: {ex.Message}");
            }
        }

        private void SaveData()
        {
            try
            {
                ObservableCollection<Clothing> allItems = new ObservableCollection<Clothing>();

                if (File.Exists(dataPath))
                {
                    string json = File.ReadAllText(dataPath);
                    allItems = JsonConvert.DeserializeObject<ObservableCollection<Clothing>>(json);
                }

                foreach (var shoe in shoes)
                {
                    for (int i = 0; i < allItems.Count; i++)
                    {
                        if (allItems[i].Name == shoe.Name && allItems[i].Type == "взуття")
                        {
                            allItems[i].IsFavorite = shoe.IsFavorite;
                            allItems[i].IsDeleted = shoe.IsDeleted;
                            break;
                        }
                    }
                }

                string newJson = JsonConvert.SerializeObject(allItems, Formatting.Indented);
                File.WriteAllText(dataPath, newJson);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}");
            }
        }

        private void Favorite_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Clothing item = button?.DataContext as Clothing;

            if (item != null)
            {
                item.IsFavorite = !item.IsFavorite;
                SaveData();

                if (item.IsFavorite)
                    MessageBox.Show($"❤️ '{item.Name}' додано в улюблене!");
                else
                    MessageBox.Show($"💔 '{item.Name}' видалено з улюбленого!");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Clothing item = button?.DataContext as Clothing;

            if (item != null)
            {
                MessageBoxResult result = MessageBox.Show($"Видалити '{item.Name}'?", "Підтвердження", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    item.IsDeleted = true;

                    shoes.Remove(item);

                    SaveData();

                    ShoesList.Items.Refresh();

                    MessageBox.Show($"🗑️ '{item.Name}' переміщено в корзину!");
                }
            }
        }
    }
}