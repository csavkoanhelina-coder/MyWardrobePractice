using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using MyWardrobe.Models;
using Newtonsoft.Json;

namespace MyWardrobe.Views
{
    public partial class TrashPage : Page
    {
        private ObservableCollection<Clothing> allClothes;
        private string dataPath;

        public TrashPage()
        {
            InitializeComponent();
            dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "clothes.json");
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (File.Exists(dataPath))
                {
                    string json = File.ReadAllText(dataPath);
                    allClothes = JsonConvert.DeserializeObject<ObservableCollection<Clothing>>(json);

                    ObservableCollection<Clothing> deleted = new ObservableCollection<Clothing>();

                    foreach (var item in allClothes)
                    {
                        if (item.IsDeleted == true)
                        {
                            deleted.Add(item);
                        }
                    }

                    TrashList.ItemsSource = deleted;

                    MessageBox.Show($"Корзина: знайдено {deleted.Count} видалених речей");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
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
                MessageBox.Show($"Помилка: {ex.Message}");
            }
        }

        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Clothing item = button?.DataContext as Clothing;

            if (item != null)
            {
                item.IsDeleted = false;
                SaveData();
                LoadData();
                MessageBox.Show($"✨ '{item.Name}' відновлено!");

                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.RefreshClothesPage();
                }
            }
        }

        private void PermanentDelete_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Clothing item = button?.DataContext as Clothing;

            if (item != null)
            {
                MessageBoxResult result = MessageBox.Show($"Видалити '{item.Name}' назавжди?", "Підтвердження", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    allClothes.Remove(item);
                    SaveData();
                    LoadData();
                    MessageBox.Show($"💀 '{item.Name}' видалено назавжди!");
                }
            }
        }
    }
}