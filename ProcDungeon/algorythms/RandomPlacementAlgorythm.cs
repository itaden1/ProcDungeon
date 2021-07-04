using System;
using System.Collections.Generic;
using ProcDungeon.Interfaces;
using ProcDungeon.Structures;

namespace ProcDungeon.Algorythms
{
    public class RandomPlacementAlgorythm : IGenerationAlgorythm
    {
        private int _failThreshold = 100;
        public int FailThreshold { get; set; }
        private Random _random = new Random();
        private List<Rectangle> _rooms = new List<Rectangle>();

        public List<Rectangle> Rooms  => _rooms;

        public DungeonGrid<Tile> Grid {get; }
        public RandomPlacementAlgorythm(DungeonGrid<Tile> g) => Grid = g;

        public void ConnectPoints(List<Point> points)
        {
            throw new NotImplementedException();
        }

        public void Generate(int roomCount, List<int> exits)
        {
            var rects = new List<Rectangle>();
            Rectangle rect;

            var minSize = Grid.Grid.GetLength(0) / 4;
            var maxSize = Grid.Grid.GetLength(0) / 2;

            while (true)
            {
                _failThreshold --;
                if (_failThreshold <=0) break;

                int w = _random.Next(minSize, maxSize);
                int h = _random.Next(minSize, maxSize);
                var p = new Point(_random.Next(1, Grid.Grid.GetLength(0) - w), _random.Next(1, Grid.Grid.GetLength(1) - h));

                // Create a rect
                rect = new Rectangle(){X = p.X, Y = p.Y, Width = w, Height = h};

                // make sure its inside bounds and does not overlap with previouse rects
                bool validPlacement = true;
                if (rects.Count > 0)
                {
                    foreach(Rectangle r in rects)
                    {
                        if (rect.OverlapsWith(r)) validPlacement = false;
                    }
                }
                if (validPlacement)
                {
                    Grid.ClearArea(rect, 1);
                    rects.Add(rect);
                }
                else continue;
            }  
            _rooms.AddRange(rects);
        }
    }
}