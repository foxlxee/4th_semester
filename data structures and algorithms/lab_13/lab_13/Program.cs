using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lab_13 {
    internal class Program {
        static void Main(string[] args) {
            string text = File.ReadAllText("..//..//..//..//text.txt");

            Console.WriteLine("Текст: " + text);

            while (true) {
                Console.Write("Введите строку поиска: ");
                string textToSearch = Console.ReadLine();
                int[] indices = searchBoyerMoore(text, textToSearch);

                if (indices.Length == 0) {
                    Console.WriteLine();
                    Console.WriteLine("Образец не найден");
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine();

                foreach (int index in indices) {
                    Console.WriteLine("Образец найден на индексе: " + index);
                }

                ConsoleColor color = Console.ForegroundColor;

                for (int i = 0; i < text.Length;) {
                    if (indices.Contains(i)) {
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

        static int[] searchBoyerMoore(string text, string textToFind) {
            // Найденные индексы
            List<int> indices = new List<int>();

            // Заполняем таблицу смещений по правилу плохого символа
            int[] table = new int[256];
            for (int i = 0; i < table.Length; i++) {
                table[i] = -1;
            }
            for (int i = 0; i < textToFind.Length; i++) {
                table[textToFind[i]] = i;
            }

            // Длина образца
            int m = textToFind.Length;

            // Длина всего текста
            int n = text.Length;

            // Смещение относительно текста
            int leftOffset = 0;

            while (leftOffset <= (n - m)) {
                // Смещение относительно образца
                int j = m - 1;

                // Сравниваем шаблон с текстом справа налево
                while (j >= 0 && textToFind[j] == text[leftOffset + j]) j--;

                // Если шаблон совпал с подстрокой текста
                // добавляем текущее смещение слева в список найденных индексов
                // и вычисляем смещение на следующей итерации
                if (j < 0) {
                    indices.Add(leftOffset);
                    leftOffset += (leftOffset + m < n) ? m - table[text[leftOffset + m]] : 1;
                    continue;
                }

                // Если шаблон не совпал с подстрокой текста вычисляем смещение на следующей итерации
                leftOffset += Math.Max(1, j - table[text[leftOffset + j]]);
            }

            return indices.ToArray();
        }
    }
}