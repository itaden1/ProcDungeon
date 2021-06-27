using System.Collections.Generic;
using ProcDungeon.Structures;

namespace ProcDungeon.Interfaces
{
    public interface IGenerationAlgorythm
    {
        public List<Rectangle> Rooms { get; }
        public void Generate(DungeonGrid<Tile> canvas, DungeonGraph graph);
    }
}