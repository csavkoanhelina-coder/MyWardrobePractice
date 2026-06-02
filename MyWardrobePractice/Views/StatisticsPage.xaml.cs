using System.Collections.ObjectModel;
using System.Windows.Controls;
using MyWardrobe.Models;
using MyWardrobe.Services;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Сторінка "Статистика", яка відображає кількісні показники гардеробу користувача.
    /// Показує загальну кількість речей, розподіл за категоріями, сезонами та кількість улюблених речей.
    /// </summary>
    public partial class StatisticsPage : Page
    {
        private DataService _dataService;

        /// <summary>
        /// Ініціалізує новий екземпляр сторінки статистики.
        /// </summary>
        public StatisticsPage(DataService dataService)
        {
            InitializeComponent();
            _dataService = dataService;
            LoadStatistics();
        }

        /// <summary>
        /// Завантажує всі речі користувача та підраховує статистику:
        /// загальна кількість, кількість за категоріями, за сезонами та кількість улюблених речей.
        /// Видалені речі (IsDeleted = true) не враховуються в статистиці.
        /// </summary>
        private void LoadStatistics()
        {
            if (_dataService == null) return;

            var allItems = _dataService.LoadClothes();

            int total = 0;
            int top = 0, bottom = 0, dress = 0, shoes = 0;
            int favorites = 0;
            int spring = 0, summer = 0, autumn = 0, winter = 0;

            foreach (var item in allItems)
            {
                if (item.IsDeleted) continue;

                total++;

                switch (item.Type)
                {
                    case "верх": top++; break;
                    case "низ": bottom++; break;
                    case "плаття": dress++; break;
                    case "взуття": shoes++; break;
                }

                if (item.IsFavorite) favorites++;

                switch (item.Season)
                {
                    case "весна": spring++; break;
                    case "літо": summer++; break;
                    case "осінь": autumn++; break;
                    case "зима": winter++; break;
                }
            }

            TotalCount.Text = $"{total}";
            TopCount.Text = $"👕 {top}";
            BottomCount.Text = $"👇 {bottom}";
            DressCount.Text = $"👗 {dress}";
            ShoesCount.Text = $"👞 {shoes}";
            FavoritesCount.Text = $"❤️ {favorites}";
            SpringCount.Text = $"🌸 {spring}";
            SummerCount.Text = $"☀️ {summer}";
            AutumnCount.Text = $"🍂 {autumn}";
            WinterCount.Text = $"❄️ {winter}";
        }
    }
}