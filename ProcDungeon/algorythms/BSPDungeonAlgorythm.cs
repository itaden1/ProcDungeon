using System;
using System.Collections.Generic;
using System.Linq;
using ProcDungeon.Interfaces;
using ProcDungeon.Structures;
using ProcDungeon.Enums;

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
            BSPNode BSPTree = new BSPNode(2, canvas.GetLength(0)-4, 2, canvas.GetLength(1)-4);
            BSPTree.Partition(roomCount);

            // add rooms to the grid
            foreach(BSPNode leaf in BSPTree.Leaves)
            {
                Rectangle rect = CreateRoomInsideLeaf(leaf);
                _rooms.Add(rect);
                Grid.ClearArea(rect, 1);
            }

            // cycle through the BSPTree joining a leaf from each 
            // child to a child of the neighbouring leaf
            Queue<BSPNode> bspQueue = new Queue<BSPNode>();
            bspQueue.Enqueue(BSPTree);
            while(bspQueue.Count > 0)
            {
                _failThreshold--;
                if (_failThreshold <= 0) break;

                var bspNode = bspQueue.Dequeue();
                if (!(bspNode.Branch1 is null || bspNode.Branch2 is null))
                {
                    JoinBSPLeaves(bspNode.Branch1, bspNode.Branch2);
                    
                    bspQueue.Enqueue(bspNode.Branch1);
                    bspQueue.Enqueue(bspNode.Branch2);
                }
            }
        }

        private void JoinBSPLeaves(BSPNode branch1, BSPNode branch2)
        {
            var align = Alignment.Horizontal;
            if (branch1.BottomEdge == branch2.TopEdge || branch1.TopEdge == branch2.BottomEdge)
            {
                align = Alignment.Vertical;
            }

            List<BSPNode> validLeaves1 = branch1.GetNeighbouringLeaves(branch2);
            List<BSPNode> validLeaves2 = branch2.GetNeighbouringLeaves(branch1);

            var l1 = validLeaves1[_random.Next(0, validLeaves1.Count)];
            var l2 = validLeaves2[_random.Next(0, validLeaves2.Count)];
            Rectangle room1 = null;
            Rectangle room2 = null;
            foreach(Rectangle r in Rooms)
            {
                if (r.OverlapsWith((Rectangle)l1)) room1 = r;
                if (r.OverlapsWith((Rectangle)l2)) room2 = r;
                if (!(room1 is null) && !(room2 is null))
                {
                    CreateCorridoor(room1, room2, align);
                    break;
                }
            }
        }

        public void CreateCorridoor(Rectangle r1, Rectangle r2, Alignment align)
        {
            // start and end locations are the center of each rectangle
            Point startPoint = r1.Center;
            Point endPoint = r2.Center;

            Point wayPoint1;
            Point wayPoint2;
            if (align == Alignment.Horizontal)
            // if (l1.BottomEdge == l2.TopEdge || l1.TopEdge == l2.BottomEdge)
            {
                // for vertical alignment we first move along x and then along y finishing on x
                int distance = Math.Max(startPoint.X, endPoint.X) - Math.Min(startPoint.X, endPoint.X);
                wayPoint1 = new Point(startPoint.X + distance/2, startPoint.Y);
                wayPoint2 = new Point(startPoint.X + distance/2, endPoint.Y);
            }
            else
            {
                // for horizontal we need to move on the y first and then the x. finishing on y
                int distance = Math.Max(startPoint.Y, endPoint.Y) - Math.Min(startPoint.Y, endPoint.Y);
                wayPoint1 = new Point(startPoint.X, startPoint.Y + distance/2);
                wayPoint2 = new Point(endPoint.X, startPoint.Y + distance/2);
            }

            var points = new List<Point>() { 
                startPoint, wayPoint1, wayPoint2, endPoint };
            ConnectPoints(points);
        }

        public void ConnectPoints(List<Point> points)
        {
            // OrderedParallelQuery the points from smallest to largest
            // var orderedPoints = points.OrderBy(p => p).ToList();

            // loop through points creating rect from one to the next
            for (int i = 0; i < points.Count; i++)
            {
                Point p = points[i];
                Point np = getNextPoint(p, points);
                Rectangle  corridoor = CreateHorizontalCorridoor(p, np);
                if (p.X == np.X)
                {
                    corridoor = CreateVerticalCorridoor(p, np);
                }
                // Grid.ClearArea(corridoor);
                Grid.ClearArea(corridoor, 2);
            }
        }

        private Rectangle CreateHorizontalCorridoor(Point p1, Point p2)
        {
            return new Rectangle(){
                X = Math.Min(p1.X, p2.X),
                Y = p1.Y,
                Width = Math.Abs(p1.X - p2.X) + 1,
                Height = 1
            };
        }

        private Rectangle CreateVerticalCorridoor(Point p1, Point p2)
        {
            return new Rectangle(){
                X = p1.X,
                Y = Math.Min(p1.Y, p2.Y),
                Width = 1,
                Height = Math.Abs(p1.Y - p2.Y) + 1
            };
        }

        private Point getNextPoint(Point p, List<Point> points)
        {
            IEnumerable<Point> pQuery = from _p in points 
                                where (!(_p == p)) &&
                                    _p.X == p.X || _p.Y == p.Y
                                select _p;
            return pQuery.First();
        }

        private Rectangle CreateRoomInsideLeaf(BSPNode leaf)
        {
            // TODO Create odd shaped rooms by combining rectangles

            // creat a random width ensuring that min is not greater than max
            int width = 2;
            var minWidth = (leaf.RightEdge - leaf.LeftEdge + 2) / 2;
            var maxWidth = leaf.RightEdge - leaf.LeftEdge - 2;
            if (!(minWidth >= maxWidth)) 
            {
                width = _random.Next(minWidth, maxWidth);
            }

            // creat a random height ensuring that min is not greater than max
            int height = 2;
            int minHeight = (leaf.BottomEdge - leaf.TopEdge + 2) / 2;
            int maxHeight =  leaf.BottomEdge - leaf.TopEdge - 2;
            if (!(minHeight >= maxHeight))
            {
                height = _random.Next(minHeight, maxHeight);
            }

            return new Rectangle()
            {
                X = _random.Next(leaf.LeftEdge +1, leaf.RightEdge - width),
                Y = _random.Next(leaf.TopEdge +1, leaf.BottomEdge - height),
                Width = width,
                Height = height
            };
        }
    }
}