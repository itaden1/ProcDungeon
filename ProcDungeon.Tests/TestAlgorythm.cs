using System;
using Xunit;
using ProcDungeon;
using ProcDungeon.Structures;
using ProcDungeon.Algorythms;

namespace ProcDungeon.Tests
{
    public class TestBSPAlgorythm
    {
        [Fact]
        public void TestGenerate()
        {
			Tile[,] map = GridGenerator.GenerateTileGrid(10);
            var graph = new DungeonGraph();
            for (int i = 0; i <= 3; i++)
            {
                var n = new DNode(i);
                graph.AddNode(n);
            }
			BSPDungeonAlgorythm<Tile> alg = new BSPDungeonAlgorythm<Tile>();
			Tile[,] tileMap = alg.Generate<Tile>(map, graph);
        }
    }
    public class TestTRandomAlgorythm
    {
        [Fact]
        public void TestGenerate()
        {
            Tile[,] map = GridGenerator.GenerateTileGrid(100);
            var graph = new DungeonGraph();
            for (int i = 0; i <= 3; i++)
            {
                var n = new DNode(i);
                graph.AddNode(n);
            }

            RandomPlacementAlgorythm<Tile> alg = new RandomPlacementAlgorythm<Tile>();
			Tile[,] tileMap = alg.Generate<Tile>(map, graph);
            Assert.Equal(graph.NodeCount, alg.Rooms.Count);
        }
    }
}