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
        public DungeonGrid<Tile> Grid {get; }

        public BSPDungeonAlgorythm(DungeonGrid<Tile> g) => Grid = g;

        public void Generate(DungeonGraph graph)
        {
            var canvas = Grid.Grid;
            BSPNode BSPTree = new BSPNode(0, canvas.GetLength(0), 0, canvas.GetLength(1));
            BSPTree.Partition(graph.NodeCount);

            var rects = new List<Rectangle>();

            var nodeLeafAssociation = new Dictionary<DNode, BSPNode>();

            // create entrance point/'s
            var start = new Point(canvas.GetLength(1) / 2, 0);
            var leaf = BSPTree.GetLeafeFromPoint(start);
            var node = graph.Nodes[0];
            var rect = CreateRectangleFromLeaf(leaf);

            Grid.ClearArea(rect);
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

            foreach(BSPNode l in BSPTree.Leaves)
            {
                Rectangle r = CreateRectangleFromLeaf(l);
                rects.Add(r);
                Grid.ClearArea(r);
            }
            // while(nodeQueue.Count > 0)
            // {   
            //     emergencyBreak--;
            //     if(emergencyBreak <= 0) break;
                
            //     node = nodeQueue.Dequeue();

            //     foreach(DEdge e in node.Edges)
            //     {
            //         // get list of neighbouring leaves whos 
            //         // neighbour count matches or is larger than next node edge count
            //         // closest match is first
            //         var potentialLeaves = BSPTree.GetNeighbouringLeaves(leaf)
            //             .Where(l => BSPTree.GetNeighbouringLeaves(l).Count >= e.NodeTo.Edges.Count)
            //             .Where(l => !(processedLeaves.Contains(l)))
            //             .OrderBy(l => BSPTree.GetNeighbouringLeaves(l).Count).ToList();
            //         var l = potentialLeaves[0];
            //         Rectangle r = CreateRectangleFromLeaf(l);
            //         rects.Add(r);
            //         Grid.ClearArea(r);
            //         processedLeaves.Add(l);
            //         CreateCorridoor(nodeLeafAssociation[node], l);
            //         nodeLeafAssociation[e.NodeTo] = l;
            //         nodeQueue.Enqueue(e.NodeTo);

            //     }
            // }
            _rooms.AddRange(rects);
        }

        public void CreateCorridoor(BSPNode l1, BSPNode l2)
        {
            // start and end locations are the center of each rectangle
            Point startPoint = new Point(
                l1.RightEdge - (l1.Width / 2),
                l1.BottomEdge - (l1.Height / 2)
            );
            Point endPoint = new Point(
                l2.RightEdge - (l2.Width / 2),
                l2.BottomEdge - (l2.Height / 2)
            );

            // for the mid points we need to detrmine whether we are moving 
            // horizontally or vertically between rooms
            Point wayPoint1;
            Point wayPoint2;
            if (l1.BottomEdge == l2.TopEdge || l1.TopEdge == l2.BottomEdge)
            {
                // for vertical alignment we first move along x and then along y finishing on x
                int halfway = Math.Max(startPoint.x, endPoint.x) - Math.Min(startPoint.x, endPoint.x);
                wayPoint1 = new Point(halfway, startPoint.y);
                wayPoint2 = new Point(halfway, endPoint.y);
            }
            else
            {
                // for horizontal we need to move on the y first and then the x. finishing on y
                int halfway = Math.Max(startPoint.y, endPoint.y) - Math.Min(startPoint.y, endPoint.y);
                wayPoint1 = new Point(startPoint.x, halfway);
                wayPoint2 = new Point(endPoint.x, halfway);
            }

            var points = new List<Point>() { startPoint, wayPoint1, wayPoint2, endPoint };

            CreateCorridoor(points);
        }

        public void CreateCorridoor(List<Point> points)
        {
            // OrderedParallelQuery the points from smallest to largest
            var orderedPoints = points.OrderBy(p => p).ToList();

            // loop through points creating rect from one to the next
            for (int i = 1; i < orderedPoints.Count - 1; i++)
            {
                Point p = orderedPoints[i];
                Point np = orderedPoints[i + 1];

                int w;
                int h;
                if (p.x == np.x)
                {
                    w = 1; h = np.y - p.y;
                }
                else w = np.x - p.x; h = 1;

                var rect = new Rectangle()
                {
                    x = p.x,
                    y = p.y,
                    width = w,
                    height = h
                };
                Grid.ClearArea(rect);
            }
        }

        private static Rectangle CreateRectangleFromLeaf(BSPNode l)
        {
            return new Rectangle()
            {
                x = l.LeftEdge +1,
                y = l.TopEdge +1,
                width = (l.RightEdge - l.LeftEdge) - 2,
                height = (l.BottomEdge - l.TopEdge) - 2
            };
        }
    }
}