using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP_424_Kytergin
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int next_id = 1;
            List<Book> books = new List<Book>();
            int choice;
            while (true)
            {
                Console.WriteLine("1. Добавить книгу");
                Console.WriteLine("2. Удалить книгу");
                Console.WriteLine("3. Найти книгу и вывести информацию");
                Console.WriteLine("4. Отсортировать книги");
                Console.WriteLine("5. Вывести самую дорогую и самую дешевую книги");
                Console.WriteLine("6. Сгруппировать книги по авторам и вывести количество книг каждого автора");
                Console.WriteLine("7. Выход");
                choice = GetInt("Выберите действие: ", 1, 7);
            }
        }
        static int GetInt(string message, int min, int max)
        {
            int value;
            Console.WriteLine(message);
            while (!int.TryParse(Console.ReadLine(), out value) || value < min || value > max)
            {
                Console.WriteLine($"Некорректный ввод. Введите целое число в диапазоне {min} - {max}.");
            }
            return value;
        }
        class Book
        {
            public int book_id;
            public string book_name;
            public string book_author;
            public string book_genre;
            public int book_year;
            public decimal book_price;
            public Book(int book_id, string book_name, string book_author, string book_genre, int book_year, decimal book_price)
            {
                this.book_id = book_id;
                this.book_name = book_name;
                this.book_author = book_author;
                this.book_genre = book_genre;
                this.book_year = book_year;
                this.book_price = book_price;
            }
        }

    }
}
