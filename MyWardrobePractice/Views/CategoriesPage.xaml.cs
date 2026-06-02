using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MyWardrobe.Models;
using MyWardrobe.Services;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Сторінка "Категорії", яка відображає одяг, згрупований за типами (верх, низ, взуття, плаття).
    /// Дозволяє користувачеві фільтрувати речі за вибраною категорією.
    /// </summary>
    public partial class CategoriesPage : Page
    {
        private DataService _dataService;

        /// <summary>
        /// Ініціалізує новий екземпляр сторінки категорій.
        /// </summary>
        public CategoriesPage(DataService dataService)
        {
            InitializeComponent();
            _dataService = dataService;
            ShowAll();
        }

        /// <summary>
        /// Обробляє натискання кнопки категорії. Визначає вибрану категорію та викликає відповідний метод фільтрації.
        /// </summary>
        private void Category_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string category = button.Content.ToString();

            if (category.Contains("Всі") || category.Contains("All"))
            {
                ShowAll();
            }
            else if (category.Contains("Верх") || category.Contains("Top"))
            {
                ShowByType("верх");
            }
            else if (category.Contains("Низ") || category.Contains("Bottom"))
            {
                ShowByType("низ");
            }
            else if (category.Contains("Взуття") || category.Contains("Shoes"))
            {
                ShowByType("взуття");
            }
            else if (category.Contains("Плаття") || category.Contains("Dress"))
            {
                ShowByType("плаття");
            }
        }

        /// <summary>
        /// Відображає всі невидалені речі з гардеробу незалежно від їх типу.
        /// </summary>
        private void ShowAll()
        {
            if (_dataService != null)
            {
                var freshData = _dataService.LoadClothes();
                ObservableCollection<Clothing> result = new ObservableCollection<Clothing>();

                foreach (var item in freshData)
                {
                    if (!item.IsDeleted)
                    {
                        result.Add(item);
                    }
                }

                ItemsList.ItemsSource = result;
            }
        }

        /// <summary>
        /// Відображає лише речі вказаного типу, які не видалені.
        /// </summary>
        private void ShowByType(string type)
        {
            if (_dataService != null)
            {
                var freshData = _dataService.LoadClothes();
                ObservableCollection<Clothing> result = new ObservableCollection<Clothing>();

                foreach (var item in freshData)
                {
                    if (item.Type == type && !item.IsDeleted)
                    {
                        result.Add(item);
                    }
                }

                ItemsList.ItemsSource = result;
            }
        }
    }
}