using System.Windows;
using System.Windows.Media;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Користувацьке діалогове вікно для відображення повідомлень, 
    /// запитань підтвердження, помилок та інших сповіщень.
    /// Підтримує різні типи кнопок (Так/Ні або OK) та іконки.
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        /// <summary>Тип кнопок, які відображаються у вікні.</summary>
        public enum ButtonType
        {
            YesNo,
            Ok 
        }

        /// <summary>Тип іконки та колір заголовка вікна.</summary>
        public enum MessageIcon
        {
            Question,
            Information,
            Warning,
            Error,
            Success,
            Trash
        }

        private bool _result = false;

        /// <summary>
        /// Конструктор вікна. Встановлює текст, заголовок, іконку та набір кнопок.
        /// </summary>
        public CustomMessageBox(string message, string title, ButtonType buttons, MessageIcon icon)
        {
            InitializeComponent();

            MessageText.Text = message;
            TitleText.Text = title;

            switch (icon)
            {
                case MessageIcon.Question:
                    IconText.Text = "❓";
                    IconText.Foreground = new SolidColorBrush(Color.FromRgb(255, 105, 180));
                    break;
                case MessageIcon.Information:
                    IconText.Text = "ℹ️";
                    IconText.Foreground = new SolidColorBrush(Color.FromRgb(66, 133, 244));
                    break;
                case MessageIcon.Warning:
                    IconText.Text = "⚠️";
                    IconText.Foreground = new SolidColorBrush(Color.FromRgb(255, 193, 7));
                    break;
                case MessageIcon.Error:
                    IconText.Text = "❌";
                    IconText.Foreground = new SolidColorBrush(Color.FromRgb(220, 20, 60));
                    break;
                case MessageIcon.Success:
                    IconText.Text = "✅";
                    IconText.Foreground = new SolidColorBrush(Color.FromRgb(76, 175, 80));
                    break;
                case MessageIcon.Trash:
                    IconText.Text = "🗑️";
                    IconText.Foreground = new SolidColorBrush(Color.FromRgb(220, 20, 60));
                    break;
            }

            if (buttons == ButtonType.YesNo)
            {
                YesButton.Visibility = Visibility.Visible;
                NoButton.Visibility = Visibility.Visible;
                OkButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                YesButton.Visibility = Visibility.Collapsed;
                NoButton.Visibility = Visibility.Collapsed;
                OkButton.Visibility = Visibility.Visible;
            }
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            _result = true;
            this.DialogResult = true;
            this.Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            _result = false;
            this.DialogResult = false;
            this.Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            _result = true;
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Показує діалогове вікно з питанням (кнопки Так/Ні) та повертає true/false.
        /// </summary>
        public static bool Show(string message, string title = "Підтвердження",
            ButtonType buttons = ButtonType.YesNo, MessageIcon icon = MessageIcon.Question)
        {
            var dialog = new CustomMessageBox(message, title, buttons, icon);
            dialog.Owner = Application.Current.MainWindow;
            var result = dialog.ShowDialog();
            return result == true;
        }

        /// <summary>Показує інформаційне вікно з кнопкою OK.</summary>
        public static void ShowInfo(string message, string title = "Інформація")
        {
            var dialog = new CustomMessageBox(message, title, ButtonType.Ok, MessageIcon.Information);
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }

        /// <summary>Показує вікно успіху з кнопкою OK.</summary>
        public static void ShowSuccess(string message, string title = "Успішно")
        {
            var dialog = new CustomMessageBox(message, title, ButtonType.Ok, MessageIcon.Success);
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }

        /// <summary>Показує вікно попередження з кнопкою OK.</summary>
        public static void ShowWarning(string message, string title = "Попередження")
        {
            var dialog = new CustomMessageBox(message, title, ButtonType.Ok, MessageIcon.Warning);
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }

        /// <summary>Показує вікно помилки з кнопкою OK.</summary>
        public static void ShowError(string message, string title = "Помилка")
        {
            var dialog = new CustomMessageBox(message, title, ButtonType.Ok, MessageIcon.Error);
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }

        /// <summary>Показує вікно видалення з кнопкою OK.</summary>
        public static void ShowTrash(string message, string title = "Видалення")
        {
            var dialog = new CustomMessageBox(message, title, ButtonType.Ok, MessageIcon.Trash);
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }
    }
}