using System.Collections.Generic;
using System.Text;

namespace ProcDungeon.Structures
{
    public class DungeonGraph
    {
        private List<DNode> _nodes = new List<DNode>();
        private List<DEdge> _edges = new List<DEdge>();

        public int NodeCount => _nodes.Count;

        public List<DNode> Nodes => _nodes;

        public void AddNode(DNode n)
        {
            _nodes.Add(n);
            foreach(DEdge e in n.Edges)
            {
                _edges.Add(e);
            }
        }

        public void AddNodes(List<DNode> nodes)
        {
            _nodes.AddRange(nodes);
        }

        public Dictionary<int, List<int>> AsSimpleDictionary()
        {
            var dict = new Dictionary<int, List<int>>();
            foreach(DNode n in  _nodes)
            {
                dict[n.ID] = new List<int>();
                foreach(DEdge e in n.Edges)
                {
                    dict[n.ID].Add(e.NodeTo.ID);
                }
            }
            return dict;
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            foreach (DEdge e in _edges)
            {
                str.Append($"{e.ToString()}\n");
            }
            return str.ToString();
        }
    }

    public class DNode
    {
        public int ID;
        private List<DEdge> _edges = new List<DEdge>();
        public List<DEdge> Edges {get => _edges; }
        public DNode(int id) => this.ID = id;

        public void AddEdge(DEdge e) => _edges.Add(e);

        public bool HasEdgeToNode(DNode node)
        {
            foreach(DEdge e in Edges)
            {
                if (e.NodeTo.ID == node.ID) return true;
            }
            return false;
        }
        public override string ToString() => $"({ID})";
    }

    public class DEdge
    {
        public DNode NodeFrom;
        public DNode NodeTo;
        public DEdge(DNode n1, DNode n2)
        {
            this.NodeFrom = n1;
            this.NodeTo = n2;
        }
        public override string ToString() => $"{NodeFrom} --> {NodeTo}";
    }
}