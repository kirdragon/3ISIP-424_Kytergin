using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP_424_Kytergin
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1. Добавить книгу");
                Console.WriteLine("2. Удалить книгу");
                Console.WriteLine("3. Найти книгу");
                Console.WriteLine("4. Отсортировать книги");
                Console.WriteLine("5. Вывести самую дешевую и дорогую книгу");
                Console.WriteLine("6. Сгруппировать книги по авторам и вывести количество книг каждого автора.");
                Console.WriteLine("7. Выйти");
                int choice = ReadInt("Выберите пункт: ", 1, 7);
            }
        }
        static int ReadInt(string message, int min, int max)
        {
            while(true)
            {
                Console.WriteLine(message);
                if (int.TryParse(Console.ReadLine(), out int result) && result >= min && result <=max)
                {
                    return result;
                }
                Console.WriteLine($"Ошибка! Введите число от {min} до {max}.");
            }
        }
        static decimal ReadDecimal(string message)
        {
            while (true)
            {
                if (decimal.TryParse(Console.ReadLine(), out decimal result) && result>=0)
                {
                    return result;
                }
                Console.WriteLine("Ошибка! Введите корректную цену.");
            }
        }
        class Book
        {
            private static int idCounter = 1;
            public int book_id {  get;}
            public string book_name;
            public string book_author;
            public string book_genre;
            public int book_year;
            public decimal book_price;
            public Book(string book_name, string book_author, string book_genre, int book_year, decimal book_price)
            {
                this.book_id = idCounter;
                this.book_name = book_name;
                this.book_author = book_author;
                this.book_genre = book_genre;
                this.book_year = book_year;
                this.book_price = book_price;
                idCounter++;
            }
        }
    }
}
