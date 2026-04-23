using MyWardrobe.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Сторінка для керування одягом (верх, низ, плаття). 
    /// Дозволяє додавати нові речі, відмічати улюблені, видаляти в корзину.
    /// </summary>
    public partial class ClothesPage : Page
    {
        private ObservableCollection<Clothing> clothes = new ObservableCollection<Clothing>();
        private string dataPath;

        /// <summary>
        /// Ініціалізує компоненти, встановлює шлях до файлу даних та завантажує список одягу.
        /// </summary>
        public ClothesPage()
        {
            InitializeComponent();
            dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "clothes.json");
            LoadData();
            ClothesList.ItemsSource = clothes;
        }

        /// <summary>
        /// Завантажує з файлу clothes.json лише невидалені речі, які не є взуттям.
        /// </summary>
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
                CustomMessageBox.ShowError($"Помилка завантаження: {ex.Message}");
            }
        }

        /// <summary>
        /// Зберігає поточний стан колекції одягу у файл clothes.json.
        /// Оновлює статуси IsFavorite та IsDeleted для існуючих записів, додає нові.
        /// </summary>
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
                CustomMessageBox.ShowError($"Помилка збереження: {ex.Message}");
            }
        }

        /// <summary>
        /// Відкриває вікно додавання нового одягу. При успішному додаванні оновлює список.
        /// </summary>
        private void OpenAddWindow_Click(object sender, RoutedEventArgs e)
        {
            AddClothesWindow window = new AddClothesWindow();

            if (window.ShowDialog() == true && window.NewItem != null)
            {
                clothes.Add(window.NewItem);
                SaveData();
                LoadData();
                CustomMessageBox.ShowSuccess($"'{window.NewItem.Name}' додано!");
            }
        }

        /// <summary>
        /// Обробляє натискання кнопки "❤️": змінює статус IsFavorite, зберігає дані та оновлює список.
        /// </summary>
        private void Favorite_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Clothing item = button?.DataContext as Clothing;

            if (item != null)
            {
                item.IsFavorite = !item.IsFavorite;

                SaveData();

                ClothesList.Items.Refresh();

                if (item.IsFavorite)
                    CustomMessageBox.ShowSuccess($"'{item.Name}' додано в улюблене!");
                else
                    CustomMessageBox.ShowInfo($"'{item.Name}' видалено з улюбленого!");
            }
        }

        /// <summary>
        /// Обробляє натискання кнопки "🗑️": після підтвердження встановлює IsDeleted = true,
        /// зберігає зміни та оновлює список (річ переміщується в корзину).
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Clothing item = button?.DataContext as Clothing;

            if (item != null)
            {
                if (CustomMessageBox.Show($"Видалити '{item.Name}'?", "Підтвердження") == true)
                {
                    item.IsDeleted = true;

                    SaveData();

                    LoadData();

                    CustomMessageBox.ShowTrash($"'{item.Name}' переміщено в корзину!");
                }
            }
        }
    }
}