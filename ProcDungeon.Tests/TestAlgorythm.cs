using System;
using Xunit;
using ProcDungeon.Structures;
using ProcDungeon.Algorythms;
using Xunit.Abstractions;


namespace ProcDungeon.Tests
{

    public class TestBSPAlgorythm
    {
        public TestBSPAlgorythm(ITestOutputHelper output)
        {
            Converter converter = new Converter(output);
            Console.SetOut(converter);
        }

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

            Assert.Equal(graph.NodeCount, alg.Rooms.Count);
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