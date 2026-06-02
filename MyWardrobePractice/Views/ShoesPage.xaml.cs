using MyWardrobe.Models;
using MyWardrobe.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Сторінка "Взуття", яка відображає всі речі типу "взуття" в гардеробі.
    /// Дозволяє додавати, редагувати, видаляти взуття та відмічати його як улюблене.
    /// </summary>
    public partial class ShoesPage : Page
    {
        private ObservableCollection<Clothing> allClothes = new ObservableCollection<Clothing>();
        private DataService _dataService;

        /// <summary>
        /// Ініціалізує новий екземпляр сторінки взуття.
        /// </summary>
        public ShoesPage(DataService dataService)
        {
            InitializeComponent();
            _dataService = dataService;
            LoadData();
            ShoesList.ItemsSource = allClothes;
        }

        /// <summary>
        /// Завантажує всі невидалені речі типу "взуття" з файлу даних.
        /// </summary>
        public void LoadData()
        {
            if (_dataService != null)
            {
                var items = _dataService.LoadClothes();
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

        /// <summary>
        /// Зберігає поточний стан взуття у файл даних, оновлюючи статуси IsFavorite та IsDeleted.
        /// </summary>
        private void SaveData()
        {
            if (_dataService != null)
            {
                var allItems = _dataService.LoadClothes();
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
                _dataService.SaveClothes(allItems);
            }
        }

        /// <summary>
        /// Відкриває вікно редагування вибраного взуття та зберігає зміни.
        /// </summary>
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Clothing item = button?.DataContext as Clothing;

            if (item != null)
            {
                EditClothesWindow window = new EditClothesWindow(item);
                if (window.ShowDialog() == true && window.EditedItem != null)
                {
                    var allItems = _dataService.LoadClothes();
                    for (int i = 0; i < allItems.Count; i++)
                    {
                        if (allItems[i].Id == item.Id)
                        {
                            allItems[i] = window.EditedItem;
                            break;
                        }
                    }
                    _dataService.SaveClothes(allItems);

                    LoadData();
                    CustomMessageBox.ShowSuccess($"'{window.EditedItem.Name}' відредаговано!");
                }
            }
        }

        /// <summary>
        /// Змінює статус "улюблене" для вибраного взуття та зберігає зміни.
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
        /// Позначає вибране взуття як видалене (переміщує до корзини) після підтвердження користувача.
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