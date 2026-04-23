using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using MyWardrobe.Models;
using MyWardrobe.Views;
using Newtonsoft.Json;

namespace MyWardrobe
{
    /// <summary>
    /// Головне вікно програми, яке містить ліву панель навігації та фрейм для відображення сторінок.
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Clothing> clothes;

        /// <summary>
        /// Ініціалізує компоненти вікна, завантажує дані та відображає головну сторінку.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            MainFrame.Navigate(new HomePage());
        }

        /// <summary>
        /// Завантажує дані з файлу clothes.json у колекцію clothes.
        /// Якщо файл не існує або виникає помилка, створюється порожня колекція.
        /// </summary>
        private void LoadData()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "clothes.json");

                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    clothes = JsonConvert.DeserializeObject<ObservableCollection<Clothing>>(json);
                }
                else
                {
                    clothes = new ObservableCollection<Clothing>();
                }
            }
            catch
            {
                clothes = new ObservableCollection<Clothing>();
            }
        }

        /// <summary>Перехід на сторінку "Головна".</summary>
        private void Home_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new HomePage());

        /// <summary>Перехід на сторінку "Образи".</summary>
        private void Outfits_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new OutfitsPage());
        /// <summary>Перехід на сторінку "Категорії" з передачею колекції одягу.</summary>
        private void Categories_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new CategoriesPage(clothes));
       
        /// <summary>Перехід на сторінку "Одяг".</summary>
        private void Clothes_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new ClothesPage());
       
        /// <summary>Перехід на сторінку "Взуття".</summary>
        private void Shoes_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new ShoesPage());

        /// <summary>Перехід на сторінку "Улюблене" з передачею колекції одягу.</summary>
        private void Favorites_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new FavoritesPage(clothes));

        /// <summary>Перехід на сторінку "Корзина".</summary>
        private void Trash_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new TrashPage());

        /// <summary>Перехід на сторінку "Налаштування".</summary>
        private void Settings_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new SettingsPage());


        /// <summary>Обробник події навігації фрейму (залишено для можливого розширення).</summary>
        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}