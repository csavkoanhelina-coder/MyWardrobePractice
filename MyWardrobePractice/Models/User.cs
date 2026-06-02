using System.ComponentModel;

namespace MyWardrobe.Models
{
    /// <summary>
    /// Модель даних, що представляє користувача системи.
    /// Реалізує INotifyPropertyChanged для автоматичного оновлення інтерфейсу при зміні властивостей.
    /// </summary>
    public class User : INotifyPropertyChanged
    {
        private string _login;
        private string _passwordHash;

        /// <summary>Логін користувача (унікальний ідентифікатор).</summary>
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        /// <summary>Хеш пароля, отриманий за допомогою алгоритму SHA256.
        /// Відкритий пароль ніколи не зберігається в системі для забезпечення безпеки.</summary>
        public string PasswordHash
        {
            get => _passwordHash;
            set
            {
                _passwordHash = value;
                OnPropertyChanged(nameof(PasswordHash));
            }
        }

        /// <summary>Подія, що виникає при зміні будь-якої властивості.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Викликає подію PropertyChanged для вказаної властивості.</summary>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}