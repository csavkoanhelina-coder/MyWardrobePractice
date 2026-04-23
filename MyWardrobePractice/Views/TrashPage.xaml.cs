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
    /// Сторінка "Корзина", яка відображає всі видалені речі (IsDeleted = true).
    /// Дозволяє відновлювати речі назад до основного списку або остаточно видаляти їх з файлу.
    /// </summary>
    public partial class TrashPage : Page
    {
        private ObservableCollection<Clothing> allClothes;
        private string dataPath;

        /// <summary>
        /// Ініціалізує компоненти сторінки, встановлює шлях до файлу даних та завантажує список видалених речей.
        /// </summary>
        public TrashPage()
        {
            InitializeComponent();
            dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "clothes.json");
            LoadData();
        }

        /// <summary>
        /// Завантажує з файлу clothes.json усі речі, у яких IsDeleted == true, та відображає їх у списку.
        /// </summary>
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
                CustomMessageBox.ShowError($"Помилка збереження: {ex.Message}");
            }
        }

        /// <summary>
        /// Відновлює вибрану річ: змінює IsDeleted на false, зберігає зміни та оновлює список корзини.
        /// </summary>
        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Clothing item = button?.DataContext as Clothing;

            if (item != null)
            {
                item.IsDeleted = false;
                SaveData();
                LoadData();
                CustomMessageBox.ShowSuccess($"'{item.Name}' відновлено!");
            }
        }

        /// <summary>
        /// Остаточно видаляє вибрану річ з колекції та з файлу (без можливості відновлення).
        /// Перед видаленням запитує підтвердження у користувача.
        /// </summary>
        private void PermanentDelete_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Clothing item = button?.DataContext as Clothing;

            if (item != null)
            {
                if (CustomMessageBox.Show($"Видалити '{item.Name}' назавжди?", "Підтвердження") == true)
                {
                    allClothes.Remove(item);
                    SaveData();
                    LoadData();
                    CustomMessageBox.ShowInfo($"'{item.Name}' видалено назавжди!");
                }
            }
        }
    }
}