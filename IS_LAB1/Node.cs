using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_LAB1
{
    internal class Node
    {
        private int g;
        private int h;
        private Node lastNode;
        private List<Node> nextNodes;
        private int[,] matrix;

        private bool swapped;


        public int G { get { return g; } set { g = value; } }
        public int H { get { return h; } set { h = value; } }
        public int[,] Matrix { get { return matrix; } set { matrix = value; } }
        public Node LastNode { get { return lastNode; } set { lastNode = value; } }
        public List<Node> NextNodes { get { return nextNodes; } set { nextNodes = value; } }
        public bool Swapped { get { return swapped; } set { swapped = value; } }

        public Node(int[,] matrix, int g, int h, Node lastNode = null)
        {
            this.LastNode = lastNode;
            this.Matrix = matrix;
            this.G = g;
            this.H = h;
        }
    }
}
