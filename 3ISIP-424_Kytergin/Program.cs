
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3ISIP_424_Kytergin
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Book> books = new List<Book>();

            while (true)
            {
                Console.WriteLine("1. Добавить книгу");
                Console.WriteLine("2. Удалить книгу");
                Console.WriteLine("3. Найти книгу и вывести информацию");
                Console.WriteLine("4. Отсортировать книги");
                Console.WriteLine("5. Вывести самую дорогую и самую дешевую книги");
                Console.WriteLine("6. Сгруппировать книги по авторам и вывести количество книг каждого автора");
                Console.WriteLine("7. Выход");

                int choice = GetInt("Выберите действие: ", 1, 7);

                switch (choice)
                {
                    case 1:
                        {
                            string bookName = GetText("Введите название книги: ");
                            string bookAuthor = GetText("Введите автора: ");
                            Genre bookGenre = GetGenre();
                            int bookYear = GetInt("Введите год издания: ", 1, DateTime.Now.Year);
                            decimal bookPrice = GetPrice();

                            books.Add(new Book(bookName, bookAuthor, bookGenre, bookYear, bookPrice));
                            Console.WriteLine("Книга добавлена.");
                            break;
                        }

                    case 2:
                        {
                            if (books.Count == 0)
                            {
                                Console.WriteLine("Список книг пуст.");
                                break;
                            }

                            int input_id = GetInt("Введите ID книги: ", 1, int.MaxValue);
                            Book book = books.FirstOrDefault(b => b.book_id == input_id);

                            if (book != null)
                            {
                                books.Remove(book);
                                Console.WriteLine("Книга удалена.");
                            }
                            else
                            {
                                Console.WriteLine("Книга с таким ID не найдена.");
                            }

                            break;
                        }

                    case 3:
                        {
                            if (books.Count == 0)
                            {
                                Console.WriteLine("Список книг пуст.");
                                break;
                            }

                            var foundBooks = new List<Book>();
                            int num = GetInt("1. Найти по названию\n2. По автору\n3. По жанру\nВыбор: ", 1, 3);

                            if (num == 1)
                            {
                                string bookName = GetText("Введите название книги: ");
                                foundBooks = books.Where(b => b.book_name.ToLower().Contains(bookName.ToLower())).ToList();
                            }
                            else if (num == 2)
                            {
                                string bookAuthor = GetText("Введите автора книги: ");
                                foundBooks = books.Where(b => b.book_author.ToLower().Contains(bookAuthor.ToLower())).ToList();
                            }
                            else
                            {
                                Genre bookGenre = GetGenre();
                                foundBooks = books.Where(b => b.book_genre == bookGenre).ToList();
                            }

                            if (foundBooks.Count == 0)
                            {
                                Console.WriteLine("Книги не найдены.");
                            }
                            else
                            {
                                foreach (var book in foundBooks)
                                {
                                    PrintBook(book);
                                }
                            }

                            break;
                        }

                    case 4:
                        {
                            if (books.Count == 0)
                            {
                                Console.WriteLine("Список книг пуст.");
                                break;
                            }

                            var sortedbooks = new List<Book>();
                            int num = GetInt("1. Отсортировать книги по названию\n2. По году выпуска\nВыбор: ", 1, 2);

                            if (num == 1)
                            {
                                sortedbooks = books.OrderBy(b => b.book_name).ToList();
                            }
                            else
                            {
                                sortedbooks = books.OrderBy(b => b.book_year).ToList();
                            }

                            foreach (var book in sortedbooks)
                            {
                                PrintBook(book);
                            }

                            break;
                        }

                    case 5:
                        {
                            if (books.Count == 0)
                            {
                                Console.WriteLine("Список книг пуст.");
                                break;
                            }

                            Book max_book = books.OrderByDescending(b => b.book_price).First();
                            Book min_book = books.OrderBy(b => b.book_price).First();

                            Console.WriteLine("Самая дорогая книга:");
                            PrintBook(max_book);
                            Console.WriteLine("Самая дешевая книга:");
                            PrintBook(min_book);
                            break;
                        }

                    case 6:
                        {
                            if (books.Count == 0)
                            {
                                Console.WriteLine("Список книг пуст.");
                                break;
                            }

                            var groups = books.GroupBy(b => b.book_author).OrderBy(g => g.Key);

                            foreach (var group in groups)
                            {
                                Console.WriteLine($"Автор: {group.Key}, количество книг: {group.Count()}");
                            }

                            break;
                        }

                    case 7:
                        return;
                }
            }
        }

        static string GetText(string message)
        {
            Console.Write(message);
            string value = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(value))
            {
                Console.Write("Поле не может быть пустым. Повторите ввод: ");
                value = Console.ReadLine();
            }

            return value.Trim();
        }

        static int GetInt(string message, int min, int max)
        {
            int value;
            Console.Write(message);

            while (!int.TryParse(Console.ReadLine(), out value) || value < min || value > max)
            {
                Console.WriteLine($"Некорректный ввод. Введите целое число в диапазоне {min} - {max}.");
            }

            return value;
        }

        static decimal GetPrice()
        {
            decimal price;
            Console.Write("Введите цену: ");

            while (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0)
            {
                Console.Write("Некорректный ввод. Введите цену больше нуля: ");
            }

            return price;
        }

        static Genre GetGenre()
        {
            Console.WriteLine("Выберите жанр:");

            foreach (Genre genre in Enum.GetValues(typeof(Genre)))
            {
                Console.WriteLine($"{(int)genre + 1}. {genre}");
            }

            int choice = GetInt("Ваш выбор: ", 1, Enum.GetValues(typeof(Genre)).Length);
            return (Genre)(choice - 1);
        }

        static void PrintBook(Book book)
        {
            Console.WriteLine($"ID: {book.book_id}, Название: {book.book_name}, Автор: {book.book_author}, Жанр: {book.book_genre}, Год издания: {book.book_year}, Цена: {book.book_price}");
        }

        class Book
        {
            private static int idCounter = 1;
            public int book_id { get; }
            public string book_name;
            public string book_author;
            public Genre book_genre;
            public int book_year;
            public decimal book_price;

            public Book(string book_name, string book_author, Genre book_genre, int book_year, decimal book_price)
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

        enum Genre
        {
            Roman,
            Fantasy,
            Detective,
            ScienceFiction,
            Romance
        }
    }
}
