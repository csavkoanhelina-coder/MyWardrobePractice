using System.Windows;
using MyWardrobe.Services;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Вікно входу в систему.
    /// Перевіряє логін та пароль через AuthService,
    /// при успіху відкриває головне вікно з передачею логіну користувача.
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обробляє натискання кнопки "Увійти".
        /// Виконує вхід через AuthService.Login().
        /// </summary>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = LoginBox.Text.Trim();
                string password = PasswordBox.Password;

                var user = AuthService.Login(login, password);

                MainWindow mainWindow = new MainWindow(user.Login);
                mainWindow.Show();
                this.Close();
            }
            catch (System.Exception ex)
            {
                CustomMessageBox.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Перехід до вікна реєстрації.
        /// </summary>
        private void GoToRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }
    }
}