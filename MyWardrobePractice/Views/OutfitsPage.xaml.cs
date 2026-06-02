using MyWardrobe.Models;
using MyWardrobe.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Сторінка "Образи", яка відображає всі створені користувачем образи (комбінації одягу).
    /// Дозволяє створювати нові образи, переглядати існуючі та видаляти їх.
    /// </summary>
    public partial class OutfitsPage : Page
    {
        private DataService _dataService;
        private ObservableCollection<Outfit> _outfits;
        private ObservableCollection<Clothing> _clothes;

        /// <summary>
        /// Ініціалізує новий екземпляр сторінки образів.
        /// </summary>
        public OutfitsPage(DataService dataService)
        {
            InitializeComponent();
            _dataService = dataService;

            if (_dataService != null)
            {
                _clothes = _dataService.LoadClothes();
                LoadOutfits();
            }
        }

        /// <summary>
        /// Завантажує всі образи користувача з файлу outfits.json.
        /// </summary>
        private void LoadOutfits()
        {
            if (_dataService != null)
            {
                _outfits = _dataService.LoadOutfits();
                OutfitsList.ItemsSource = _outfits;
            }
        }

        /// <summary>
        /// Зберігає всі образи користувача у файл outfits.json.
        /// </summary>
        private void SaveOutfits()
        {
            if (_dataService != null)
            {
                _dataService.SaveOutfits(_outfits);
            }
        }

        /// <summary>
        /// Відкриває вікно створення нового образу. При успішному створенні додає образ до колекції та зберігає дані.
        /// </summary>
        private void CreateOutfit_Click(object sender, RoutedEventArgs e)
        {
            if (_dataService == null)
            {
                CustomMessageBox.ShowError("Помилка: сервіс даних не знайдено!");
                return;
            }

            CreateOutfitWindow window = new CreateOutfitWindow(_dataService);
            if (window.ShowDialog() == true && window.NewOutfit != null)
            {
                _outfits.Add(window.NewOutfit);
                SaveOutfits();
                LoadOutfits();
                CustomMessageBox.ShowSuccess($"Образ '{window.NewOutfit.Name}' створено!");
            }
        }

        /// <summary>
        /// Відкриває вікно перегляду вибраного образу з усіма речами, що до нього входять.
        /// </summary>
        private void ViewOutfit_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Outfit outfit = button?.DataContext as Outfit;

            if (outfit != null && _clothes != null)
            {
                ViewOutfitWindow window = new ViewOutfitWindow(outfit, _clothes);
                window.ShowDialog();
            }
        }

        /// <summary>
        /// Видаляє вибраний образ після підтвердження користувача.
        /// </summary>
        private void DeleteOutfit_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Outfit outfit = button?.DataContext as Outfit;

            if (outfit != null)
            {
                if (CustomMessageBox.Show($"Видалити образ '{outfit.Name}'?", "Підтвердження") == true)
                {
                    _outfits.Remove(outfit);
                    SaveOutfits();
                    LoadOutfits();
                    CustomMessageBox.ShowInfo($"Образ '{outfit.Name}' видалено!");
                }
            }
        }
    }
}