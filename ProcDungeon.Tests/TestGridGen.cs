using System;
using Xunit;
using ProcDungeon;
using ProcDungeon.Structures;

namespace ProcDungeon.Tests
{
    public class TestDungeonGen
    {
        [Fact]
        public void TestGenerateMapGrid()
        {
            var gridGen = new GridGenerator();
            var graph = new DungeonGraph();
            for (int i = 0; i <= 3; i++)
            {
                var n = new DNode(i);
                graph.AddNode(n);
            }
            Tile[,] tiles = gridGen.GenerateMapGrid(graph, 50, new char[1]);
            Assert.Equal(tiles.GetLength(0), 50);
        }
    }
}