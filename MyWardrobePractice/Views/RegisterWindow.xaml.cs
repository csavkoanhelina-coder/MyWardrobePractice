using MyWardrobe.Services;
using MyWardrobe.Views;
using System.Windows;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Вікно реєстрації нового користувача.
    /// Виконує валідацію полів, перевіряє збіг паролів,
    /// викликає AuthService.Register() для створення акаунта.
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обробляє натискання кнопки "Зареєструватися".
        /// Перевіряє введені дані та створює нового користувача.
        /// </summary>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = LoginBox.Text.Trim();
                string password = PasswordBox.Password;
                string confirmPassword = ConfirmPasswordBox.Password;

                if (string.IsNullOrWhiteSpace(login))
                {
                    CustomMessageBox.ShowWarning("Будь ласка, введіть логін!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    CustomMessageBox.ShowWarning("Будь ласка, введіть пароль!");
                    return;
                }

                if (password.Length < 4)
                {
                    CustomMessageBox.ShowWarning("Пароль повинен містити не менше 4 символів!");
                    return;
                }

                if (password != confirmPassword)
                {
                    CustomMessageBox.ShowWarning("Паролі не співпадають! Перевірте ще раз.");
                    return;
                }

                AuthService.Register(login, password);

                CustomMessageBox.ShowSuccess("Реєстрація успішна! Тепер ви можете увійти в систему.");

                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            catch (System.Exception ex)
            {
                CustomMessageBox.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Перехід до вікна входу.
        /// </summary>
        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}