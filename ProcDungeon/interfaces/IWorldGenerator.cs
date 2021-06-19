using System;
using ProcDungeon.Structures;

namespace ProcDungeon.Interfaces
{
    public interface IWorldGenerator
    {
        public void Create();
        public DungeonGraph GetGraph();
        public Tile[,] GetGridMap();
    }
}