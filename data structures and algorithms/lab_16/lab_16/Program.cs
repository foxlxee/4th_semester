using System;

namespace lab_16 {
    internal class Program {

        // Метод для решения задачи о рюкзаке
        public static int Knapsack(int[] weights, int[] values, int capacity) {
            int n = weights.Length;

            // Создаем таблицу для хранения промежуточных значений
            int[,] dp = new int[n + 1, capacity + 1];

            // Заполняем таблицу с использованием подхода динамического программирования
            for (int i = 1; i <= n; i++) {
                for (int w = 1; w <= capacity; w++) {
                    // Если вес текущего предмета меньше или равен текущей вместимости
                    if (weights[i - 1] <= w) {
                        // Максимизируем ценность с включением или исключением предмета
                        dp[i, w] = Math.Max(dp[i - 1, w], dp[i - 1, w - weights[i - 1]] + values[i - 1]);
                    } else {
                        // Если вес превышает, оставляем значение без включения предмета
                        dp[i, w] = dp[i - 1, w];
                    }
                }
            }

            // Максимальная ценность будет в последней ячейке
            return dp[n, capacity];
        }

        static void Main(string[] args) {
            int[] values = new int[] { 60, 100, 120 }; // Стоимости предметов
            int[] weights = new int[] { 10, 20, 30 }; // Веса предметов
            int capacity = 50; // Вместимость рюкзака

            Console.WriteLine("Максимальная стоимость, которую можно уложить в рюкзак: " + Knapsack(weights, values, capacity));

            Console.ReadKey();
        }
    }
}