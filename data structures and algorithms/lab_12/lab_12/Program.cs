using System;
using System.Collections.Generic;
using System.IO;

namespace lab_12 {
    internal class KnuthMorrisPrattSearch {
        int[] prefixFunction; // Префикс-функция для образца
        string pattern;       // Образец для поиска

        // Конструктор, который принимает образец
        public KnuthMorrisPrattSearch(string pattern) {
            this.pattern = pattern;
            prefixFunction = buildPrefixFunction(pattern);
        }

        // Метод, который строит префикс-функцию
        int[] buildPrefixFunction(string pattern) {
            int m = pattern.Length;
            int[] prefixFunction = new int[m];
            int k = 0;

            for (int i = 1; i < m; i++) {
                while (k > 0 && pattern[k] != pattern[i])
                    k = prefixFunction[k - 1];

                if (pattern[k] == pattern[i])
                    k++;

                prefixFunction[i] = k;
            }

            return prefixFunction;
        }

        // Метод, который проходит по тексту, используя префикс-функцию для поиска совпадений
        public List<int> Search(string text) {
            int n = text.Length;
            int m = pattern.Length;
            int j = 0; // Индекс для pattern
            List<int> result = new List<int>();

            for (int i = 0; i < n; i++) {
                while (j > 0 && text[i] != pattern[j])
                    j = prefixFunction[j - 1];

                if (text[i] == pattern[j])
                    j++;

                if (j == m) {
                    result.Add(i - m + 1); // Совпадение найдено
                    j = prefixFunction[j - 1];
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

                List<int> result = new KnuthMorrisPrattSearch(textToSearch).Search(text);

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