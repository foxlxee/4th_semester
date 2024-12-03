using System;
using System.Collections.Generic;
using System.IO;

namespace lab_8 {
    // Класс, который представляет граф
    internal class Graph {

        // Матрица смежности графа
        int[,] adjacencyMatrix;

        // Количество строк и столбцов матрицы смежности
        int adjacencyMatrixOrder;

        // Список смежности
        List<List<int>> adjacencyList;

        // Конструктор, который принимает матрицу смежности
        public Graph(int[,] adjacencyMatrix) {
            // Записываем матрицу смежности
            this.adjacencyMatrix = adjacencyMatrix;

            // Вычисляем порядок матрицы
            adjacencyMatrixOrder = (int)Math.Sqrt(adjacencyMatrix.Length);

            // Выделяем память под список смежности
            adjacencyList = getAdjacencyList();
        }

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
            adjacencyList = getAdjacencyList();
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

                    Console.Write(string.Format("({0})", adjacencyMatrix[i, list[j]].ToString()));

                    if (j != list.Count - 1) Console.Write(','); else Console.WriteLine();
                }
            }

            Console.WriteLine();
        }
        
        // Метод, который выводит в консоль кратчайшие пути из заданной вершины до всех остальных
        public void DisplayShortestPaths(int startVertexIndex) {
            Console.WriteLine("Кратчайшие пути:");

            Dictionary<int, int[]> paths = GetShortestPaths(startVertexIndex);
            for (int i = 0; i < adjacencyMatrixOrder; i++) {
                if (paths.ContainsKey(i)) {
                    Console.Write("От вершины " + startVertexIndex.ToString() + " до вершины " + i.ToString() + ": ");

                    int[] path = paths[i];
                    for (int j = 0; j < path.Length; j++) {
                        Console.Write(path[j]);

                        if (j != path.Length - 1) {
                            Console.Write(" -> ");
                        }
                    }

                    Console.WriteLine();
                }
            }
        }

        // Метод, который возвращает кратчайшие пути из заданной вершины
        // до всех остальных с помощью алгоритма Дейкстры
        public Dictionary<int, int[]> GetShortestPaths(int startVertexIndex) {
            // Инициализируем расстояние до всех вершин от текущей как int.MaxValue
            int[] distances = new int[adjacencyMatrixOrder];
            bool[] visited = new bool[adjacencyMatrixOrder];
            int[] from = new int[adjacencyMatrixOrder];

            for (int i = 0; i < adjacencyMatrixOrder; i++) {
                from[i] = -1;

                if (i == startVertexIndex) {
                    // до текущей вершины инициализируем расстояние равное нулю
                    distances[i] = 0;
                    continue;
                }

                distances[i] = int.MaxValue;
            }

            // Пробегаем все вершины в графе
            for (int i = 0; i < adjacencyMatrixOrder; i++) {
                int nearest = -1;

                // Находим ближайщую непосещенную вершину
                for (int v = 0; v < adjacencyMatrixOrder; v++) {
                    if (!visited[v] && (nearest == -1 || distances[nearest] > distances[v])) {
                        nearest = v;
                    }
                }

                // Помечаем вершину как обработанную
                visited[nearest] = true;

                // Релаксируем рёбра
                for (int j = 0; j < adjacencyMatrixOrder; j++) {
                    int weight = adjacencyMatrix[nearest, j];
                    if (weight != 0) {
                        if (distances[j] > distances[nearest] + weight) {
                            distances[j] = distances[nearest] + weight;
                            from[j] = nearest;
                        }
                    }
                }
            }

            // Вычисляем кратчайшие пути
            Dictionary<int, int[]> paths = new Dictionary<int, int[]>();

            for (int vertexToReach = 0; vertexToReach < adjacencyList.Count; vertexToReach++) {
                if (vertexToReach != startVertexIndex && distances[vertexToReach] != int.MaxValue) {
                    List<int> path = new List<int>();
                    for (int i = vertexToReach; i != -1; i = from[i]) {
                        path.Add(i);
                    }
                    path.Reverse();
                    paths.Add(vertexToReach, path.ToArray());
                }
            }

            return paths;
        }

        List<List<int>> getAdjacencyList() {
            List<List<int>> result = new List<List<int>>();

            // Заполняем список смежности
            for (int i = 0; i < adjacencyMatrixOrder; i++) {
                List<int> list = new List<int>();

                for (int j = 0; j < adjacencyMatrixOrder; j++) {
                    if (adjacencyMatrix[i, j] != 0) {
                        list.Add(j);
                    }
                }

                result.Add(list);
            }

            return result;
        }
    }

    internal class Program {
        static void Main(string[] args) {
            Graph graph = new Graph("..//..//..//..//weighted_graph.txt");
            Console.WriteLine("Список смежности исходного взвешенного графа:");
            graph.DisplayAdjacencyList();

            graph.DisplayShortestPaths(0);

            Console.ReadKey();
        }
    }
}