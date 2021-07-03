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
            BSPNode BSPTree = new BSPNode(0, canvas.GetLength(0), 0, canvas.GetLength(1));
            BSPTree.Partition(roomCount);

            // add rooms to the grid
            foreach(BSPNode leaf in BSPTree.Leaves)
            {
                Rectangle rect = CreateRoomInsideLeaf(leaf);
                _rooms.Add(rect);
                Grid.ClearArea(rect);
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
            Point startPoint = new Point(
                r1.ex - r1.width / 2,
                r1.ey - r1.height / 2
            );
            Point endPoint = new Point(
                r2.ex - r2.width / 2,
                r2.ey - r2.height / 2
            );

            // int midX = startPoint.x + Math.Abs(endPoint.x - startPoint.x) / 2;
            // int midY = startPoint.y + Math.Abs(endPoint.x - startPoint.x) / 2;
            // Point wayPoint1 = new Point(midX, startPoint.y);
            // Point wayPoint2 = new Point(midX,endPoint.y);
            // if (align == Alignment.Vertical)
            // {
            //     wayPoint1 = new Point(startPoint.x, midY);
            //     wayPoint2 = new Point(endPoint.x, midY);
            // }
          

            // // for the mid points we need to detrmine whether we are moving 
            // // horizontally or vertically between rooms
            Point wayPoint1;
            Point wayPoint2;
            if (align == Alignment.Vertical)
            // if (l1.BottomEdge == l2.TopEdge || l1.TopEdge == l2.BottomEdge)
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

            var points = new List<Point>() { 
                startPoint, wayPoint1, wayPoint2, endPoint };
            CreateCorridoor(points);
        }

        public void CreateCorridoor(List<Point> points)
        {
            // OrderedParallelQuery the points from smallest to largest
            var orderedPoints = points.OrderBy(p => p).ToList();

            // loop through points creating rect from one to the next
            for (int i = 0; i < orderedPoints.Count - 1; i++)
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

        private Rectangle CreateRoomInsideLeaf(BSPNode leaf)
        {
            // TODO Create odd shaped rooms by combining rectangles

            // creat a random width ensuring that min is not greater than max
            int width = 2;
            var minWidth = (leaf.RightEdge - leaf.LeftEdge + 2) / 3;
            var maxWidth = leaf.RightEdge - leaf.LeftEdge - 2;
            if (!(minWidth >= maxWidth)) 
            {
                width = _random.Next(minWidth, maxWidth);
            }

            // creat a random height ensuring that min is not greater than max
            int height = 2;
            int minHeight = (leaf.BottomEdge - leaf.TopEdge + 2) / 3;
            int maxHeight =  leaf.BottomEdge - leaf.TopEdge - 2;
            if (!(minHeight >= maxHeight))
            {
                height = _random.Next(minHeight, maxHeight);
            }

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