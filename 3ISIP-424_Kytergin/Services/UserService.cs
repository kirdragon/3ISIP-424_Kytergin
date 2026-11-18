
using System;
using System.Linq;
using System.Security.Cryptography;

namespace _3ISIP_424_Kytergin.Services
{
    public class UserService
    {
        // Регистрация нового пользователя
        public static bool Register(
            string username,
            string password,
            string confirmPassword,
            out string message)
        {
            username = username == null ? "" : username.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                message = "Введите имя пользователя.";
                return false;
            }

            if (username.Length > 50)
            {
                message = "Имя пользователя не должно превышать 50 символов.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                message = "Пароль должен содержать минимум 6 символов.";
                return false;
            }

            if (password != confirmPassword)
            {
                message = "Пароли не совпадают.";
                return false;
            }

            // Проверяем, свободно ли имя пользователя
            bool exists = Core.Context.User.Any(
                u => u.Username == username);

            if (exists)
            {
                message = "Пользователь с таким именем уже существует.";
                return false;
            }

            // Создаём пользователя
            User user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                CreatedAt = DateTime.Now
            };

            // Создаём пользователя и его корзину в одной транзакции
            using (var transaction = Core.Context.Database.BeginTransaction())
            {
                Core.Context.User.Add(user);
                Core.Context.SaveChanges();

                Cart cart = new Cart
                {
                    UserId = user.Id
                };

                Core.Context.Cart.Add(cart);
                Core.Context.SaveChanges();

                transaction.Commit();
            }

            message = "Регистрация прошла успешно!";
            return true;
        }

        // Авторизация пользователя
        public static User Login(string username, string password)
        {
            username = username == null ? "" : username.Trim();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrEmpty(password))
            {
                return null;
            }

            User user = Core.Context.User.FirstOrDefault(
                u => u.Username == username);

            if (user == null)
            {
                return null;
            }

            if (!VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        // Создание хеша пароля
        private static string HashPassword(string password)
        {
            byte[] salt = new byte[16];

            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(salt);
            }

            const int iterations = 100000;

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password, salt, iterations))
            {
                byte[] hash = pbkdf2.GetBytes(32);

                return iterations + ":" +
                    Convert.ToBase64String(salt) + ":" +
                    Convert.ToBase64String(hash);
            }
        }

        // Проверка введённого пароля
        private static bool VerifyPassword(
            string password,
            string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash))
                return false;

            try
            {
                string[] parts = storedHash.Split(':');

                if (parts.Length != 3)
                    return false;

                int iterations = int.Parse(parts[0]);

                if (iterations < 1 || iterations > 1000000)
                    return false;

                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] originalHash = Convert.FromBase64String(parts[2]);

                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    password, salt, iterations))
                {
                    byte[] newHash = pbkdf2.GetBytes(originalHash.Length);

                    if (newHash.Length != originalHash.Length)
                        return false;

                    int difference = 0;

                    for (int i = 0; i < newHash.Length; i++)
                    {
                        difference |= newHash[i] ^ originalHash[i];
                    }

                    return difference == 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
