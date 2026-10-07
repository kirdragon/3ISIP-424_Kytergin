using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP_424_Kytergin
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string text;
            while (true)
            {
                Console.Write("Введите текст: ");
                text = Console.ReadLine();
                if (text.Length < 100)
                {
                    Console.WriteLine("Текст должен иметь длину минимум в 100 символов");
                }
                else
                {
                    Console.WriteLine("Текст принят");
                    break;
                }
            }
            WordWork wordwork = new WordWork();
            int WordCount = wordwork.CountWords(text);
            Console.WriteLine($"Количество слов: {WordCount}");
        }
        class WordWork
        {
            public int CountWords(string text)
            {
            string[] words = text.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
            }
        }
    }
}
