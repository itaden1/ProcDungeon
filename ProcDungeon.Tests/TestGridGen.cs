using System;
using Xunit;
using ProcDungeon;
using ProcDungeon.Structures;

namespace ProcDungeon.Tests
{
    public class TestDungeonGen
    {
        [Fact]
        public void TestGenerateGrid()
        {
            var graph = new DungeonGraph();
            for (int i = 0; i <= 3; i++)
            {
                var n = new DNode(i);
                graph.AddNode(n);
            }
            var map = new DungeonGrid<Tile>(50);
            Assert.Equal(map.Grid.GetLength(0), 50);
        }
    }
}