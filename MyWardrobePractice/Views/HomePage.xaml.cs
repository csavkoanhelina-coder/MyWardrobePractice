using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MyWardrobe.Models;
using MyWardrobe.Services;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Головна сторінка програми, яка відображає всі речі гардеробу та дозволяє виконувати пошук за назвою.
    /// </summary>
    public partial class HomePage : Page
    {
        private DataService _dataService;

        /// <summary>
        /// Колекція всіх невидалених речей для відображення на головній сторінці.
        /// </summary>
        public ObservableCollection<Clothing> clothes;

        /// <summary>
        /// Ініціалізує новий екземпляр головної сторінки.
        /// </summary>
        public HomePage(DataService dataService)
        {
            InitializeComponent();
            _dataService = dataService;
            LoadData();
            ItemsList.ItemsSource = clothes;
            UpdateCountText();
        }

        /// <summary>
        /// Завантажує з файлу даних усі невидалені речі (IsDeleted == false).
        /// У разі відсутності даних створює порожню колекцію.
        /// </summary>
        private void LoadData()
        {
            if (_dataService != null)
            {
                var allItems = _dataService.LoadClothes();
                clothes = new ObservableCollection<Clothing>();
                foreach (var item in allItems)
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
            }
        }

        /// <summary>
        /// Оновлює текстовий блок із загальною кількістю речей у гардеробі.
        /// </summary>
        private void UpdateCountText()
        {
            if (clothes != null)
            {
                CountText.Text = $"Всього речей: {clothes.Count}";
            }
        }

        /// <summary>
        /// Виконує пошук речей за назвою (без урахування регістру символів).
        /// Результати пошуку відображаються в окремому списку.
        /// </summary>
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
                CustomMessageBox.ShowInfo($"Нічого не знайдено за запитом \"{SearchBox.Text}\"");
            }
            else
            {
                CustomMessageBox.ShowSuccess($"Знайдено {result.Count} речей!");
            }
        }
    }
}