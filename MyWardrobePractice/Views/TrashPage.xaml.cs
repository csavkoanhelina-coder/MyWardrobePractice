using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MyWardrobe.Models;
using MyWardrobe.Services;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Сторінка "Корзина", яка відображає всі видалені речі (IsDeleted = true).
    /// Дозволяє відновлювати речі назад до основного списку або остаточно видаляти їх з файлу.
    /// </summary>
    public partial class TrashPage : Page
    {
        private DataService _dataService;
        private ObservableCollection<Clothing> _allClothes;

        /// <summary>
        /// Ініціалізує новий екземпляр сторінки корзини.
        /// </summary>
        public TrashPage(DataService dataService)
        {
            InitializeComponent();
            _dataService = dataService;
            LoadData();
        }

        /// <summary>
        /// Завантажує з файлу даних усі речі, які мають позначку IsDeleted = true.
        /// Відображає їх у списку корзини.
        /// </summary>
        private void LoadData()
        {
            if (_dataService != null)
            {
                _allClothes = _dataService.LoadClothes();
                ObservableCollection<Clothing> deleted = new ObservableCollection<Clothing>();

                foreach (var item in _allClothes)
                {
                    if (item.IsDeleted == true)
                    {
                        deleted.Add(item);
                    }
                }

                TrashList.ItemsSource = deleted;
            }
        }

        /// <summary>
        /// Зберігає поточний стан усіх речей у файл даних.
        /// </summary>
        private void SaveData()
        {
            if (_dataService != null && _allClothes != null)
            {
                _dataService.SaveClothes(_allClothes);
            }
        }

        /// <summary>
        /// Відновлює вибрану річ з корзини (змінює IsDeleted на false).
        /// Після відновлення оновлює список корзини.
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
                    _allClothes.Remove(item);
                    SaveData();
                    LoadData();
                    CustomMessageBox.ShowInfo($"'{item.Name}' видалено назавжди!");
                }
            }
        }
    }
}