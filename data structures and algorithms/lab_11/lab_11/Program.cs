using System;
using System.Collections.Generic;
using System.IO;

namespace lab_11 {
    // Класс, который представляет конечный автомат
    internal class FiniteAutomatonSearch {
        int[,] transitionTable; // Таблица переходов для автомата
        int patternLength;      // Длина образца
        string pattern;         // Образец для поиска

        public const int ASCII_SIZE = 256; // Размер алфавита (ASCII)

        // Конструктор, который принимает образец
        public FiniteAutomatonSearch(string pattern) {
            this.pattern = pattern;
            patternLength = pattern.Length;
            buildTransitionTable();
        }

        // Метод, который строит таблицу переходов
        void buildTransitionTable() {
            transitionTable = new int[patternLength + 1, ASCII_SIZE];

            for (int state = 0; state <= patternLength; state++) {
                for (int x = 0; x < ASCII_SIZE; x++) {
                    transitionTable[state, x] = getNextState(state, (char)x);
                }
            }
        }

        // Метод, которвый возвращает следующее состояние автомата для текущего символа
        int getNextState(int state, char x) {
            if (state < patternLength && x == pattern[state])
                return state + 1;

            for (int ns = state; ns > 0; ns--) {
                if (pattern[ns - 1] == x) {
                    int i;
                    for (i = 0; i < ns - 1; i++)
                        if (pattern[i] != pattern[state - ns + 1 + i])
                            break;

                    if (i == ns - 1)
                        return ns;
                }
            }
            return 0;
        }

        // Метод, проходит по всему тексту, используя автомат для переходов,
        // и фиксирует индексы начала совпадений
        public List<int> Search(string text) {
            int textLength = text.Length;
            int currentState = 0;
            List<int> result = new List<int>();

            for (int i = 0; i < textLength; i++) {
                currentState = transitionTable[currentState, text[i]];
                if (currentState == patternLength) {
                    result.Add(i - patternLength + 1); // Совпадение найдено
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
                
                List<int> result = new FiniteAutomatonSearch(textToSearch).Search(text);

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