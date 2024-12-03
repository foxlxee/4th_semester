using System;
using System.Collections.Generic;
using System.IO;

namespace lab_4 {
    // Класс, который представляет граф
    internal class Graph {

        // Класс, который представляет компоненту связности
        internal class ConnectedComponent {

            // Массив индексов вершин
            public readonly int[] Vertices;

            public ConnectedComponent(int[] vertices) {
                Vertices = vertices;
            }
        }

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

        // Метод, который возвращает компоненты связности с помощью поиска в глубину (DFS)
        public ConnectedComponent[] findConnectedComponents() {
            bool[] visited = new bool[adjacencyMatrixOrder];

            List<ConnectedComponent> result = new List<ConnectedComponent>();

            for (int i = 0; i < adjacencyMatrixOrder; i++) {
                if (!visited[i]) {
                    List<int> elements = new List<int>();

                    dfs(i, visited, elements);

                    result.Add(new ConnectedComponent(elements.ToArray()));
                }
            }

            return result.ToArray();
        }

        void dfs(int vertexIndex, bool[] visited, List<int> elements) {
            visited[vertexIndex] = true;

            if (!elements.Contains(vertexIndex)) elements.Add(vertexIndex);

            List<int> list = adjacencyList[vertexIndex];

            for (int i = 0; i < list.Count; i++) {
                int vertexToProcess = list[i];

                if (!visited[vertexToProcess]) {
                    elements.Add(vertexToProcess);
                    dfs(vertexToProcess, visited, elements);
                }
            }
        }
    }

    internal class Program {
        static void Main(string[] args) {
            Graph graph = new Graph("..//..//..//..//graph.txt");

            graph.DisplayAdjacencyMatrix();
            graph.DisplayAdjacencyList();

            Graph.ConnectedComponent[] connectedComponents = graph.findConnectedComponents();

            Console.WriteLine("Количество компонент связности: " + connectedComponents.Length.ToString());
            Console.WriteLine();

            for (int i = 0; i < connectedComponents.Length; i++) {
                Console.Write("Состав компоненты связности #" + (i + 1).ToString() + '\t');

                int[] vertices = connectedComponents[i].Vertices;

                for (int j = 0; j < vertices.Length; j++) {
                    Console.Write(vertices[j]);

                    if (j != vertices.Length - 1) Console.Write(','); else Console.WriteLine();
                }
            }

            Console.ReadKey();
        }
    }
}