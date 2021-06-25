using System;
using System.Collections.Generic;
using System.Linq;
using ProcDungeon.Interfaces;
using ProcDungeon.Structures;

namespace ProcDungeon.Algorythms
{
    public class BSPDungeonAlgorythm<T> : IGenerationAlgorythm<T>
    {
        private Random _random = new Random();
        private List<Rectangle> _rooms = new List<Rectangle>();
        public List<Rectangle> Rooms => _rooms;

        public T[,] Generate<T>(T[,] canvas, DungeonGraph graph)
            where T : ITileNode
        {
            BSPNode BSPTree = new BSPNode(0, canvas.GetLength(0), 0, canvas.GetLength(1));
            BSPTree.Partition(graph.NodeCount);
            var rects = new List<Rectangle>();

            var start = new Point(canvas.GetLength(1) / 2, 0);

            // Select an edge node from the BSPTree
            // TODO: first node is always on the top... change this to be dynamic
            List<BSPNode> leafNodes = BSPTree.Leaves;
            IEnumerable<BSPNode> query = from n in leafNodes
                                         where n.TopEdge == 0
                                         where n.LeftEdge <= start.x
                                         where n.RightEdge >= start.x
                                         select n;
            var firstLeaf = query.First();

            var leafQueue = new Queue<BSPNode>();
            var nodeQueue = new Queue<DNode>();
            leafQueue.Enqueue(firstLeaf);
            nodeQueue.Enqueue(graph.Nodes[0]);

            var processedLeaves = new List<BSPNode>();
            var processedNodes = new List<DNode>();

            var emergencyBreak = 100;
            while(nodeQueue.Count > 0)
            {   
                emergencyBreak--;
                if(emergencyBreak <= 0) break;

                var leaf = leafQueue.Dequeue();
                var node = nodeQueue.Dequeue();
                var rect = new Rectangle()
                {
                    x = leaf.LeftEdge + 1,
                    y = leaf.TopEdge + 1,
                    width = (leaf.RightEdge - leaf.LeftEdge) - 2,
                    height = (leaf.BottomEdge - leaf.TopEdge) - 2
                };

                // place it on the grid
                for (int y = rect.y; y < rect.y + rect.height; y++)
                {
                    for (int x = rect.x; x < rect.x + rect.width; x++)
                    {
                        canvas[y, x].Blocking = false;
                    }
                }
                // if (rects.Count == graph.NodeCount) break;
                rects.Add(rect);

                // Get neighbouring leaves from BSPTree
                IEnumerable<BSPNode> leafQuery = 
                    from ln in leafNodes
                    where ln.RightEdge == leaf.LeftEdge
                          || ln.TopEdge == leaf.BottomEdge
                          || ln.LeftEdge == leaf.RightEdge
                          || ln.BottomEdge == leaf.TopEdge
                    select ln;

                var leafList = leafQuery.ToList();
         
 
                foreach(DEdge e in node.Edges)
                {
           
                    leafQueue.Enqueue(leafList[0]);
                    leafList.RemoveAt(0);
                
                    Console.WriteLine($"node({node}) has {node.Edges.Count} edges");
                    Console.WriteLine($"{e.NodeFrom}--->{e.NodeTo}");
                    if (e.NodeFrom == node && !(processedNodes.Contains(e.NodeTo)))
                    {
                        Console.WriteLine($"queuing {e.NodeTo}");
                        nodeQueue.Enqueue(e.NodeTo);
                    }
                    else if (!(processedNodes.Contains(e.NodeFrom)))
                    {
                        Console.WriteLine($"queuing {e.NodeFrom}");
                        nodeQueue.Enqueue(e.NodeFrom);
                    }
         
                }
                processedLeaves.Add(leaf);
                processedNodes.Add(node);
            }
            _rooms.AddRange(rects);
            return canvas;
        }
    }
}