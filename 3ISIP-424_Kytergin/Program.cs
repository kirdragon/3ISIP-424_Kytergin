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
            List<TextStatistics> textStatisticsList = new List<TextStatistics>();
            string glas = "аеёиоуыэюяeyuioa";
            string soglas = "бвгджзйклмнпрстфхцчшщqwrtpsdfghjklzxcvbnm";
            WordWork wordwork = new WordWork();

            while (true)
            {
                int glascount = 0;
                int sogcount = 0;
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

                int WordCount = wordwork.CountWords(text);
                Console.WriteLine($"Количество слов: {WordCount}");

                int LongestWord = wordwork.LongestWord(text);
                Console.WriteLine($"Самое длинное слово содержит {LongestWord} символ(ов)");

                int ShortestWord = wordwork.ShortestWord(text);
                Console.WriteLine($"Самое короткое слово содержит {ShortestWord} символ(ов)");

                int sentencescount = wordwork.SentencesCount(text);
                Console.WriteLine($"Количество предложений: {sentencescount}");

                Console.WriteLine("Частота букв: ");

                Dictionary<char, int> letterFrequency = wordwork.LetterFrequency(text);

                foreach (var letter in letterFrequency)
                {
                    if (glas.Contains(letter.Key))
                    {
                        Console.WriteLine($"Гласная буква '{letter.Key}' встречается {letter.Value} раз(а)");
                    }
                    else if (soglas.Contains(letter.Key))
                    {
                        Console.WriteLine($"Согласная буква '{letter.Key}' встречается {letter.Value} раз(а)");
                    }
                }

                foreach (char letter in text.ToLower())
                {
                    if (glas.Contains(letter))
                    {
                        glascount++;
                    }
                    else if (soglas.Contains(letter))
                    {
                        sogcount++;
                    }
                }

                Console.WriteLine($"Количество гласных букв: {glascount}");
                Console.WriteLine($"Количество согласных букв: {sogcount}");

                TextStatistics current = new TextStatistics();
                current.WordCount = WordCount;
                current.LongestWord = LongestWord;
                current.ShortestWord = ShortestWord;
                current.SentencesCount = sentencescount;
                current.VowelsCount = glascount;
                current.ConsonantsCount = sogcount;
                current.LetterFrequency = letterFrequency;

                textStatisticsList.Add(current);

                Console.WriteLine();
                Console.Write("Ввести новый текст? (да/нет): ");
                string answer = Console.ReadLine().ToLower();

                if (answer != "да")
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("===== Статистика всех текстов =====");

            for (int i = 0; i < textStatisticsList.Count; i++)
            {
                TextStatistics statistics = textStatisticsList[i];

                Console.WriteLine();
                Console.WriteLine($"Текст №{i + 1}");
                Console.WriteLine($"Количество слов: {statistics.WordCount}");
                Console.WriteLine($"Самое длинное слово: {statistics.LongestWord} символ(ов)");
                Console.WriteLine($"Самое короткое слово: {statistics.ShortestWord} символ(ов)");
                Console.WriteLine($"Количество предложений: {statistics.SentencesCount}");
                Console.WriteLine($"Количество гласных букв: {statistics.VowelsCount}");
                Console.WriteLine($"Количество согласных букв: {statistics.ConsonantsCount}");
                Console.WriteLine("Частота букв:");

                foreach (var letter in statistics.LetterFrequency)
                {
                    Console.WriteLine($"{letter.Key}: {letter.Value}");
                }
            }
        }

        class WordWork
        {
            public string[] Getwords(string text)
            {
                return text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }

            public int CountWords(string text)
            {
                string[] words = Getwords(text);
                return words.Length;
            }

            public int LongestWord(string text)
            {
                string[] words = Getwords(text);
                int longest = words[0].Length;
                int WordLenght;

                foreach (string word in words)
                {
                    WordLenght = word.Length;

                    if (WordLenght > longest)
                    {
                        longest = WordLenght;
                    }
                }

                return longest;
            }

            public int SentencesCount(string text)
            {
                string[] sentences = text.Split(
                    new char[] { '.', '!', '?' },
                    StringSplitOptions.RemoveEmptyEntries
                );

                return sentences.Length;
            }

            public int ShortestWord(string text)
            {
                string[] words = Getwords(text);
                int shortest = words[0].Length;
                int WordLenght;

                foreach (string word in words)
                {
                    WordLenght = word.Length;

                    if (WordLenght < shortest)
                    {
                        shortest = WordLenght;
                    }
                }

                return shortest;
            }

            public Dictionary<char, int> LetterFrequency(string text)
            {
                text = text.ToLower();
                Dictionary<char, int> frequency = new Dictionary<char, int>();

                foreach (char letter in text)
                {
                    if (char.IsLetter(letter))
                        if (frequency.ContainsKey(letter))
                        {
                            frequency[letter]++;
                        }
                        else
                        {
                            frequency.Add(letter, 1);
                        }
                }

                return frequency;
            }
        }

        class TextStatistics
        {
            public int WordCount;
            public int LongestWord;
            public int ShortestWord;
            public int SentencesCount;
            public int VowelsCount;
            public int ConsonantsCount;
            public Dictionary<char, int> LetterFrequency;
        }
    }
}