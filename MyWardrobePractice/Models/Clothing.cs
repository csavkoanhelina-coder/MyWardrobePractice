using System.ComponentModel;

namespace MyWardrobe.Models
{
    /// <summary>
    /// Модель даних, що представляє предмет одягу або взуття.
    /// Реалізує INotifyPropertyChanged для автоматичного оновлення інтерфейсу при зміні властивостей.
    /// </summary>
    public class Clothing : INotifyPropertyChanged
    {
        private string _name;
        private string _type;
        private string _season;
        private string _size;
        private string _color;
        private string _imagePath;
        private bool _isFavorite;
        private bool _isDeleted;
        private int _id;

        /// <summary>Назва предмета (наприклад, "спідниця", "кросівки").</summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>Тип одягу: "верх", "низ", "плаття", "взуття".</summary>
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        /// <summary>Сезон: "весна", "літо", "осінь", "зима".</summary>
        public string Season
        {
            get => _season;
            set
            {
                _season = value;
                OnPropertyChanged(nameof(Season));
            }
        }

        /// <summary>Розмір (наприклад, "S", "M", "L", "38").</summary>
        public string Size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged(nameof(Size));
            }
        }

        /// <summary>Колір предмета.</summary>
        public string Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        /// <summary>Відносний шлях до фотографії предмета.</summary>
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }

        /// <summary>Позначка "улюблене" (true – в улюблених, false – ні).</summary>
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                _isFavorite = value;
                OnPropertyChanged(nameof(IsFavorite));
            }
        }

        /// <summary>Позначка "видалено" (true – в корзині, false – активний предмет).</summary>
        public bool IsDeleted
        {
            get => _isDeleted;
            set
            {
                _isDeleted = value;
                OnPropertyChanged(nameof(IsDeleted));
            }
        }

        /// <summary>Унікальний ідентифікатор речі (для образів).</summary>
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
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