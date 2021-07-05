using System;
using Xunit;
using ProcDungeon;
using ProcDungeon.Structures;
using ProcDungeon.Algorythms;

namespace ProcDungeon.Tests
{
    public class TestDungeonGen
    {
        [Fact]
        public void TestGenerateGrid()
        {            
            RandomPlacementAlgorythm alg = new RandomPlacementAlgorythm(5);

            var graph = new DungeonGraph();
            for (int i = 0; i <= 3; i++)
            {
                var n = new DNode(i);
                graph.AddNode(n);
            }
            var map = new DungeonGrid(50, alg);
            Assert.Equal(map.Grid.GetLength(0), 50);
        }
    }
}