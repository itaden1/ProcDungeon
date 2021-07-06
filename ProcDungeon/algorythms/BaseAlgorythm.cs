using System;
using System.Collections.Generic;
using ProcDungeon.Structures;

namespace ProcDungeon.Algorythms
{    
    public class BaseAlgorythm
    {
        protected Random _random = new Random();
        protected int _roomCount;
        protected List<Rectangle> _rooms = new List<Rectangle>();
        public List<Rectangle> Rooms => _rooms;

        protected Rectangle CreateHorizontalCorridoor(Point p1, Point p2)
        {
            return new Rectangle()
            {
                X = Math.Min(p1.X, p2.X),
                Y = p1.Y,
                Width = Math.Abs(p1.X - p2.X) + 1,
                Height = 1
            };
        }

        protected Rectangle CreateVerticalCorridoor(Point p1, Point p2)
        {
            return new Rectangle(){
                X = p1.X,
                Y = Math.Min(p1.Y, p2.Y),
                Width = 1,
                Height = Math.Abs(p1.Y - p2.Y) + 1
            };
        }

        protected Point getNextPoint(Point point, List<Point> points)
        {
            // retrieve a Point from the list where 1 axis is the same as supplied Point
            
            foreach(Point p in points)
            {
                if (p == point) continue;
                if (p.X == point.X || p.Y == point.Y)
                {
                    return p;
                }
            }
            return null;
        }
    }
}