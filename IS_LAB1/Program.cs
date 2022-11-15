using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IS_LAB1
{
    internal class Program
    {
        public static int[,] startPos = new int[3, 3];
        public static int[,] endPos = new int[3, 3];

        //public static int[,] startPos = new int[,] { { 2, 1, 6 }, { 4, 0, 8 }, { 7, 5, 3 } };
        //public static int[,] endPos = new int[,] { { 1, 2, 3 }, { 8, 0, 4 }, { 7, 6, 5 } };

        public static List<Node> nodes = new List<Node>();
        static void Main(string[] args)
        {
            startPos = getMatrix("startPos.txt");
            endPos = getMatrix("endPos.txt");
            Node startNode = new Node(startPos, CheckG(startPos, endPos), 0);
            

            ShowMatrix(startNode.Matrix);
            Console.WriteLine("G = " + startNode.G);
            Console.WriteLine("---------------");
            
            Node solveMatrix = new Node(startPos, CheckG(startPos, endPos), -1);
            nodes.Add(solveMatrix);
            while (solveMatrix.G > 0)
            {
                solveMatrix = Swap(solveMatrix);
                foreach(Node node in nodes)
                {
                    if(node.Matrix == solveMatrix.Matrix)
                    {
                        node.Swapped = true;
                    }
                }
                for(int i = 0; i < solveMatrix.NextNodes.Count; i++)
                {
                    if(solveMatrix.NextNodes[i].G == 0)
                    {
                        solveMatrix = solveMatrix.NextNodes[i];
                        break;
                    }
                    else
                    {
                        nodes.Add(solveMatrix.NextNodes[i]);
                    }
                }
                if(solveMatrix.G > 0)
                {
                    int min = 1000;
                    int pos = 0;
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        int F = 3*nodes[i].G + nodes[i].H;
                        if (F <= min && nodes[i].Swapped == false)
                        {
                            min = F;
                        }
                    }
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        int F = 3*nodes[i].G + nodes[i].H;
                        if (F == min && nodes[i].Swapped == false)
                        {
                            pos = i;
                            break;
                        }
                    }
                    solveMatrix = nodes[pos];
                }
            }
            
            
            Console.Clear();
            Console.WriteLine("----------------");
            Console.WriteLine($"Общие затраты на поиск: {nodes.Count+1}");
            Console.WriteLine($"Количество оптимальных вершин: {getCountNodes(solveMatrix)}");
            Console.WriteLine($"Количество оптимальных перестановок: {solveMatrix.H+1}");
            double countNodes = getCountNodes(solveMatrix);
            double qpd = (countNodes / (double)(nodes.Count+1)) * 100;
            Console.WriteLine($"КПД: {Math.Round(qpd,2)}%");
            Console.WriteLine($"Траектория решения:");
            ShowSolvedMatrix(solveMatrix);
            Console.ReadKey();
        }

        public static void ShowSolvedMatrix(Node solveMatrix)
        {
            if(solveMatrix.LastNode != null)
            {
                ShowSolvedMatrix(solveMatrix.LastNode);
            }
            ShowMatrix(solveMatrix.Matrix);
            
            Console.WriteLine($"H = {solveMatrix.H}");
            Console.WriteLine("------------------");
        }

        public static int getCountNodes(Node solveMatrix)
        {
            if (solveMatrix.LastNode != null)
            {
                return getCountNodes(solveMatrix.LastNode)+1;  
            }
            else
            {
                return 1;
            }
            
        }

        public static void ShowMatrix(int[,] matrix)
        {
            for(int i = 0; i < matrix.GetLength(0); i++)
            {
                for(int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i,j] + " ");
                }
                Console.WriteLine();
            }
        }

        public static int CheckG(int[,] currentPos, int[,] endPos)
        {
            int g = 0;
            for (int i = 0; i < currentPos.GetLength(0); i++)
            {
                for (int j = 0; j < currentPos.GetLength(1); j++)
                {
                    if(currentPos[i,j] != endPos[i, j])
                    {
                        g++;
                    }
                }
            };
            return g;
        }
        public static Node Swap(Node currentPos)
        {
            int xPos = 0, yPos = 0;
            for (int i = 0; i < currentPos.Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < currentPos.Matrix.GetLength(1); j++)
                {
                    if (currentPos.Matrix[i, j] == 0)
                    {
                        xPos = j;
                        yPos = i;
                        break;
                    }
                }
            };
            if((xPos > 0 && xPos < currentPos.Matrix.GetLength(1)-1) && (yPos > 0 && yPos < currentPos.Matrix.GetLength(0)-1))
            {
                currentPos = Swapper(currentPos, yPos, xPos, true, true, true, true);
            }
            else if((xPos > 0 && xPos < currentPos.Matrix.GetLength(1)-1) && yPos == 0)
            {
                currentPos = Swapper(currentPos, yPos, xPos, true, true, false, true);
            }
            else if((xPos > 0 && xPos < currentPos.Matrix.GetLength(1)-1) && yPos == currentPos.Matrix.GetLength(0)-1)
            {
                currentPos = Swapper(currentPos, yPos, xPos, true, true, true, false);
            }
            else if(xPos == 0 && (yPos > 0 && yPos < currentPos.Matrix.GetLength(0)-1))
            {
                currentPos = Swapper(currentPos, yPos, xPos, true, false, true, true);
            }
            else if (xPos == currentPos.Matrix.GetLength(1)-1 && (yPos > 0 && yPos < currentPos.Matrix.GetLength(0)-1))
            {
                currentPos = Swapper(currentPos, yPos, xPos, false, true, true, true);
            }
            else if(xPos == currentPos.Matrix.GetLength(1) - 1 && yPos == 0)
            {
                currentPos = Swapper(currentPos, yPos, xPos, false, true, false, true);
            }
            else if (xPos == 0 && yPos == 0)
            {
                currentPos = Swapper(currentPos, yPos, xPos, true, false, false, true);
            }
            else if (xPos == currentPos.Matrix.GetLength(1) - 1 && yPos == currentPos.Matrix.GetLength(0) - 1)
            {
                currentPos = Swapper(currentPos, yPos, xPos, false, true, true, false);
            }
            else if (xPos == 0 && yPos == currentPos.Matrix.GetLength(0) - 1)
            {
                currentPos = Swapper(currentPos, yPos, xPos, true, false, true, false);
            }

            return currentPos;
        }
        public static Node Swapper(Node currentPos, int xPos, int yPos, bool right, bool left, bool up, bool down)
        {
            currentPos.NextNodes = new List<Node>();
            if (up)
            {
                int[,] tempMatrix = (int[,])currentPos.Matrix.Clone();
                int tempValue = tempMatrix[xPos - 1, yPos];
                tempMatrix[xPos - 1, yPos] = 0;
                tempMatrix[xPos, yPos] = tempValue;
                Node nextNode = new Node(tempMatrix, CheckG(tempMatrix, endPos), currentPos.H + 1, currentPos);

                int g = 1;
                for(int i = 0; i < nodes.Count; i++)
                {
                    g = CheckG(nextNode.Matrix, nodes[i].Matrix);
                    if (g == 0) { break; }
                }
                if(g != 0)
                {
                    currentPos.NextNodes.Add(nextNode);
                    ShowMatrix(nextNode.Matrix);
                    Console.WriteLine("G = " + nextNode.G);
                    Console.WriteLine("---------------");
                }
            }
            if (down)
            {
                int[,] tempMatrix = (int[,])currentPos.Matrix.Clone();
                int tempValue = tempMatrix[xPos + 1, yPos];
                tempMatrix[xPos + 1, yPos] = 0;
                tempMatrix[xPos, yPos] = tempValue;
                Node nextNode = new Node(tempMatrix, CheckG(tempMatrix, endPos), currentPos.H + 1, currentPos);

                int g = 1;
                for (int i = 0; i < nodes.Count; i++)
                {
                    g = CheckG(nextNode.Matrix, nodes[i].Matrix);
                    if (g == 0) { break; }
                }
                if (g != 0)
                {
                    currentPos.NextNodes.Add(nextNode);
                    ShowMatrix(nextNode.Matrix);
                    Console.WriteLine("G = " + nextNode.G);
                    Console.WriteLine("---------------");
                }
            }
            if (left)
            {
                int[,] tempMatrix = (int[,])currentPos.Matrix.Clone();
                int tempValue = tempMatrix[xPos, yPos - 1];
                tempMatrix[xPos, yPos - 1] = 0;
                tempMatrix[xPos, yPos] = tempValue;
                Node nextNode = new Node(tempMatrix, CheckG(tempMatrix, endPos), currentPos.H + 1, currentPos);

                int g = 1;
                for (int i = 0; i < nodes.Count; i++)
                {
                    g = CheckG(nextNode.Matrix, nodes[i].Matrix);
                    if (g == 0) { break; }
                }
                if (g != 0)
                {
                    currentPos.NextNodes.Add(nextNode);
                    ShowMatrix(nextNode.Matrix);
                    Console.WriteLine("G = " + nextNode.G);
                    Console.WriteLine("---------------");
                }
            }
            if (right)
            {
                int[,] tempMatrix = (int[,])currentPos.Matrix.Clone();
                int tempValue = tempMatrix[xPos, yPos + 1];
                tempMatrix[xPos, yPos + 1] = 0;
                tempMatrix[xPos, yPos] = tempValue;
                Node nextNode = new Node(tempMatrix, CheckG(tempMatrix, endPos), currentPos.H + 1, currentPos);

                int g = 1;
                for (int i = 0; i < nodes.Count; i++)
                {
                    g = CheckG(nextNode.Matrix, nodes[i].Matrix);
                    if (g == 0) { break; }
                }
                if (g != 0)
                {
                    currentPos.NextNodes.Add(nextNode);
                    ShowMatrix(nextNode.Matrix);
                    Console.WriteLine("G = " + nextNode.G);
                    Console.WriteLine("---------------");
                }
            }
            return currentPos;
        }

        public static int[,] getMatrix(string path)
        {
            int size = 3;
            string[] lines = File.ReadAllLines(path).Take(10).ToArray();

            int[,] arr = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                int[] row = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
                for (int j = 0; j < size; j++)
                {
                    arr[i, j] = row[j];
                }
            }
            return arr;
        }
    }
}
