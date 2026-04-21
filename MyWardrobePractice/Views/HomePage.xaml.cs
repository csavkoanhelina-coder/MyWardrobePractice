using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using MyWardrobe.Models;
using Newtonsoft.Json;

namespace MyWardrobe.Views
{
    public partial class HomePage : Page
    {
        public ObservableCollection<Clothing> clothes;

        public HomePage()
        {
            InitializeComponent();
            LoadData();
            ItemsList.ItemsSource = clothes;
            UpdateCountText();
        }

        private void LoadData()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "clothes.json");

                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    var items = JsonConvert.DeserializeObject<ObservableCollection<Clothing>>(json);

                    clothes = new ObservableCollection<Clothing>();

                    foreach (var item in items)
                    {
                        if (!item.IsDeleted)
                        {
                            clothes.Add(item);
                        }
                    }
                }
                else
                {
                    clothes = new ObservableCollection<Clothing>();
                    MessageBox.Show("Файл з даними не знайдено!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження: {ex.Message}");
                clothes = new ObservableCollection<Clothing>();
            }
        }

        private void UpdateCountText()
        {
            if (clothes != null)
            {
                CountText.Text = $"Всього речей: {clothes.Count}";
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                ResultList.ItemsSource = null;
                return;
            }

            ObservableCollection<Clothing> result = new ObservableCollection<Clothing>();

            foreach (var item in clothes)
            {
                if (item.Name.ToLower().Contains(SearchBox.Text.ToLower()))
                {
                    result.Add(item);
                }
            }

            ResultList.ItemsSource = result;

            if (result.Count == 0)
            {
                MessageBox.Show($"Нічого не знайдено за запитом \"{SearchBox.Text}\"", "Результат пошуку", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Знайдено {result.Count} речей!", "Результат пошуку", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}