using System.Collections.Generic;
using ProcDungeon.Structures;

namespace ProcDungeon.Interfaces
{
    public interface IGenerationAlgorythm
    {
        public List<Rectangle> Rooms { get; }
        public DungeonGrid<Tile> Grid {get; }
        public void Generate(int roomCount, List<int> exits);
        public void ConnectPoints(List<Point> points);
    }
}