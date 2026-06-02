using MyWardrobe.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Вікно для перегляду створеного образу.
    /// Відображає назву образу, опис, дату створення та всі речі, що входять до нього.
    /// </summary>
    public partial class ViewOutfitWindow : Window
    {
        /// <summary>
        /// Ініціалізує новий екземпляр вікна перегляду образу.
        /// </summary>
        public ViewOutfitWindow(Outfit outfit, ObservableCollection<Clothing> allClothes)
        {
            InitializeComponent();

            TitleText.Text = outfit.Name;
            DescriptionText.Text = string.IsNullOrEmpty(outfit.Description) ? "Без опису" : outfit.Description;
            DateText.Text = $"Створено: {outfit.CreatedDate:dd.MM.yyyy HH:mm}";

            var itemsInOutfit = allClothes.Where(c => outfit.ItemIds.Contains(c.Id)).ToList();
            ItemsList.ItemsSource = itemsInOutfit;
        }

        /// <summary>
        /// Закриває вікно перегляду образу.
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}