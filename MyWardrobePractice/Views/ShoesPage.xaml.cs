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
    /// Сторінка для керування взуттям. 
    /// Відображає список взуття, дозволяє додавати в улюблене та видаляти в корзину.
    /// </summary>
    public partial class ShoesPage : Page
    {
        private ObservableCollection<Clothing> allClothes = new ObservableCollection<Clothing>();
        private string dataPath;

        /// <summary>
        /// Ініціалізує компоненти сторінки, встановлює шлях до файлу даних та завантажує список взуття.
        /// </summary>
        public ShoesPage()
        {
            InitializeComponent();
            dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "clothes.json");
            LoadData();
            ShoesList.ItemsSource = allClothes;
        }

        /// <summary>
        /// Завантажує з файлу clothes.json лише невидалене взуття (Type == "взуття" та IsDeleted == false).
        /// </summary>
        public void LoadData()
        {
            try
            {
                if (File.Exists(dataPath))
                {
                    string json = File.ReadAllText(dataPath);
                    var items = JsonConvert.DeserializeObject<ObservableCollection<Clothing>>(json);

                    allClothes.Clear();

                    foreach (var item in items)
                    {
                        if (item.Type == "взуття" && item.IsDeleted == false)
                        {
                            allClothes.Add(item);
                        }
                    }

                    ShoesList.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError($"Помилка завантаження: {ex.Message}");
            }
        }

        /// <summary>
        /// Зберігає поточний стан колекції взуття у файл clothes.json.
        /// Оновлює статуси IsFavorite та IsDeleted для існуючих записів, додає нове взуття.
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

                foreach (var currentItem in allClothes)
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

                ShoesList.Items.Refresh();

                if (item.IsFavorite)
                    CustomMessageBox.ShowSuccess($"'{item.Name}' додано в улюблене!");
                else
                    CustomMessageBox.ShowInfo($"'{item.Name}' видалено з улюбленого!");
            }
        }

        /// <summary>
        /// Обробляє натискання кнопки "🗑️": після підтвердження встановлює IsDeleted = true,
        /// зберігає зміни та оновлює список (взуття переміщується в корзину).
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