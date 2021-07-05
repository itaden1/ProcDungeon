using System;
using System.Collections.Generic;
using System.Linq;
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

        protected Point getNextPoint(Point p, List<Point> points)
        {
            // retrieve a Point from the list where 1 axis is the same as supplied Point
            IEnumerable<Point> pQuery = from _p in points 
                                where (!(_p == p)) &&
                                    _p.X == p.X || _p.Y == p.Y
                                select _p;
            return pQuery.First();
        }

        protected void ClearArea(BaseTile[,] grid, Rectangle rect, byte symbol)
        {
			for (int y = rect.Y; y < rect.Y + rect.Height; y++)
			{
				for (int x = rect.X; x < rect.X + rect.Width; x++)
				{
					grid[y, x].Blocking = false;
					grid[y, x].BitSymbol = symbol;
				}
			}
        }
    }
}