using System.Windows;
using System.Windows.Controls;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Сторінка "Образи", яка відображає інтерфейс для створення комбінацій одягу.
    /// На поточному етапі реалізовано лише заглушку з повідомленням про майбутню функціональність.
    /// </summary>
    public partial class OutfitsPage : Page
    {
        /// <summary>
        /// Ініціалізує компоненти сторінки.
        /// </summary>
        public OutfitsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обробляє натискання кнопки створення нового образу.
        /// Показує інформаційне повідомлення, що функція знаходиться в розробці.
        /// </summary>
        private void CreateOutfit_Click(object sender, RoutedEventArgs e)
        {
            CustomMessageBox.ShowInfo("Функція створення образів буде доступна найближчим часом!", "В розробці");
        }
    }
}