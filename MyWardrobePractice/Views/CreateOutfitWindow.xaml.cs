using MyWardrobe.Models;
using MyWardrobe.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Вікно для створення нового образу (комбінації одягу).
    /// Дозволяє користувачеві вибирати речі з гардеробу та об'єднувати їх в образ.
    /// </summary>
    public partial class CreateOutfitWindow : Window
    {
        private DataService _dataService;
        private ObservableCollection<Clothing> _allClothes;
        private ObservableCollection<Clothing> _filteredClothes;
        private ObservableCollection<Clothing> _selectedItems;

        /// <summary>
        /// Отримує створений образ після успішного збереження.
        /// </summary>
        public Outfit NewOutfit { get; set; }

        /// <summary>
        /// Ініціалізує новий екземпляр вікна створення образу.
        /// </summary>
        public CreateOutfitWindow(DataService dataService)
        {
            InitializeComponent();
            _dataService = dataService;
            _selectedItems = new ObservableCollection<Clothing>();
            SelectedItemsList.ItemsSource = _selectedItems;

            LoadClothes();
        }

        /// <summary>
        /// Завантажує всі невидалені речі користувача з файлу даних.
        /// </summary>
        private void LoadClothes()
        {
            _allClothes = _dataService.LoadClothes();

            var activeItems = _allClothes.Where(c => !c.IsDeleted).ToList();
            _filteredClothes = new ObservableCollection<Clothing>(activeItems);
            AvailableItemsList.ItemsSource = _filteredClothes;
        }

        /// <summary>
        /// Фільтрує список доступних речей за вибраним типом одягу (верх, низ, плаття, взуття).
        /// </summary>
        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string filter = (FilterBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(filter) || filter == "Всі")
            {
                AvailableItemsList.ItemsSource = _filteredClothes;
                return;
            }

            string type = filter switch
            {
                "Верх" => "верх",
                "Низ" => "низ",
                "Плаття" => "плаття",
                "Взуття" => "взуття",
                _ => ""
            };

            var filtered = _filteredClothes.Where(c => c.Type == type).ToList();
            AvailableItemsList.ItemsSource = filtered;
        }

        /// <summary>
        /// Додає вибрану річ до списку речей, що входять в образ.
        /// </summary>
        private void Item_Click(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            Clothing item = border?.DataContext as Clothing;

            if (item != null && !_selectedItems.Contains(item))
            {
                _selectedItems.Add(item);
                SelectedItemsList.Items.Refresh();
            }
        }

        /// <summary>
        /// Видаляє вибрану річ зі списку речей образу.
        /// </summary>
        private void RemoveSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Clothing item = button?.DataContext as Clothing;

            if (item != null)
            {
                _selectedItems.Remove(item);
                SelectedItemsList.Items.Refresh();
            }
        }

        /// <summary>
        /// Створює новий образ, генерує унікальний ідентифікатор та зберігає його.
        /// Виконує перевірку наявності назви та хоча б однієї речі в образі.
        /// </summary>
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                CustomMessageBox.ShowWarning("Введіть назву образу!");
                return;
            }

            if (_selectedItems.Count == 0)
            {
                CustomMessageBox.ShowWarning("Додайте хоча б одну річ до образу!");
                return;
            }

            var existingOutfits = _dataService.LoadOutfits();
            int newId = 1;
            if (existingOutfits.Count > 0)
            {
                newId = existingOutfits.Max(o => o.Id) + 1;
            }

            System.Diagnostics.Debug.WriteLine("=== ВИБРАНІ РЕЧІ ===");
            foreach (var item in _selectedItems)
            {
                System.Diagnostics.Debug.WriteLine($"ID: {item.Id}, Назва: {item.Name}");
            }

            NewOutfit = new Outfit
            {
                Id = newId,
                Name = NameBox.Text.Trim(),
                Description = DescriptionBox.Text.Trim(),
                ItemIds = new ObservableCollection<int>(_selectedItems.Select(i => i.Id)),
                CreatedDate = DateTime.Now
            };

            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Скасовує створення образу та закриває вікно.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}