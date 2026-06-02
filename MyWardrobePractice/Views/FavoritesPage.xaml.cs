using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MyWardrobe.Models;
using MyWardrobe.Services;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Сторінка "Улюблене", яка відображає всі речі, позначені користувачем як улюблені.
    /// Дозволяє видаляти речі зі списку улюблених.
    /// </summary>
    public partial class FavoritesPage : Page
    {
        private DataService _dataService;

        /// <summary>
        /// Ініціалізує новий екземпляр сторінки улюблених речей.
        /// </summary>
        public FavoritesPage(DataService dataService)
        {
            InitializeComponent();
            _dataService = dataService;
            LoadFavorites();
        }

        /// <summary>
        /// Завантажує з файлу даних усі речі, які мають позначку IsFavorite = true та не видалені.
        /// Оновлює список на сторінці.
        /// </summary>
        private void LoadFavorites()
        {
            if (_dataService != null)
            {
                var allItems = _dataService.LoadClothes();
                ObservableCollection<Clothing> favorites = new ObservableCollection<Clothing>();

                foreach (var item in allItems)
                {
                    if (item.IsFavorite == true && item.IsDeleted == false)
                    {
                        favorites.Add(item);
                    }
                }

                FavoritesList.ItemsSource = favorites;
            }
        }

        /// <summary>
        /// Зберігає поточний стан усіх речей у файл даних.
        /// </summary>
        private void SaveData()
        {
            if (_dataService != null)
            {
                var allItems = _dataService.LoadClothes();
                _dataService.SaveClothes(allItems);
            }
        }

        /// <summary>
        /// Видаляє вибрану річ зі списку улюблених (змінює IsFavorite на false).
        /// Після видалення оновлює відображення сторінки.
        /// </summary>
        private void RemoveFavorite_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Clothing item = button?.DataContext as Clothing;

            if (item != null)
            {
                item.IsFavorite = false;

                var allItems = _dataService.LoadClothes();
                foreach (var existing in allItems)
                {
                    if (existing.Id == item.Id)
                    {
                        existing.IsFavorite = false;
                        break;
                    }
                }
                _dataService.SaveClothes(allItems);

                LoadFavorites();
                CustomMessageBox.ShowInfo($"'{item.Name}' видалено з улюбленого!");
            }
        }
    }
}