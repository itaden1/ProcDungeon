using System;
using System.Collections.Generic;
using ProcDungeon.Interfaces;
using ProcDungeon.Structures;
using ProcDungeon.Enums;

namespace ProcDungeon.Algorythms
{
    public class AdjacentRooms
    {
        private Rectangle _room1;
        private Rectangle _room2;
        private Alignment _alignment;

        public Alignment Alignment { get => _alignment; set => _alignment = value; }
        public Rectangle Room1 { get => _room1; set => _room1 = value; }
        public Rectangle Room2 { get => _room2; set => _room2 = value; }

        public AdjacentRooms(Rectangle room1, Rectangle room2, Alignment alignment)
        {
            Room1 = room1;
            Room2 = room2;
            Alignment = alignment;
        }
    }
    public class BSPDungeonAlgorythm : BaseAlgorythm, IGenerationAlgorythm
    {
        private int _failThreshold = 100;

        public BSPDungeonAlgorythm(int roomCount) => _roomCount = roomCount;

        public void Generate(DungeonGrid canvas, List<int> exits)
        {
            // var canvas = Grid.Grid;
            // TODO make this configurable
            BSPNode BSPTree = new BSPNode(2, canvas.Grid.GetLength(0)-2, 2, canvas.Grid.GetLength(1)-2);
            BSPTree.Partition(_roomCount);

            // add rooms to the grid
            foreach(BSPNode leaf in BSPTree.Leaves)
            {
                Rectangle rect = CreateRoomInsideLeaf(leaf);
                _rooms.Add(rect);
                canvas.ClearArea(rect, 1);
            }

            // // cycle through the BSPTree joining a leaf from each 
            // // child to a child of the neighbouring leaf
            Queue<BSPNode> bspQueue = new Queue<BSPNode>();
            bspQueue.Enqueue(BSPTree);
            while(bspQueue.Count > 0)
            {
                _failThreshold--;
                if (_failThreshold <= 0) break;

                var bspNode = bspQueue.Dequeue();
                if (!(bspNode.Branch1 is null || bspNode.Branch2 is null))
                {

                    // // we want to work with rects (rooms) not leaves                    
                    AdjacentRooms rooms = GetRoomsFromLeaves(bspNode.Branch1, bspNode.Branch2);
         
                    List<Point> wayPoints = GetWayPoints(rooms.Room1, rooms.Room2, rooms.Alignment);
                    List<Rectangle> corridoors = ConnectWayPoints(wayPoints);
                    foreach (Rectangle c in corridoors)
                    {
                        canvas.ClearArea(c, 2);
                    }

                    bspQueue.Enqueue(bspNode.Branch1);
                    bspQueue.Enqueue(bspNode.Branch2);
                    
                }
            }
        }

        private AdjacentRooms GetRoomsFromLeaves(BSPNode branch1, BSPNode branch2)
        {
            // make sure we are aware of whether the rooms are top to bottom or left to right
            var align = Alignment.Horizontal;
            if (branch1.BottomEdge == branch2.TopEdge || branch1.TopEdge == branch2.BottomEdge)
            {
                align = Alignment.Vertical;
            }

            // Get all possible leaves that we may wish to connect
            List<BSPNode> validLeaves1 = branch1.GetNeighbouringLeaves(branch2);
            List<BSPNode> validLeaves2 = branch2.GetNeighbouringLeaves(branch1);

            // Chose 2 random leaves to connect
            var l1 = validLeaves1[_random.Next(0, validLeaves1.Count)];
            var l2 = validLeaves2[_random.Next(0, validLeaves2.Count)];

            // Get the room contained in each leaf
            Rectangle room1 = (Rectangle)l1;
            Rectangle room2 = (Rectangle)l2;
            foreach(Rectangle r in Rooms)
            {
                if (r.OverlapsWith((Rectangle)l1)) room1 = r;
                if (r.OverlapsWith((Rectangle)l2)) room2 = r;
            }

            return new AdjacentRooms(room1, room2, align);
        }

        public List<Point> GetWayPoints(Rectangle r1, Rectangle r2, Alignment align)
        {
            // start and end locations are the center of each rectangle
            Point startPoint = r1.Center;
            Point endPoint = r2.Center;

            Point wayPoint1;
            Point wayPoint2;
            if (align == Alignment.Horizontal)
            {
                // for vertical alignment we first move along x and then along y finishing on x
                int distanceX = Math.Max(startPoint.X, endPoint.X) - Math.Min(startPoint.X, endPoint.X);
                wayPoint1 = new Point(startPoint.X + distanceX / 2, startPoint.Y);
                wayPoint2 = new Point(startPoint.X + distanceX / 2, endPoint.Y);
            }
            else
            {
                // for horizontal we need to move on the y first and then the x. finishing on y
                int distanceY = Math.Max(startPoint.Y, endPoint.Y) - Math.Min(startPoint.Y, endPoint.Y);
                wayPoint1 = new Point(startPoint.X, startPoint.Y + distanceY / 2);
                wayPoint2 = new Point(endPoint.X, startPoint.Y + distanceY / 2);
            }

            return new List<Point>() { 
                startPoint, wayPoint1, wayPoint2, endPoint };
        }

        public List<Rectangle> ConnectWayPoints(List<Point> points)
        {

            // loop through points creating rect from one to the next
            List<Rectangle> corridoors = new List<Rectangle>();
            for (int i = 0; i < points.Count; i++)
            {
                Point p = points[i];
                Point np = getNextPoint(p, points);
                Rectangle  corridoor = CreateHorizontalCorridoor(p, np);
                if (p.X == np.X)
                {
                    corridoor = CreateVerticalCorridoor(p, np);
                }
                corridoors.Add(corridoor);
            }
            return corridoors;
        }

        private Rectangle CreateRoomInsideLeaf(BSPNode leaf)
        {
            // TODO Create odd shaped rooms by combining rectangles

            // creat a random width ensuring that min is not greater than max
            int width = 2;
            var minWidth = (leaf.RightEdge - leaf.LeftEdge + 2) / 2;
            var maxWidth = leaf.RightEdge - leaf.LeftEdge - 2;
            if (minWidth < maxWidth)
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

            int x = leaf.LeftEdge + 1;
            int y = leaf.TopEdge + 1;
            if (x < leaf.RightEdge - width) x = _random.Next(x, leaf.RightEdge - width);
            if (y < leaf.TopEdge - height) y = _random.Next(y, leaf.BottomEdge - height);
            return new Rectangle()
            {
                X = x,
                Y = y,
                Width = width,
                Height = height
            };
        }
    }
}