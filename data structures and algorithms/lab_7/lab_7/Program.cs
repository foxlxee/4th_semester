using System;
using System.Collections.Generic;
using System.IO;

namespace lab_7 {
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

            public bool Contains(int vertex) {
                return Start == vertex || End == vertex;
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

        // Метод, который возвращает минимальное покрывающее дерево используя алгоритм Прима
        public Graph GetMinimalSubgraph() {
            // Список рёбер минимального остова
            List<Edge> minimalSubgraphEdges = new List<Edge>();

            // Список вершин минимального остова
            List<int> minimalSubgraphVertices = new List<int>();

            // Функция, возвращающая ребро с минимальным весом, не образующее цикл в множестве вершин остова
            Edge getMinimalEdge() {
                Edge result = null;

                // Пробегаем все вершины в текущем остове
                foreach (int vertex in minimalSubgraphVertices) {

                    // Пробегаем все рёбра в графе
                    foreach (Edge edge in edgesList) {

                        // Проверяем что ребро содержит вершину
                        // и не образует цикл и имеет минимальный вес
                        if (edge.Contains(vertex) &&
                            (!minimalSubgraphVertices.Contains(edge.Start) || !minimalSubgraphVertices.Contains(edge.End))) {

                            if (result == null) {
                                result = edge;
                                continue;
                            }

                            if (edge.Weight < result.Weight) {
                                result = edge;
                            }
                        }
                    }
                }

                return result;
            }

            // Начинаем с вершины 0
            minimalSubgraphVertices.Add(0);

            // Добавляем рёбра пока не обработаем все вершины
            while (minimalSubgraphVertices.Count < adjacencyMatrixOrder) {
                Edge edge = getMinimalEdge();

                if (edge == null) break;

                minimalSubgraphEdges.Add(edge);

                if (!minimalSubgraphVertices.Contains(edge.Start)) minimalSubgraphVertices.Add(edge.Start);
                if (!minimalSubgraphVertices.Contains(edge.End)) minimalSubgraphVertices.Add(edge.End);
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