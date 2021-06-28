using System.Collections.Generic;
using ProcDungeon.Structures;

namespace ProcDungeon.Interfaces
{
    public interface IGenerationAlgorythm
    {
        public List<Rectangle> Rooms { get; }
        public DungeonGrid<Tile> Grid {get; }
        public void Generate(DungeonGraph graph);
        public void CreateCorridoor(List<Point> points);
    }
}