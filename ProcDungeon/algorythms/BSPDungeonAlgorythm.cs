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
        private int _failThreshold = 100;
        private List<Rectangle> _rooms = new List<Rectangle>();
        public List<Rectangle> Rooms => _rooms;
        public DungeonGrid<Tile> Grid {get; }

        public BSPDungeonAlgorythm(DungeonGrid<Tile> g) => Grid = g;

        public void Generate(int roomCount, List<int> exits)
        {
            var canvas = Grid.Grid;
            BSPNode BSPTree = new BSPNode(0, canvas.GetLength(0), 0, canvas.GetLength(1));
            BSPTree.Partition(roomCount);

            var rects = new List<Rectangle>();

            foreach(BSPNode leaf in BSPTree.Leaves)
            {
                Rectangle rect = CreateRectangleFromLeaf(leaf);
                rects.Add(rect);
                Grid.ClearArea(rect);
            }
            Queue<BSPNode> bspQueue = new Queue<BSPNode>();
            bspQueue.Enqueue(BSPTree);
            while(bspQueue.Count > 0)
            {
                _failThreshold--;
                if (_failThreshold <= 0) break;

                var bspNode = bspQueue.Dequeue();
                if (bspNode.Leaves.Count <= 0) continue;
                
                JoinBSPLeaves(bspNode.Branch1, bspNode.Branch2);
                
                bspQueue.Enqueue(bspNode.Branch1);
                bspQueue.Enqueue(bspNode.Branch2);
            }

            _rooms.AddRange(rects);
        }

        private void JoinBSPLeaves(BSPNode branch1, BSPNode branch2)
        {
            List<BSPNode> validLeaves1 = branch1.GetNeighbouringLeaves(branch2);
            List<BSPNode> validLeaves2 = branch2.GetNeighbouringLeaves(branch1);
            var r1 = validLeaves1[_random.Next(0, validLeaves1.Count)];
            var r2 = validLeaves2[_random.Next(0, validLeaves2.Count)];
            CreateCorridoor(r1, r2);
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

        private Rectangle CreateRectangleFromLeaf(BSPNode leaf)
        {
            // TODO Create odd shaped rooms by combining rectangles
            var width = _random.Next((leaf.RightEdge - leaf.LeftEdge + 2)/3, leaf.RightEdge - leaf.LeftEdge - 2);
            var height = _random.Next((leaf.BottomEdge - leaf.TopEdge + 2)/3, leaf.BottomEdge - leaf.TopEdge - 2);
            return new Rectangle()
            {
                x = _random.Next(leaf.LeftEdge +1, leaf.RightEdge - width),
                y = _random.Next(leaf.TopEdge +1, leaf.BottomEdge - height),
                width = width,
                height = height
            };
        }
    }
}