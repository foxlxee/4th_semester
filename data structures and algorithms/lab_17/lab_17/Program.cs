using System;
using System.Collections.Generic;

namespace lab_17 {
    internal class Program {

        // Метод для решения задачи о раскладке по ящикам
        public static int FirstFit(int[] items, int binCapacity) {
            // Список для хранения оставшегося места в каждом ящике
            List<int> bins = new List<int>();

            // Проходим по каждому предмету
            foreach (int item in items) {
                bool placed = false;

                // Проверяем, может ли предмет поместиться в один из существующих ящиков
                for (int i = 0; i < bins.Count; i++) {
                    if (bins[i] >= item) {
                        bins[i] -= item;
                        placed = true;
                        break;
                    }
                }

                // Если предмет не помещается в ни один существующий ящик, создаём новый
                if (!placed) {
                    bins.Add(binCapacity - item);
                }
            }

            // Количество ящиков, которое потребуется
            return bins.Count;
        }

        static void Main(string[] args) {
            int[] items = { 4, 8, 1, 4, 2, 1 };
            int binCapacity = 10;

            int binsNeeded = FirstFit(items, binCapacity);
            Console.WriteLine("Минимальное количество ящиков: " + binsNeeded);

            Console.ReadKey();
        }
    }
}