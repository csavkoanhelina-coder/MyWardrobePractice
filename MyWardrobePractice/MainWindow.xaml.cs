using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using MyWardrobe.Models;
using MyWardrobe.Views;
using Newtonsoft.Json;

namespace MyWardrobe
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Clothing> clothes;

        public MainWindow()
        {
            InitializeComponent();
            LoadData();  
            MainFrame.Navigate(new HomePage());
        }

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

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HomePage());
        }

        private void Outfits_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OutfitsPage());
        }

        private void Categories_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CategoriesPage(clothes));
        }

        private void Clothes_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ClothesPage());
        }

        private void Shoes_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ShoesPage());
        }

        private void Favorites_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new FavoritesPage(clothes));
        }

        private void Trash_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TrashPage());
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SettingsPage());
        }

        public void RefreshClothesPage()
        {
            if (MainFrame.Content is ClothesPage clothesPage)
            {
                clothesPage.LoadData();
            }
        }
    }
}