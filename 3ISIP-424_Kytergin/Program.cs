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
            string glas = "аеёиоуыэюяeyuioa";
            string soglas = "бвгджзйклмнпрстфхцчшщqwrtpsdfghjklzxcvbnm";
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
            int LongestWord = wordwork.LongestWord(text);
            Console.WriteLine($"Самое длинное слово содержит {LongestWord} символ(ов)");
            int ShortestWord = wordwork.ShortestWord(text);
            Console.WriteLine($"Самое короткое слово содержит {ShortestWord} символ(ов)");
            int sentencescount = wordwork.SentencesCount(text);
            Console.WriteLine($"Количество предложений: {sentencescount}");
            Console.WriteLine("Частота букв: ");
            foreach (var letter in wordwork.LetterFrequency(text))
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
                foreach(string word in words)
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
                foreach(char letter in text)
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
    }
}