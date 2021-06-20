using System.Collections.Generic;
using ProcDungeon.Structures;

namespace ProcDungeon.Interfaces
{
    public interface IGenerationAlgorythm<T>
    {
        public List<Rectangle> Rooms { get; }
        public T[,] Generate<T>(T[,] canvas, DungeonGraph graph) where T : ITileNode;
    }
}