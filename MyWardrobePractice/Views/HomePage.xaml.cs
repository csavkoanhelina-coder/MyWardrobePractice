using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using MyWardrobe.Models;
using Newtonsoft.Json;

namespace MyWardrobe.Views
{
    /// <summary>
    /// Головна сторінка додатку, яка відображає всю колекцію одягу та дозволяє виконувати пошук за назвою.
    /// </summary>
    public partial class HomePage : Page
    {
        /// <summary>
        /// Колекція всіх невидалених речей (використовується для відображення та пошуку).
        /// </summary>
        public ObservableCollection<Clothing> clothes;

        /// <summary>
        /// Ініціалізує компоненти сторінки, завантажує дані з JSON-файлу та оновлює лічильник речей.
        /// </summary>
        public HomePage()
        {
            InitializeComponent();
            LoadData();
            ItemsList.ItemsSource = clothes;
            UpdateCountText();
        }

        /// <summary>
        /// Завантажує з файлу clothes.json усі невидалені речі (IsDeleted == false).
        /// У разі помилки показує повідомлення та створює порожню колекцію.
        /// </summary>
        private void LoadData()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "clothes.json");

                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    var items = JsonConvert.DeserializeObject<ObservableCollection<Clothing>>(json);

                    clothes = new ObservableCollection<Clothing>();

                    foreach (var item in items)
                    {
                        if (!item.IsDeleted)
                        {
                            clothes.Add(item);
                        }
                    }
                }
                else
                {
                    clothes = new ObservableCollection<Clothing>();
                    MessageBox.Show("Файл з даними не знайдено!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження: {ex.Message}");
                clothes = new ObservableCollection<Clothing>();
            }
        }

        /// <summary>
        /// Оновлює текстовий блок CountText, відображаючи загальну кількість речей у колекції.
        /// </summary>
        private void UpdateCountText()
        {
            if (clothes != null)
            {
                CountText.Text = $"Всього речей: {clothes.Count}";
            }
        }

        /// <summary>
        /// Виконує пошук речей за назвою (нечутливий до регістру).
        /// Результати відображаються в ResultList. Якщо нічого не знайдено – виводить відповідне повідомлення.
        /// </summary>
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                ResultList.ItemsSource = null;
                return;
            }

            ObservableCollection<Clothing> result = new ObservableCollection<Clothing>();

            foreach (var item in clothes)
            {
                if (item.Name.ToLower().Contains(SearchBox.Text.ToLower()))
                {
                    result.Add(item);
                }
            }

            ResultList.ItemsSource = result;

            if (result.Count == 0)
            {
                CustomMessageBox.ShowInfo($"Нічого не знайдено за запитом \"{SearchBox.Text}\"");
            }
            else
            {
                CustomMessageBox.ShowSuccess($"Знайдено {result.Count} речей!");
            }
        }
    }
}