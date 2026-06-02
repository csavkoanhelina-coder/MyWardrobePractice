using Microsoft.Win32;
using MyWardrobe.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Діалогове вікно для редагування існуючої речі гардеробу.
    /// Дозволяє змінювати назву, розмір, колір, тип, сезон та фотографію.
    /// </summary>
    public partial class EditClothesWindow : Window
    {
        /// <summary>
        /// Отримує відредагований предмет одягу після успішного збереження.
        /// </summary>
        public Clothing EditedItem { get; set; }

        private Clothing _originalItem;
        private string selectedImagePath = "";

        /// <summary>
        /// Ініціалізує новий екземпляр вікна редагування та заповнює поля даними вибраної речі.
        /// </summary>
        public EditClothesWindow(Clothing item)
        {
            InitializeComponent();
            _originalItem = item;

            NameBox.Text = item.Name;
            SizeBox.Text = item.Size;
            ColorBox.Text = item.Color;

            switch (item.Type)
            {
                case "верх": TypeBox.SelectedIndex = 0; break;
                case "низ": TypeBox.SelectedIndex = 1; break;
                case "плаття": TypeBox.SelectedIndex = 2; break;
                case "взуття": TypeBox.SelectedIndex = 3; break;
                default: TypeBox.SelectedIndex = 0; break;
            }

            switch (item.Season)
            {
                case "весна": SeasonBox.SelectedIndex = 0; break;
                case "літо": SeasonBox.SelectedIndex = 1; break;
                case "осінь": SeasonBox.SelectedIndex = 2; break;
                case "зима": SeasonBox.SelectedIndex = 3; break;
                default: SeasonBox.SelectedIndex = 0; break;
            }

            if (!string.IsNullOrEmpty(item.ImagePath))
            {
                try
                {
                    PreviewImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(item.ImagePath, UriKind.Relative));
                    ImagePathText.Text = Path.GetFileName(item.ImagePath);
                }
                catch { }
            }
        }

        /// <summary>
        /// Відкриває діалог вибору нового файлу фотографії та оновлює попередній перегляд.
        /// </summary>
        private void ChangeImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Виберіть нове фото";
            dialog.Filter = "Зображення (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp";

            if (dialog.ShowDialog() == true)
            {
                selectedImagePath = dialog.FileName;
                PreviewImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(selectedImagePath));
                ImagePathText.Text = Path.GetFileName(selectedImagePath);
            }
        }

        /// <summary>
        /// Перевіряє введені дані, копіює нове фото (якщо вибрано), створює оновлений об'єкт Clothing.
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                CustomMessageBox.ShowWarning("Введіть назву!");
                return;
            }
            if (string.IsNullOrWhiteSpace(SizeBox.Text))
            {
                CustomMessageBox.ShowWarning("Введіть розмір!");
                return;
            }
            if (string.IsNullOrWhiteSpace(ColorBox.Text))
            {
                CustomMessageBox.ShowWarning("Введіть колір!");
                return;
            }

            string type = "";
            if (TypeBox.SelectedItem is ComboBoxItem selectedType)
            {
                string t = selectedType.Content.ToString();
                if (t.Contains("Верх")) type = "верх";
                else if (t.Contains("Низ")) type = "низ";
                else if (t.Contains("Плаття")) type = "плаття";
                else if (t.Contains("Взуття")) type = "взуття";
            }

            string season = "";
            if (SeasonBox.SelectedItem is ComboBoxItem selectedSeason)
            {
                string s = selectedSeason.Content.ToString();
                if (s.Contains("Весна")) season = "весна";
                else if (s.Contains("Літо")) season = "літо";
                else if (s.Contains("Осінь")) season = "осінь";
                else if (s.Contains("Зима")) season = "зима";
            }

            string imagePath = _originalItem.ImagePath;
            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                imagePath = CopyImageToProject(selectedImagePath);
            }

            EditedItem = new Clothing
            {
                Id = _originalItem.Id,
                Name = NameBox.Text.Trim(),
                Size = SizeBox.Text.Trim(),
                Color = ColorBox.Text.Trim(),
                Type = type,
                Season = season,
                ImagePath = imagePath,
                IsFavorite = _originalItem.IsFavorite,
                IsDeleted = _originalItem.IsDeleted
            };

            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Копіює вибране зображення в папку Assets/Images проекту.
        /// Якщо файл з таким ім'ям вже існує, додає числовий суфікс.
        /// Повертає відносний шлях для збереження в JSON.
        /// </summary>
        private string CopyImageToProject(string sourcePath)
        {
            try
            {
                string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                string fileName = Path.GetFileName(sourcePath);
                string destinationPath = Path.Combine(imagesFolder, fileName);

                int counter = 1;
                string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                string extension = Path.GetExtension(fileName);

                while (File.Exists(destinationPath))
                {
                    string newName = $"{nameWithoutExt}_{counter}{extension}";
                    destinationPath = Path.Combine(imagesFolder, newName);
                    counter++;
                }

                File.Copy(sourcePath, destinationPath, true);
                return $"/Assets/Images/{Path.GetFileName(destinationPath)}";
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError($"Помилка копіювання фото: {ex.Message}");
                return "";
            }
        }

        /// <summary>
        /// Скасовує редагування та закриває вікно без збереження змін.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}