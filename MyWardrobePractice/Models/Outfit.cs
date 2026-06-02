using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MyWardrobe.Models
{
    /// <summary>
    /// Модель даних, що представляє образ (комбінацію одягу).
    /// Реалізує INotifyPropertyChanged для автоматичного оновлення інтерфейсу при зміні властивостей.
    /// </summary>
    public class Outfit : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private ObservableCollection<int> _itemIds;
        private string _description;
        private DateTime _createdDate;

        /// <summary>Унікальний ідентифікатор образу.</summary>
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        /// <summary>Назва образу (наприклад, "Літній прогулянковий").</summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>Список ID речей, що входять в образ.</summary>
        public ObservableCollection<int> ItemIds
        {
            get => _itemIds ?? (_itemIds = new ObservableCollection<int>());
            set
            {
                _itemIds = value;
                OnPropertyChanged(nameof(ItemIds));
            }
        }

        /// <summary>Опис образу (необов'язкове поле).</summary>
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        /// <summary>Дата та час створення образу.</summary>
        public DateTime CreatedDate
        {
            get => _createdDate;
            set
            {
                _createdDate = value;
                OnPropertyChanged(nameof(CreatedDate));
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