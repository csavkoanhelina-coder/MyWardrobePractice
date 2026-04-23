using Microsoft.Win32;
using MyWardrobe.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Діалогове вікно для додавання нового одягу або взуття.
    /// Дозволяє ввести назву, розмір, колір, вибрати тип, сезон та завантажити фотографію.
    /// </summary>
    public partial class AddClothesWindow : Window
    {
        /// <summary>
        /// Отримує доданий предмет одягу після успішного збереження.
        /// </summary>
        public Clothing NewItem { get; set; }
        private string selectedImagePath = "";

        /// <summary>
        /// Ініціалізує компоненти вікна та встановлює типові значення для комбобоксів.
        /// </summary>
        public AddClothesWindow()
        {
            InitializeComponent();
            TypeBox.SelectedIndex = 0;
            SeasonBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Відкриває діалог вибору файлу для завантаження фотографії.
        /// Після вибору оновлює попередній перегляд та зберігає шлях.
        /// </summary>
        private void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Виберіть фото одягу";
            dialog.Filter = "Зображення (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp";

            if (dialog.ShowDialog() == true)
            {
                selectedImagePath = dialog.FileName;
                PreviewImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(selectedImagePath));
                ImagePathText.Text = Path.GetFileName(selectedImagePath);
            }
        }

        /// <summary>
        /// Перевіряє введені дані, копіює фото в папку проєкту,
        /// створює об'єкт Clothing та закриває вікно з результатом DialogResult = true.
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введіть назву!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(SizeBox.Text))
            {
                MessageBox.Show("Введіть розмір!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(ColorBox.Text))
            {
                MessageBox.Show("Введіть колір!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(selectedImagePath))
            {
                MessageBox.Show("Виберіть фото!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
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

            string imagePath = CopyImageToProject(selectedImagePath);

            NewItem = new Clothing
            {
                Name = NameBox.Text.Trim(),
                Size = SizeBox.Text.Trim(),
                Color = ColorBox.Text.Trim(),
                Type = type,
                Season = season,
                ImagePath = imagePath,
                IsFavorite = false,
                IsDeleted = false
            };

            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Копіює вибране зображення в папку Assets/Images проєкту.
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
        /// Скасовує додавання та закриває вікно з результатом DialogResult = false.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}