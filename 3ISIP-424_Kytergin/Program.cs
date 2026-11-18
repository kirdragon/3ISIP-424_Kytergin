
using System;
using System.Linq;
using _3ISIP_424_Kytergin.Services;

namespace _3ISIP_424_Kytergin
{
    class Program
    {
        static User currentUser;

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\nМАРКЕТПЛЕЙС");
                Console.WriteLine("1. Просмотреть каталог");
                Console.WriteLine("2. Войти в аккаунт");
                Console.WriteLine("3. Регистрация");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowCatalog();
                        break;

                    case "2":
                        Login();
                        break;

                    case "3":
                        Register();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Ошибка: выберите существующий пункт меню.");
                        break;
                }
            }
        }
        static void ShowCatalog()
        {
            Console.WriteLine("\nКАТАЛОГ");

            var products = ProductService.GetAllProducts();

            if (products.Count == 0)
            {
                Console.WriteLine("Товаров пока нет.");
                return;
            }

            foreach (var product in products)
            {
                Console.WriteLine(
                    $"{product.Id}. {product.Name} | " +
                    $"{product.Price} руб. | " +
                    $"Остаток: {product.StockQuantity}");
            }
        }
        static void Register()
        {
            Console.WriteLine("\nРЕГИСТРАЦИЯ");

            Console.Write("Придумайте логин: ");
            string username = Console.ReadLine();

            Console.Write("Придумайте пароль: ");
            string password = ReadPassword();

            Console.Write("\nПовторите пароль: ");
            string confirmPassword = ReadPassword();

            bool result = UserService.Register(
                username,
                password,
                confirmPassword,
                out string message);

            Console.WriteLine("\n" + message);
        }
        static void Login()
        {
            Console.WriteLine("\nВХОД");

            Console.Write("Логин: ");
            string username = Console.ReadLine();

            Console.Write("Пароль: ");
            string password = ReadPassword();

            currentUser = UserService.Login(username, password);

            if (currentUser == null)
            {
                Console.WriteLine("\nНеверный логин или пароль.");
                return;
            }

            Console.WriteLine("\nВы успешно вошли в аккаунт!");
            UserMenu();
        }
        static void UserMenu()
        {
            while (currentUser != null)
            {
                Console.WriteLine("\nЛИЧНЫЙ КАБИНЕТ");
                Console.WriteLine("1. Каталог товаров");
                Console.WriteLine("2. Добавить товар в корзину");
                Console.WriteLine("3. Просмотреть корзину");
                Console.WriteLine("4. Удалить товар из корзины");
                Console.WriteLine("5. Купить один товар");
                Console.WriteLine("6. Купить всю корзину");
                Console.WriteLine("7. История заказов");
                Console.WriteLine("0. Выйти из аккаунта");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowCatalog();
                        break;

                    case "2":
                        AddToCart();
                        break;

                    case "3":
                        ShowCart();
                        break;

                    case "4":
                        RemoveFromCart();
                        break;

                    case "5":
                        BuyOne();
                        break;

                    case "6":
                        BuyAll();
                        break;

                    case "7":
                        ShowHistory();
                        break;

                    case "0":
                        currentUser = null;
                        Console.WriteLine("Вы вышли из аккаунта.");
                        break;

                    default:
                        Console.WriteLine("Ошибка: выберите существующий пункт меню.");
                        break;
                }
            }
        }
        static void AddToCart()
        {
            ShowCatalog();

            Console.Write("\nВведите ID товара: ");
            int productId = ReadInt();

            Console.Write("Введите количество: ");
            int quantity = ReadInt();

            CartService.AddToCart(
                currentUser.Id,
                productId,
                quantity,
                out string message);

            Console.WriteLine(message);
        }
        static void ShowCart()
        {
            Console.WriteLine("\nКОРЗИНА");

            var items = CartService.GetCartItems(currentUser.Id);

            if (items.Count == 0)
            {
                Console.WriteLine("Корзина пуста.");
                return;
            }

            decimal total = 0;

            foreach (var item in items)
            {
                decimal sum = item.Product.Price * item.Quantity;
                total += sum;

                Console.WriteLine(
                    $"{item.Id}. {item.Product.Name} | " +
                    $"Количество: {item.Quantity} | " +
                    $"Сумма: {sum} руб.");
            }

            Console.WriteLine($"Итого: {total} руб.");
        }
        static void RemoveFromCart()
        {
            ShowCart();

            Console.Write("\nВведите ID позиции корзины: ");
            int id = ReadInt();

            bool result = CartService.RemoveFromCart(
                currentUser.Id,
                id);

            Console.WriteLine(result
                ? "Товар удалён из корзины."
                : "Не удалось удалить товар.");
        }
        static void BuyOne()
        {
            ShowCart();

            Console.Write("\nВведите ID позиции корзины: ");
            int cartItemId = ReadInt();

            int pickupPointId = ChoosePickupPoint();

            if (pickupPointId == 0)
            {
                Console.WriteLine("Покупка отменена.");
                return;
            }

            OrderService.PurchaseCartItem(
                currentUser.Id,
                cartItemId,
                pickupPointId,
                out string message);

            Console.WriteLine(message);
        }
        static void BuyAll()
        {
            ShowCart();

            int pickupPointId = ChoosePickupPoint();

            if (pickupPointId == 0)
            {
                Console.WriteLine("Покупка отменена.");
                return;
            }

            OrderService.PurchaseCart(
                currentUser.Id,
                pickupPointId,
                out string message);

            Console.WriteLine(message);
        }
        static int ChoosePickupPoint()
        {
            Console.WriteLine("\nПУНКТЫ ВЫДАЧИ");

            var points = OrderService.GetPickupPoints();

            if (points.Count == 0)
            {
                Console.WriteLine("Доступных пунктов выдачи нет.");
                return 0;
            }

            foreach (var point in points)
            {
                Console.WriteLine(
                    $"{point.Id}. {point.Name} | {point.Address}");
            }

            Console.Write("Введите ID ПВЗ (0 — отмена): ");

            return ReadInt();
        }
        static void ShowHistory()
        {
            Console.WriteLine("\nИСТОРИЯ ЗАКАЗОВ");

            var orders = OrderService.GetOrderHistory(currentUser.Id);

            if (orders.Count == 0)
            {
                Console.WriteLine("У вас пока нет заказов.");
                return;
            }

            foreach (var order in orders)
            {
                Console.WriteLine(
                    $"\nЗаказ №{order.Id} | " +
                    $"Дата: {order.CreatedAt:g} | " +
                    $"Статус: {order.Status}");

                foreach (var item in order.OrderItem)
                {
                    Console.WriteLine(
                        $"  {item.Product.Name} | " +
                        $"Количество: {item.Quantity} | " +
                        $"Цена: {item.UnitPrice} руб.");
                }
            }
        }
        static int ReadInt()
        {
            while (true)
            {
                string input = Console.ReadLine();

                if (int.TryParse(input, out int value))
                    return value;

                Console.Write("Ошибка. Введите целое число: ");
            }
        }
        static string ReadPassword()
        {
            string password = "";

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                    break;

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Substring(
                            0,
                            password.Length - 1);

                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            }

            return password;
        }
    }
}
