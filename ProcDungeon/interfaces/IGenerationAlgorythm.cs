using System.Collections.Generic;
using ProcDungeon.Enums;
using ProcDungeon.Structures;

namespace ProcDungeon.Interfaces
{
    public interface IGenerationAlgorythm
    {
        public List<Rectangle> Rooms { get; }
        // public DungeonGrid<Tile> Grid {get; }
        public void Generate(DungeonGrid canvas, List<int> exits);
        public List<Rectangle> ConnectWayPoints(List<Point> points);
        List<Point> GetWayPoints(Rectangle r1, Rectangle r2, Alignment align);
    }
}