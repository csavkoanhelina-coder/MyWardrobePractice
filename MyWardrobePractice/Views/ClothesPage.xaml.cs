using MyWardrobe.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MyWardrobe.Views
{
    public partial class ClothesPage : Page
    {
        private ObservableCollection<Clothing> clothes = new ObservableCollection<Clothing>();
        private string dataPath;

        public ClothesPage()
        {
            InitializeComponent();
            dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "clothes.json");
            LoadData();
            ClothesList.ItemsSource = clothes;
        }

        public void LoadData()
        {
            try
            {
                if (File.Exists(dataPath))
                {
                    string json = File.ReadAllText(dataPath);
                    var allItems = JsonConvert.DeserializeObject<ObservableCollection<Clothing>>(json);

                    clothes.Clear();

                    foreach (var item in allItems)
                    {
                        if (item.IsDeleted == false && item.Type != "взуття")
                        {
                            clothes.Add(item);
                        }
                    }

                    ClothesList.Items.Refresh();
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

                foreach (var currentItem in clothes)
                {
                    bool found = false;
                    for (int i = 0; i < allItems.Count; i++)
                    {
                        if (allItems[i].Name == currentItem.Name && allItems[i].ImagePath == currentItem.ImagePath)
                        {
                            allItems[i].IsFavorite = currentItem.IsFavorite;
                            allItems[i].IsDeleted = currentItem.IsDeleted;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        allItems.Add(currentItem);
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

        private void OpenAddWindow_Click(object sender, RoutedEventArgs e)
        {
            AddClothesWindow window = new AddClothesWindow();

            if (window.ShowDialog() == true && window.NewItem != null)
            {
                clothes.Add(window.NewItem);

                SaveData();

                LoadData();

                MessageBox.Show($"✅ '{window.NewItem.Name}' додано успішно!");
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
                    SaveData();
                    LoadData();
                    MessageBox.Show($"🗑️ '{item.Name}' переміщено в корзину!");
                }
            }
        }
    }
}