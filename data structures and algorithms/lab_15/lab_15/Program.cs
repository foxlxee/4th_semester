using System;
using System.IO;
using System.Collections.Generic;

namespace lab_15 {
    // Класс, который представляет граф
    internal class Graph {

        // Матрица смежности графа
        int[,] adjacencyMatrix;

        // Количество строк и столбцов матрицы смежности
        int adjacencyMatrixOrder;

        // Список смежности
        List<List<int>> adjacencyList;

        // Конструктор, который принимает путь к файлу, в котором записана матрица смежности графа
        public Graph(string adjacencyMatrixFilePath) {

            // Считываем все строки из файла
            string[] rows = File.ReadAllLines(adjacencyMatrixFilePath);

            // Записываем количество строк и столбцов (порядок матрицы)
            adjacencyMatrixOrder = rows.Length;

            // Выделяем память под матрицу смежности
            adjacencyMatrix = new int[adjacencyMatrixOrder, adjacencyMatrixOrder];

            // Пробегаем по всем строкам
            for (int i = 0; i < rows.Length; i++) {

                // Разбиваем строку на подстроки
                string[] elementsInRow = rows[i].Split(' ');

                // Заполняем строку
                for (int j = 0; j < elementsInRow.Length; j++) {
                    adjacencyMatrix[i, j] = int.Parse(elementsInRow[j]);
                }
            }

            // Выделяем память под список смежности
            adjacencyList = new List<List<int>>(adjacencyMatrixOrder);

            // Заполняем список смежности
            for (int i = 0; i < adjacencyMatrixOrder; i++) {
                List<int> list = new List<int>();

                for (int j = 0; j < adjacencyMatrixOrder; j++) {
                    if (adjacencyMatrix[i, j] == 1) {
                        list.Add(j);
                    }
                }

                adjacencyList.Add(list);
            }
        }

        // Метод, который выводит в консоль матрицу смежности
        public void DisplayAdjacencyMatrix() {
            Console.WriteLine("Матрица смежности графа:");

            for (int i = 0; i < adjacencyMatrixOrder; i++) {
                for (int j = 0; j < adjacencyMatrixOrder; j++) {
                    Console.Write(adjacencyMatrix[i, j].ToString());
                    if (j != adjacencyMatrixOrder - 1) Console.Write(' '); else Console.WriteLine();
                }
            }

            Console.WriteLine();
        }

        // Метод, который выводит в консоль список смежности
        public void DisplayAdjacencyList() {
            Console.WriteLine("Список смежности графа:");

            for (int i = 0; i < adjacencyList.Count; i++) {
                Console.Write("Вершина " + i.ToString() + ", Смежные вершины: ");

                List<int> list = adjacencyList[i];

                if (list.Count == 0) {
                    Console.Write("нет смежных вершин");
                    Console.WriteLine();
                    continue;
                }

                for (int j = 0; j < list.Count; j++) {
                    Console.Write(list[j]);
                    if (j != list.Count - 1) Console.Write(','); else Console.WriteLine();
                }
            }

            Console.WriteLine();
        }

        // Метод для раскраски графа
        public void ColorGraph() {
            // Массив для хранения цвета каждой вершины (-1 значит, что цвет не назначен)
            int[] result = new int[adjacencyMatrixOrder];
            for (int i = 0; i < adjacencyMatrixOrder; i++)
                result[i] = -1;

            // Массив для отслеживания доступных цветов
            bool[] available = new bool[adjacencyMatrixOrder];

            // Назначаем первый цвет первой вершине
            result[0] = 0;

            // Назначаем цвета оставшимся вершинам
            for (int u = 1; u < adjacencyMatrixOrder; u++) {
                // Помечаем цвета смежных вершин как недоступные
                foreach (int i in adjacencyList[u]) {
                    if (result[i] != -1)
                        available[result[i]] = true;
                }

                // Находим первый доступный цвет
                int color;
                for (color = 0; color < adjacencyMatrixOrder; color++) {
                    if (!available[color])
                        break;
                }

                result[u] = color; // назначаем найденный цвет

                // Сбрасываем доступные цвета для следующей вершины
                foreach (int i in adjacencyList[u]) {
                    if (result[i] != -1)
                        available[result[i]] = false;
                }
            }

            // Выводим результаты раскраски
            for (int u = 0; u < adjacencyMatrixOrder; u++)
                Console.WriteLine($"Вершина {u} ---> Цвет {result[u]}");
        }
    }

    internal class Program {
        static void Main(string[] args) {
            Graph graph = new Graph("..//..//..//..//graph.txt");

            graph.DisplayAdjacencyMatrix();
            graph.DisplayAdjacencyList();
            graph.ColorGraph();

            Console.ReadKey();
        }
    }
}