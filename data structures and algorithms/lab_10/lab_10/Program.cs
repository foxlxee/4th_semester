using System;
using System.IO;
using System.Collections.Generic;

namespace lab_10 {
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

        // Метод для проверки, содержит ли граф Эйлеров цикл
        bool hasEulerianCycle() {
            foreach (var neighbors in adjacencyList) {
                if (neighbors.Count % 2 != 0) // Вершина с нечетной степенью
                    return false;
            }

            return isConnected();
        }

        // Метод для проверки связности графа
        bool isConnected() {
            bool[] visited = new bool[adjacencyMatrixOrder];
            int startVertex = -1;

            // Находим начальную вершину
            for (int i = 0; i < adjacencyMatrixOrder; i++) {
                if (adjacencyList[i].Count > 0) {
                    startVertex = i;
                    break;
                }
            }

            if (startVertex == -1) return true; // Нет ребер, граф считается связным

            DFS(startVertex, visited);

            // Проверка, что все вершины с ребрами посещены
            for (int i = 0; i < adjacencyMatrixOrder; i++) {
                if (adjacencyList[i].Count > 0 && !visited[i])
                    return false;
            }

            return true;
        }

        // Поиск в глубину (DFS) для проверки связности
        void DFS(int vertex, bool[] visited) {
            visited[vertex] = true;
            foreach (int neighbor in adjacencyList[vertex]) {
                if (!visited[neighbor]) {
                    DFS(neighbor, visited);
                }
            }
        }

        // Метод для поиска Эйлерова цикла
        public List<int> FindEulerianCycle() {
            if (!hasEulerianCycle()) return null;

            // Используем стек для поиска Эйлерова цикла
            Stack<int> stack = new Stack<int>();
            List<int> eulerCycle = new List<int>();
            int current = 0;
            stack.Push(current);

            while (stack.Count > 0) {
                if (adjacencyList[current].Count == 0) {
                    eulerCycle.Add(current);
                    current = stack.Pop();
                } else {
                    stack.Push(current);
                    int neighbor = adjacencyList[current][0];
                    adjacencyList[current].Remove(neighbor);
                    adjacencyList[neighbor].Remove(current);
                    current = neighbor;
                }
            }

            return eulerCycle;
        }
    }

    internal class Program {
        static void Main(string[] args) {
            Graph graph = new Graph("..//..//..//..//graph.txt");

            graph.DisplayAdjacencyMatrix();
            graph.DisplayAdjacencyList();

            List<int> list = graph.FindEulerianCycle();

            if (list == null) {
                Console.WriteLine("Граф не содержит Эйлеров цикл");
            } else {
                Console.WriteLine("Граф содержит Эйлеров цикл, который проходит через вершины:");

                for (int i = 0; i < list.Count; i++) {
                    Console.Write(list[i]);
                    if (i == list.Count - 1) Console.WriteLine();
                    else Console.Write(", ");
                }
            }

            Console.ReadKey();
        }
    }
}