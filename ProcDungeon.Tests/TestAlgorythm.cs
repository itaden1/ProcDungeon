using System;
using Xunit;
using ProcDungeon.Structures;
using ProcDungeon.Algorythms;
using Xunit.Abstractions;
using System.Collections.Generic;

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
            var map = new DungeonGrid<Tile>(10);
            var graph = new DungeonGraph();
            var n1 = new DNode(1);
            var n2 = new DNode(2);
            var n3 = new DNode(3);

            var e1 = new DEdge(n1, n2);
            var e2 = new DEdge(n2, n3);

            n1.AddEdge(e1);
            n2.AddEdge(e2);

            graph.AddNodes(new List<DNode>{n1, n2, n3});

			BSPDungeonAlgorythm alg = new BSPDungeonAlgorythm();
			DungeonGrid<Tile> tileMap = alg.Generate(map, graph);

            Assert.Equal(graph.NodeCount, alg.Rooms.Count);
        }
    }
    public class TestTRandomAlgorythm
    {
        [Fact]
        public void TestGenerate()
        {
            var map = new DungeonGrid<Tile>(100);
            var graph = new DungeonGraph();
            for (int i = 0; i <= 3; i++)
            {
                var n = new DNode(i);
                graph.AddNode(n);
            }

            RandomPlacementAlgorythm alg = new RandomPlacementAlgorythm();
			DungeonGrid<Tile> tileMap = alg.Generate(map, graph);
            Assert.Equal(graph.NodeCount, alg.Rooms.Count);
        }
    }
}