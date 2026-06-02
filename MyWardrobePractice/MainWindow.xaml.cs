using System.Collections.ObjectModel;
using System.Windows;
using MyWardrobe.Models;
using MyWardrobe.Services;
using MyWardrobe.Views;

namespace MyWardrobe
{
    /// <summary>
    /// Головне вікно програми, яке містить панель навігації та фрейм для відображення сторінок.
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataService _dataService;

        /// <summary>
        /// Логін поточного користувача, який увійшов у систему.
        /// </summary>
        public string CurrentUserLogin { get; private set; }

        /// <summary>
        /// Ініціалізує новий екземпляр головного вікна для вказаного користувача.
        /// </summary>
        /// <param name="userLogin">Логін користувача, який увійшов у систему.</param>
        public MainWindow(string userLogin)
        {
            InitializeComponent();

            CurrentUserLogin = userLogin;
            _dataService = new DataService(userLogin);

            this.Title = $"Мій гардероб - {userLogin}";

            MainFrame.Navigate(new HomePage(_dataService));
        }

        /// <summary>
        /// Повертає сервіс роботи з даними для поточного користувача.
        /// </summary>
        /// <returns>Екземпляр сервісу DataService.</returns>
        public DataService GetDataService()
        {
            return _dataService;
        }

        /// <summary>Перехід на сторінку "Головна".</summary>
        private void Home_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new HomePage(_dataService));

        /// <summary>Перехід на сторінку "Образи".</summary>
        private void Outfits_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new OutfitsPage(_dataService));

        /// <summary>Перехід на сторінку "Категорії".</summary>
        private void Categories_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new CategoriesPage(_dataService));

        /// <summary>Перехід на сторінку "Одяг".</summary>
        private void Clothes_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new ClothesPage(_dataService));

        /// <summary>Перехід на сторінку "Взуття".</summary>
        private void Shoes_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new ShoesPage(_dataService));

        /// <summary>Перехід на сторінку "Улюблене".</summary>
        private void Favorites_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new FavoritesPage(_dataService));

        /// <summary>Перехід на сторінку "Корзина".</summary>
        private void Trash_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new TrashPage(_dataService));

        /// <summary>Перехід на сторінку "Налаштування".</summary>
        private void Settings_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new SettingsPage());

        /// <summary>Перехід на сторінку "Статистика".</summary>
        private void Statistics_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new StatisticsPage(_dataService));

        /// <summary>Обробник події навігації фрейму.</summary>
        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e) { }
    }
}