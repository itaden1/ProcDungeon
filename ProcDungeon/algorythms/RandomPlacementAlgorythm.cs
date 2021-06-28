using System;
using System.Collections.Generic;
using ProcDungeon.Interfaces;
using ProcDungeon.Structures;

namespace ProcDungeon.Algorythms
{
    public class RandomPlacementAlgorythm : IGenerationAlgorythm
    {
        private int _faileThreshold = 100;
        private Random _random = new Random();
        private List<Rectangle> _rooms = new List<Rectangle>();

        public List<Rectangle> Rooms  => _rooms;

        public DungeonGrid<Tile> Grid {get; }
        public RandomPlacementAlgorythm(DungeonGrid<Tile> g) => Grid = g;

        public void CreateCorridoor(List<Point> points)
        {
            throw new NotImplementedException();
        }

        public void Generate(DungeonGraph graph)
        {
            var rects = new List<Rectangle>();
			foreach (DNode node in graph.Nodes)
            {
                Rectangle rect;
                while (true)
                {
                    _faileThreshold --;
                    if (_faileThreshold <=0) 
                    {
                        throw new Exception("dungeon generation failed due to infinite loop");
                    }
                    var p = new Point(_random.Next(1, Grid.Grid.GetLength(0) - 6), _random.Next(1, Grid.Grid.GetLength(1) - 6));
                    int w = _random.Next(2, 6);
                    int h = _random.Next(2, 6);
                    
                    // Create a rect
                    rect = new Rectangle(){x = p.x, y = p.y, width = w, height = h};

                    // make sure its inside bounds and does not overlap with previouse rects
                    bool validPlacement = true;
                    if (rects.Count > 0)
                    {
                        foreach(Rectangle r in rects)
                        {
                            if (rect.OverlapsWith(r)) validPlacement = false;
                        }
                    }
                    if (validPlacement) break;
                    else continue;
                }
        
                // place it on the grid
                for (int y = rect.y; y < rect.y + rect.height; y++)
                {
                    for (int x = rect.x; x < rect.x + rect.width; x++)
                    {
                        Grid.Grid[y,x].Blocking = false;
                    }
                }
                rects.Add(rect);
            }
            _rooms.AddRange(rects);
        }
    }
}