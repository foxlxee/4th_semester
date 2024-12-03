using System;
using System.Collections.Generic;
using System.IO;

namespace lab_6 {
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

        // Конструктор, который принимает матрицу смежности
        public Graph(int[,] adjacencyMatrix) {
            // Записываем матрицу смежности
            this.adjacencyMatrix = adjacencyMatrix;

            // Вычисляем порядок матрицы
            adjacencyMatrixOrder = (int)Math.Sqrt(adjacencyMatrix.Length);

            // Выделяем память под список смежности
            adjacencyList = getAdjacencyList();

            // Выделяем память под список рёбер
            edgesList = getEdgesList();
        }

        // Конструктор, который принимает список рёбер
        public Graph(List<Edge> edges) {
            edgesList = edges;

            // Заполняем матрицу смежности
            adjacencyMatrixOrder = edgesList.Count + 1;
            adjacencyMatrix = new int[adjacencyMatrixOrder, adjacencyMatrixOrder];

            foreach (Edge edge in edgesList) {
                adjacencyMatrix[edge.Start, edge.End] =
                    adjacencyMatrix[edge.End, edge.Start] = edge.Weight;
            }

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

            // Выделяем память под список рёбер
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

        // Метод, который возвращает минимальное покрывающее дерево используя алгоритм Краскала
        public Graph GetMinimalSubgraph() {
            // Список соединённых вершин
            List<int> connectedVertices = new List<int>();

            // Словарь изолированных групп соединённых вершин
            Dictionary<int, List<int>> isolatedConnections = new Dictionary<int, List<int>>();

            // Список рёбер минимального остова
            List<Edge> minimalSubgraphEdges = new List<Edge>();

            // Первый этап работы алгоритма: находим изолированные группы вершин
            foreach (Edge edge in edgesList) {
                if (!connectedVertices.Contains(edge.Start) || !connectedVertices.Contains(edge.End)) {
                    if (!connectedVertices.Contains(edge.Start) && !connectedVertices.Contains(edge.End)) {
                        List<int> list = new List<int>() {
                            edge.Start, edge.End
                        };

                        isolatedConnections.Add(edge.Start, list);
                        isolatedConnections.Add(edge.End, list);

                    } else if (!isolatedConnections.ContainsKey(edge.Start)) {
                        isolatedConnections[edge.End].Add(edge.Start);
                        isolatedConnections.Add(edge.Start, isolatedConnections[edge.End]);
                    } else {
                        isolatedConnections[edge.Start].Add(edge.End);
                        isolatedConnections.Add(edge.End, isolatedConnections[edge.Start]);
                    }

                    minimalSubgraphEdges.Add(edge);
                    connectedVertices.Add(edge.Start);
                    connectedVertices.Add(edge.End);
                }
            }

            // Второй этап работы алгоритма: соединяем разные группы соединённых вершин
            foreach (Edge edge in edgesList) {
                List<int> list = isolatedConnections[edge.Start];

                if (list.Contains(edge.Start) && !list.Contains(edge.End)) {

                    minimalSubgraphEdges.Add(edge);

                    list.AddRange(isolatedConnections[edge.End]);
                    isolatedConnections[edge.End] = list;
                }
            }

            return new Graph(minimalSubgraphEdges);
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
            Console.WriteLine("Список смежности исходного взвешенного графа:");
            graph.DisplayAdjacencyList();

            Console.WriteLine("Список смежности минимального остовного дерева:");
            graph.GetMinimalSubgraph().DisplayAdjacencyList();

            Console.ReadKey();
        }
    }
}