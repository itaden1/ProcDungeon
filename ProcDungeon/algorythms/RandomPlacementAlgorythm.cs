using System;
using System.Collections.Generic;
using ProcDungeon.Enums;
using ProcDungeon.Interfaces;
using ProcDungeon.Structures;

namespace ProcDungeon.Algorythms
{
    public class RandomPlacementAlgorythm : BaseAlgorythm, IGenerationAlgorythm
    {
        private int _failThreshold = 100;
        public int FailThreshold { get; set; }

        public RandomPlacementAlgorythm(int roomCount) => _roomCount = roomCount;

        public void ConnectWayPoints(List<Point> points)
        {
            throw new NotImplementedException();
        }

        public void Generate(DungeonGrid canvas, List<int> exits)
        {
            var rects = new List<Rectangle>();
            Rectangle rect;

            var minSize = canvas.Grid.GetLength(0) / 4;
            var maxSize = canvas.Grid.GetLength(0) / 2;

            while (true)
            {
                _failThreshold --;
                if (_failThreshold <=0) break;

                int w = _random.Next(minSize, maxSize);
                int h = _random.Next(minSize, maxSize);
                var p = new Point(_random.Next(1, canvas.Grid.GetLength(0) - w), _random.Next(1, canvas.Grid.GetLength(1) - h));

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
                    canvas.ClearArea(rect, 1);
                    rects.Add(rect);
                }
                else continue;
            }  
            _rooms.AddRange(rects);
        }

        public List<Point> GetWayPoints(Rectangle r1, Rectangle r2, Alignment align)
        {
            throw new NotImplementedException();
        }

        List<Rectangle> IGenerationAlgorythm.ConnectWayPoints(List<Point> points)
        {
            throw new NotImplementedException();
        }
    }
}