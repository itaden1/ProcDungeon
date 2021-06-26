using System.Collections.Generic;
using ProcDungeon.Structures;

namespace ProcDungeon.Interfaces
{
    public interface IGenerationAlgorythm
    {
        public List<Rectangle> Rooms { get; }
        public DungeonGrid<Tile> Generate(DungeonGrid<Tile> canvas, DungeonGraph graph);
    }
}