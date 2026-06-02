using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MyWardrobe.Models;
using Newtonsoft.Json;

namespace MyWardrobe.Services
{
    /// <summary>
    /// Сервіс для роботи з авторизацією та реєстрацією користувачів.
    /// Зберігає дані у файлі users.json, паролі хешуються через SHA256.
    /// </summary>
    public class AuthService
    {
        private static string UsersPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "users.json");
        private static string DataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        /// <summary>
        /// Хешує пароль за допомогою алгоритму SHA256.
        /// </summary>
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Завантажує список усіх користувачів з файлу users.json.
        /// Якщо файл не існує, повертає порожній список.
        /// </summary>
        private static List<User> LoadUsers()
        {
            try
            {
                if (File.Exists(UsersPath))
                {
                    string json = File.ReadAllText(UsersPath);
                    return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
                }
            }
            catch
            {
            }
            return new List<User>();
        }

        /// <summary>
        /// Зберігає список користувачів у файл users.json.
        /// Автоматично створює теку Data, якщо вона не існує.
        /// </summary>
        private static void SaveUsers(List<User> users)
        {
            try
            {
                if (!Directory.Exists(DataFolder))
                {
                    Directory.CreateDirectory(DataFolder);
                }
                string json = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllText(UsersPath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка збереження користувачів: {ex.Message}");
            }
        }

        /// <summary>
        /// Реєструє нового користувача.
        /// Виконує валідацію логіну та пароля, перевіряє унікальність логіну.
        /// </summary>
        public static bool Register(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new Exception("Логін не може бути порожнім!");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
            {
                throw new Exception("Пароль повинен містити не менше 4 символів!");
            }

            var users = LoadUsers();

            if (users.Any(u => u.Login == login))
            {
                throw new Exception("Користувач з таким логіном вже існує!");
            }

            users.Add(new User
            {
                Login = login,
                PasswordHash = HashPassword(password)
            });

            SaveUsers(users);
            return true;
        }

        /// <summary>
        /// Виконує вхід користувача в систему.
        /// Перевіряє логін та пароль, повертає об'єкт User при успіху.
        /// </summary>
        public static User Login(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new Exception("Введіть логін!");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Введіть пароль!");
            }

            var users = LoadUsers();

            string passwordHash = HashPassword(password);

            var user = users.FirstOrDefault(u => u.Login == login && u.PasswordHash == passwordHash);

            if (user == null)
            {
                throw new Exception("Неправильний логін або пароль!");
            }

            return user;
        }
    }
}