using System;
using System.Collections.Generic;
using System.IO;

namespace lab_4 {
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
        // до всех остальных с помощью поиска в ширину (BFS)
        public Dictionary<int, int[]> GetShortestPaths(int startVertexIndex) {

            // Массив расстояний
            int[] distances = new int[adjacencyList.Count];

            // Массив индексов предыдущих вершин
            int[] from = new int[adjacencyList.Count];

            // Записываем расстояние до стартовой вершины
            distances[startVertexIndex] = 0;

            // Записываем расстояние до всех остальных вершин int.MaxValue
            // и записываем для всех вершин индекс предыдущей вершины -1
            for (int i = 0; i < distances.Length; i++) {
                if (i != startVertexIndex) {
                    distances[i] = int.MaxValue;
                }

                from[i] = -1;
            }

            // Создаём очередь
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(startVertexIndex);

            // Пробегаем по всем элементам в текущей компоненте связности
            while (queue.Count != 0) {
                // Извлекаем текущую вершину из очереди
                int currentVertex = queue.Dequeue();

                // Пробегаем всех соседей текущей вершины
                List<int> neighbors = adjacencyList[currentVertex];
                for (int i = 0; i < neighbors.Count; i++) {

                    int neighborIndex = neighbors[i];

                    if (distances[neighborIndex] == int.MaxValue) {

                        // Записываем расстояние и индекс предыдущей вершины
                        distances[neighborIndex] = distances[currentVertex] + 1;
                        from[neighborIndex] = currentVertex;

                        queue.Enqueue(neighborIndex);
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
    }

    internal class Program {
        static void Main(string[] args) {
            Graph graph = new Graph("..//..//..//..//graph.txt");
            
            graph.DisplayShortestPaths(0);

            Console.ReadKey();
        }
    }
}