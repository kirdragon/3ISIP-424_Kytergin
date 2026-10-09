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
