using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MyWardrobe.Models;

namespace MyWardrobe.Views
{
    public partial class CategoriesPage : Page
    {
        private ObservableCollection<Clothing> allClothes;

        public CategoriesPage(ObservableCollection<Clothing> clothes)
        {
            InitializeComponent();
            allClothes = clothes; 
            ShowAll();
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string category = button.Content.ToString();

            if (category == "Всі")
            {
                ShowAll();
            }
            else if (category.Contains("Верх"))
            {
                ShowByType("верх");
            }
            else if (category.Contains("Низ"))
            {
                ShowByType("низ");
            }
            else if (category.Contains("Взуття"))
            {
                ShowByType("взуття");
            }
            else if (category.Contains("Плаття"))
            {
                ShowByType("плаття");
            }
        }

        private void ShowAll()
        {
            ObservableCollection<Clothing> result = new ObservableCollection<Clothing>();

            if (allClothes != null)
            {
                foreach (var item in allClothes)
                {
                    if (!item.IsDeleted)
                    {
                        result.Add(item);
                    }
                }
            }

            ItemsList.ItemsSource = result;
        }

        private void ShowByType(string type)
        {
            ObservableCollection<Clothing> result = new ObservableCollection<Clothing>();

            if (allClothes != null)
            {
                foreach (var item in allClothes)
                {
                    if (item.Type == type && !item.IsDeleted)
                    {
                        result.Add(item);
                    }
                }
            }

            ItemsList.ItemsSource = result;
        }
    }
}