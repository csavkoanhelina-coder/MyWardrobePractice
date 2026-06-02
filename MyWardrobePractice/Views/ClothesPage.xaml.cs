using MyWardrobe.Models;
using MyWardrobe.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Сторінка "Одяг", яка відображає верхній одяг та плаття.
    /// Дозволяє додавати, редагувати, видаляти речі, відмічати улюблені та виконувати фільтрацію.
    /// </summary>
    public partial class ClothesPage : Page
    {
        private ObservableCollection<Clothing> clothes = new ObservableCollection<Clothing>();
        private DataService _dataService;

        /// <summary>
        /// Ініціалізує новий екземпляр сторінки одягу.
        /// </summary>
        public ClothesPage(DataService dataService)
        {
            InitializeComponent();
            _dataService = dataService;
            LoadData();
            ClothesList.ItemsSource = clothes;
        }

        /// <summary>
        /// Завантажує всі невидалені речі, які не є взуттям, з файлу даних.
        /// Після завантаження застосовує поточні фільтри.
        /// </summary>
        public void LoadData()
        {
            if (_dataService != null)
            {
                var allItems = _dataService.LoadClothes();
                clothes.Clear();
                foreach (var item in allItems)
                {
                    if (item.IsDeleted == false && item.Type != "взуття")
                    {
                        clothes.Add(item);
                    }
                }
                ClothesList.Items.Refresh();

                ApplyFilters();
            }
        }

        /// <summary>
        /// Зберігає поточний стан речей у файл даних, оновлюючи статуси IsFavorite та IsDeleted.
        /// </summary>
        private void SaveData()
        {
            if (_dataService != null)
            {
                var allItems = _dataService.LoadClothes();
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
                _dataService.SaveClothes(allItems);
            }
        }

        /// <summary>
        /// Відкриває вікно додавання нової речі та зберігає її при успішному завершенні.
        /// </summary>
        private void OpenAddWindow_Click(object sender, RoutedEventArgs e)
        {
            AddClothesWindow window = new AddClothesWindow();
            if (window.ShowDialog() == true && window.NewItem != null)
            {
                var allItems = _dataService.LoadClothes();
                allItems.Add(window.NewItem);
                _dataService.SaveClothes(allItems);

                LoadData();
                CustomMessageBox.ShowSuccess($"'{window.NewItem.Name}' додано!");
            }
        }

        /// <summary>
        /// Обробляє зміну значення у фільтрах (колір або сезон) та оновлює список речей.
        /// </summary>
        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        /// <summary>
        /// Застосовує вибрані фільтри (за кольором та сезоном) до списку речей.
        /// Відображає лише ті речі, які відповідають усім вибраним критеріям.
        /// </summary>
        private void ApplyFilters()
        {
            if (_dataService == null) return;

            var allItems = _dataService.LoadClothes();

            var filtered = allItems.Where(c => !c.IsDeleted && c.Type != "взуття").ToList();

            string selectedColor = (ColorFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedColor != "🎨 Всі кольори" && selectedColor != "Всі кольори" && selectedColor != null)
            {
                filtered = filtered.Where(c => c.Color.ToLower() == selectedColor.ToLower()).ToList();
            }

            string selectedSeason = (SeasonFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedSeason != "🌤️ Всі сезони" && selectedSeason != "Всі сезони" && selectedSeason != null)
            {
                string season = selectedSeason switch
                {
                    "🌸 Весна" => "весна",
                    "☀️ Літо" => "літо",
                    "🍂 Осінь" => "осінь",
                    "❄️ Зима" => "зима",
                    _ => ""
                };
                filtered = filtered.Where(c => c.Season == season).ToList();
            }

            clothes.Clear();
            foreach (var item in filtered)
            {
                clothes.Add(item);
            }
            ClothesList.Items.Refresh();
        }

        /// <summary>
        /// Відкриває вікно редагування вибраної речі та зберігає зміни.
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
        /// Змінює статус "улюблене" для вибраної речі та зберігає зміни.
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
        /// Позначає вибрану річ як видалену (переміщує до корзини) після підтвердження користувача.
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