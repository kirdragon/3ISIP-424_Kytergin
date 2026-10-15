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
                    switch (choice)
                    {
                        case 1:
                            {
                                Console.Write("Введите название книги: ");
                                string bookName = Console.ReadLine();
                                Console.Write("Введите автора: ");
                                string bookAuthor = Console.ReadLine();
                                Console.Write("Введите жанр: ");
                                string bookGenre = Console.ReadLine();
                                int bookYear = GetInt("Введите год издания: ", 0, 2026);
                                Console.Write("Введите цену: ");
                                decimal bookPrice;
                                while (!decimal.TryParse(Console.ReadLine(), out bookPrice) || bookPrice <= 0)
                                {
                                    Console.WriteLine("Некорректный ввод. Введите цену в формате decimal.");
                                }
                                books.Add(new Book(bookName, bookAuthor, bookGenre, bookYear, bookPrice));
                                break;
                            }

                        case 2:
                            {
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
                                var foundBooks = new List<Book>();
                                int num = GetInt("1. Найти по названию\n2. По автору\n3. По жанру\nВыбор: ",1,3);
                            if (num == 1)
                            { 
                                Console.Write("Введите название книги: ");
                                string bookName = Console.ReadLine();

                                foundBooks = books.Where(b => b.book_name.Contains(bookName)).ToList();
                            }
                            else if (num == 2)
                            {
                                Console.Write("Введите автора книги: ");
                                string bookAuthor = Console.ReadLine();
                                foundBooks = books.Where(b => b.book_author.Contains(bookAuthor)).ToList();
                            }
                            else if (num == 3)
                            {
                                Console.Write("Введите жанр книги: ");
                                string bookGenre = Console.ReadLine();
                                foundBooks = books.Where(b => b.book_genre.Contains(bookGenre)).ToList();
                            }
                            if (foundBooks.Count == 0)
                            {
                                Console.WriteLine("Книги не найдены.");
                            }
                            else
                            {
                                foreach (var book in foundBooks)
                                {
                                    Console.WriteLine($"ID: {book.book_id}, Название: {book.book_name}, Автор: {book.book_author}, Жанр: {book.book_genre}, Год издания: {book.book_year}, Цена: {book.book_price}");
                                }
                            }
                             break;
                            }
                    }
                }
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
