using System;
using System.IO;
using System.Collections.Generic;

namespace lab_9 {
    // Класс, который представляет граф
    internal class Graph {

        // Класс, который представляет ребро графа
        internal class Edge {
            public int Start { get; private set; }
            public int End { get; private set; }
            public int Weight { get; private set; }

            public Edge(int start, int end, int weight) {
                Start = start;
                End = end;
                Weight = weight;
            }
        }

        // Матрица смежности графа
        int[,] adjacencyMatrix;

        // Количество строк и столбцов матрицы смежности
        int adjacencyMatrixOrder;

        // Список смежности
        List<List<int>> adjacencyList;

        // Список рёбер
        List<Edge> edgesList;

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

            edgesList = getEdgesList();
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

        // Реализация алгоритма Беллмана-Форда
        public void BellmanFord(int startVertex) {
            // Инициализация расстояний до всех вершин как бесконечность, кроме стартовой
            int[] distances = new int[adjacencyMatrixOrder];
            for (int i = 0; i < adjacencyMatrixOrder; i++)
                distances[i] = int.MaxValue;
            distances[startVertex] = 0;

            // Релаксация рёбер |V| - 1 раз
            for (int i = 1; i <= adjacencyMatrixOrder - 1; i++) {
                foreach (Edge edge in edgesList) {
                    int u = edge.Start;
                    int v = edge.End;
                    int weight = edge.Weight;

                    if (distances[u] != int.MaxValue && distances[u] + weight < distances[v]) {
                        distances[v] = distances[u] + weight;
                    }
                }
            }

            // Проверка на наличие отрицательного цикла
            foreach (Edge edge in edgesList) {
                int u = edge.Start;
                int v = edge.End;
                int weight = edge.Weight;

                if (distances[u] != int.MaxValue && distances[u] + weight < distances[v]) {
                    Console.WriteLine("Граф содержит отрицательный цикл");
                    return;
                }
            }

            // Вывод кратчайших расстояний
            Console.WriteLine("Кратчайшие расстояния от вершины " + startVertex + ":");
            for (int i = 0; i < adjacencyMatrixOrder; i++) {
                Console.WriteLine($"До вершины {i}: {(distances[i] == int.MaxValue ? "∞" : distances[i].ToString())}");
            }
        }

        List<Edge> getEdgesList() {
            List<Edge> result = new List<Edge>();

            for (int i = 0; i < adjacencyMatrixOrder; i++) {
                for (int j = 0; j < adjacencyMatrixOrder; j++) {
                    int weight = adjacencyMatrix[i, j];
                    if (weight != 0) {
                        result.Add(new Edge(i, j, weight));
                    }
                }
            }

            result.Sort((Edge left, Edge right) => { return left.Weight > right.Weight ? 1 : -1; });

            return result;
        }
    }

    internal class Program {
        static void Main(string[] args) {
            Graph graph = new Graph("..//..//..//..//weighted_graph.txt");

            graph.DisplayAdjacencyMatrix();
            graph.DisplayAdjacencyList();
            graph.BellmanFord(0);

            Console.ReadKey();
        }
    }
}