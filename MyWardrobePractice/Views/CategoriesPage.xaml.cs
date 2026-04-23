using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MyWardrobe.Models;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Сторінка для відображення та фільтрації одягу за категоріями (верх, низ, взуття, плаття).
    /// Отримує колекцію одягу з головного вікна та фільтрує її відповідно до вибору користувача.
    /// </summary>
    public partial class CategoriesPage : Page
    {
        private ObservableCollection<Clothing> allClothes;

        /// <summary>
        /// Ініціалізує компоненти сторінки, зберігає посилання на колекцію одягу та показує всі речі.
        /// </summary>
        /// <param name="clothes">Колекція всіх речей (передається з MainWindow).</param>
        public CategoriesPage(ObservableCollection<Clothing> clothes)
        {
            InitializeComponent();
            allClothes = clothes; 
            ShowAll();
        }

        /// <summary>
        /// Обробляє натискання кнопок категорій та викликає відповідний метод фільтрації.
        /// </summary>
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

        /// <summary>
        /// Показує всі речі, які не видалені (IsDeleted == false).
        /// </summary>
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

        /// <summary>
        /// Фільтрує речі за вказаним типом (верх, низ, взуття, плаття) та показує тільки не видалені.
        /// </summary>
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