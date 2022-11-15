using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_LAB1
{
    internal class Tree
    {
        private List<Node> nodes;

        public List<Node> Nodes { get { return nodes; } set { nodes = Nodes; } }

        public Tree()
        {
            nodes = new List<Node>();
        }
    }
}
