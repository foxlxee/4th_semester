using System;
using System.Collections.Generic;
using System.IO;

namespace lab_14 {
    internal class RabinKarpSearch {

        public const int ASCII_SIZE = 256; // Размер алфавита (ASCII)
        const int PRIME = 101;

        string pattern;
        int patternLength;

        // Конструктор, который принимает образец
        public RabinKarpSearch(string pattern) {
            this.pattern = pattern;
            patternLength = pattern.Length;
        }

        // Метод, который выполняет поиск образца в тексте
        public List<int> Search(string text) {
            List<int> result = new List<int>();

            int N = text.Length;
            int i, j;
            int p = 0;
            int t = 0;
            int h = 1;

            for (i = 0; i < patternLength - 1; i++)
                h = (h * ASCII_SIZE) % PRIME;

            for (i = 0; i < patternLength; i++) {
                p = (ASCII_SIZE * p + pattern[i]) % PRIME;
                t = (ASCII_SIZE * t + text[i]) % PRIME;
            }

            for (i = 0; i <= N - patternLength; i++) {
                if (p == t) {
                    for (j = 0; j < patternLength; j++) {
                        if (text[i + j] != pattern[j])
                            break;
                    }

                    if (j == patternLength) result.Add(i);
                }

                if (i < N - patternLength) {
                    t = (ASCII_SIZE * (t - text[i] * h) + text[i + patternLength]) % PRIME;

                    if (t < 0) t = (t + PRIME);
                }
            }

            return result;
        }
    }

    internal class Program {
        internal static void Main() {
            string text = File.ReadAllText("..//..//..//..//text.txt");

            Console.WriteLine("Текст: " + text);

            while (true) {
                Console.Write("Введите строку поиска: ");
                string textToSearch = Console.ReadLine();

                List<int> result = new RabinKarpSearch(textToSearch).Search(text);

                if (result.Count == 0) {
                    Console.WriteLine();
                    Console.WriteLine("Образец не найден");
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine();

                foreach (int index in result) {
                    Console.WriteLine("Образец найден на индексе: " + index);
                }

                ConsoleColor color = Console.ForegroundColor;

                for (int i = 0; i < text.Length;) {
                    if (result.Contains(i)) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(textToSearch);

                        i += textToSearch.Length;
                        continue;
                    }

                    Console.ForegroundColor = color;
                    Console.Write(text[i]);
                    i++;
                }
                Console.ForegroundColor = color;

                for (int i = 0; i < 3; i++) {
                    Console.WriteLine();
                }
            }
        }
    }
}