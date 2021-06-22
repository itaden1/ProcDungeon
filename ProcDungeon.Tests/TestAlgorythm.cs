using System;
using Xunit;
using ProcDungeon;
using ProcDungeon.Structures;
using ProcDungeon.Algorythms;
using Xunit.Abstractions;
using System.Text;
using System.IO;

namespace ProcDungeon.Tests
{
    public class Converter : TextWriter
    {
        ITestOutputHelper _output;
        public Converter(ITestOutputHelper output)
        {
            _output = output;
        }
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
        public override void WriteLine(string message)
        {
            _output.WriteLine(message);
        }
        public override void WriteLine(string format, params object[] args)
        {
            _output.WriteLine(format, args);
        }
    }
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