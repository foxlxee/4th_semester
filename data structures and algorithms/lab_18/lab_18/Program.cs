using System;

namespace lab_18 {
    internal class Program {

        // Метод для решения задачи о суммах подмножеств с жадным алгоритмом
        public static bool SubsetSum(int[] numbers, int targetSum) {
            // Сортируем массив по убыванию
            Array.Sort(numbers);
            Array.Reverse(numbers);

            int currentSum = 0;

            // Проходим по отсортированным элементам
            foreach (int num in numbers) {
                // Добавляем число к текущей сумме, если не превышаем целевой суммы
                if (currentSum + num <= targetSum) {
                    currentSum += num;

                    // Если достигли целевой суммы, то возвращаем true
                    if (currentSum == targetSum) {
                        return true;
                    }
                }
            }

            // Если не удалось достичь целевой суммы, возвращаем false
            return false;
        }

        static void Main(string[] args) {
            int[] numbers = { 3, 34, 4, 12, 5, 2 };
            int targetSum = 9;

            bool found = SubsetSum(numbers, targetSum);
            Console.WriteLine(found ? "Существует подмножество с заданной суммой" : "Подмножество с заданной суммой не найдено");

            Console.ReadKey();
        }
    }
}