using System;
using System.Collections.Generic;
using System.Linq;
using ProcDungeon.Interfaces;
using ProcDungeon.Structures;

namespace ProcDungeon.Algorythms
{
    public class BSPDungeonAlgorythm : IGenerationAlgorythm
    {
        private Random _random = new Random();
        private List<Rectangle> _rooms = new List<Rectangle>();
        public List<Rectangle> Rooms => _rooms;

        public void Generate(DungeonGrid<Tile> grid, DungeonGraph graph)
        {
            var canvas = grid.Grid;
            BSPNode BSPTree = new BSPNode(0, canvas.GetLength(0), 0, canvas.GetLength(1));
            BSPTree.Partition(graph.NodeCount);

            var rects = new List<Rectangle>();

            var nodeLeafAssociation = new Dictionary<DNode, BSPNode>();

            // create entrance point/'s
            var start = new Point(canvas.GetLength(1) / 2, 0);
            var leaf = BSPTree.GetLeafeFromPoint(start);
            var node = graph.Nodes[0];
            var rect = CreateRectangleFromLeaf(leaf);

            grid.ClearArea(rect);
            rects.Add(rect);

            nodeLeafAssociation[node] = leaf;


            var leafQueue = new Queue<BSPNode>();
            var nodeQueue = new Queue<DNode>();
            var EdgeQueue = new Queue<DEdge>();

            nodeQueue.Enqueue(node);


            var processedLeaves = new List<BSPNode>();
            var processedNodes = new List<DNode>();
            processedLeaves.Add(leaf);
            processedNodes.Add(node);

            var emergencyBreak = 100;
            while(nodeQueue.Count > 0)
            {   
                emergencyBreak--;
                if(emergencyBreak <= 0) break;
                
                node = nodeQueue.Dequeue();

                foreach(DEdge e in node.Edges)
                {
                    // get list of neighbouring leaves whos 
                    // neighbour count matches or is larger than next node edge count
                    // closest match is first
                    var potentialLeaves = BSPTree.GetNeighbouringLeaves(leaf)
                        .Where(l => BSPTree.GetNeighbouringLeaves(l).Count >= e.NodeTo.Edges.Count)
                        .Where(l => !(processedLeaves.Contains(l)))
                        .OrderBy(l => BSPTree.GetNeighbouringLeaves(l).Count).ToList();
                    var l = potentialLeaves[0];
                    Rectangle r = CreateRectangleFromLeaf(l);
                    rects.Add(r);
                    grid.ClearArea(r);
                    processedLeaves.Add(l);
                    Join(nodeLeafAssociation[node], l);

                }

                // leaf = leafQueue.Dequeue();
                // node = nodeQueue.Dequeue();
                // rect = CreateRectangleFromLeaf(leaf);

                // place it on the grid
                grid.ClearArea(rect);

                rects.Add(rect);

                // Get neighbouring leaves from BSPTree
                var leafList = BSPTree.GetNeighbouringLeaves(leaf);
         
 
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
        }

        public void Join(BSPNode l1, BSPNode l2)
        {
            // the 3 points to join up
            Point overlap = BSPNode.GetLeafOverlapPoint(l1, l2);
            Point c1 = new Point(
                l1.RightEdge - (l1.Width / 2),
                l1.BottomEdge - (l1.Height / 2)
            );
            Point c2 = new Point(
                l2.RightEdge - (l2.Width / 2),
                l2.BottomEdge - (l2.Height / 2)
            );

            Rectangle rect1;
            Rectangle rect2;
            Rectangle rect3;
            Rectangle rect4;

            if (l1.RightEdge == l2.LeftEdge) // joined horizontally
            {
                rect1 = new Rectangle()
                {
                    x = c1.x,
                    y = c1.y,
                    width = 1,
                    height = overlap.y - c1.y
                };
                rect2 = new Rectangle()
                {
                    x = c1.x,
                    y = c1.y + rect1.height,
                    width = overlap.x - c1.x, 
                    height = 1
                };
                rect3 = new Rectangle()
                {
                    x = overlap.x,
                    y = overlap.y,
                    width = c2.x - overlap.x,
                    height = 1
                };
                rect4 = new Rectangle()
                {
                    x = overlap.x + rect3.width,
                    y = overlap.y,
                    width = 1,
                    height = c2.y - overlap.y
                };
            }
            else if (l1.LeftEdge == l2.RightEdge)
            {
                rect1 = new Rectangle()
                {
                    x = c1.x,
                    y = c1.y,
                    width = 1,
                    height = overlap.y - c1.y
                };
                rect2 = new Rectangle()
                {
                    x = overlap.x,
                    y = overlap.y,
                    width = c1.x - overlap.x,
                    height = 1,
                };
                rect3 = new Rectangle()
                {
                    x = c2.x,
                    y = overlap.y,
                    width = overlap.x - c2.x,
                    height = 1
                };
                rect4 = new Rectangle()
                {
                    x = overlap.x - c2.x,
                    y = overlap.y,
                    width = 1,
                    height = c2.y - overlap.y
                };
            }     
            else if (l1.BottomEdge == l2.BottomEdge)
            {
                rect1 = new Rectangle()
                {
                    x = c1.x,
                    y = c1.y,
                    width = overlap.x - c1.x,
                    height = 1
                };
                rect2 = new Rectangle()
                {
                   x = overlap.x,
                   y = c1.y,
                   width = 1,
                   height = overlap.y - c1.y
                };
                rect3 = new Rectangle()
                {
                   x = overlap.x,
                   y = overlap.y,
                   width = 1,
                   height = c2.y - overlap.y
                };
                rect4 = new Rectangle()
                {
                    x = overlap.x,
                    y = c2.y,
                    width = c2.x - overlap.x,
                    height = 1
                };
            }       
            else if (l1.TopEdge == l2.BottomEdge)
            {
                rect1 = new Rectangle()
                {
                    x = c1.x,
                    y = c1.y,
                    width = overlap.x - c1.x,
                    height = 1
                };
                rect2 = new Rectangle()
                {
                   x = overlap.x,
                   y = overlap.y,
                   width = 1,
                   height = c1.y - overlap.y
                };
                rect3 = new Rectangle()
                {
                   x = overlap.x,
                   y = c2.y,
                   width = 1,
                   height = overlap.y - c2.y
                };
                rect4 = new Rectangle()
                {
                    x = overlap.x,
                    y = c2.y,
                    width = c2.x - overlap.x,
                    height = 1
                };
            }
            throw new NotImplementedException();
        }

        private static Rectangle CreateRectangleFromLeaf(BSPNode l)
        {
            return new Rectangle()
            {
                x = l.LeftEdge + 1,
                y = l.TopEdge + 1,
                width = (l.RightEdge - l.LeftEdge) - 2,
                height = (l.BottomEdge - l.TopEdge) - 2
            };
        }
    }
}