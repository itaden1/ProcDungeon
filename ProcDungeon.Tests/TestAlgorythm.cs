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

			BSPDungeonAlgorythm alg = new BSPDungeonAlgorythm(map);
			alg.Generate(graph);

            Assert.Equal(graph.NodeCount, alg.Rooms.Count);
        }
        [Fact]
        public void TestCorridoorBetweenBSPNodes()
        {
            var map = new DungeonGrid<Tile>(3);
            var leaf1 = new BSPNode(0,1,0,3);
            var leaf2 = new BSPNode(1,2,0,3);
            BSPDungeonAlgorythm alg = new BSPDungeonAlgorythm(map);
            alg.CreateCorridoor(leaf1, leaf2);
            Console.WriteLine($"{map.Grid[0,0].Blocking}");
            Console.WriteLine($"{map.Grid[0,1].Blocking}");
            Console.WriteLine($"{map.Grid[0,2].Blocking}");
            Console.WriteLine($"{map.Grid[2,0].Blocking}");
            Console.WriteLine($"{map.Grid[2,1].Blocking}");
            Console.WriteLine($"{map.Grid[2,2].Blocking}");

            Assert.False(map.Grid[0,1].Blocking);
            Assert.False(map.Grid[1,1].Blocking);
            Assert.True(map.Grid[0,0].Blocking);
            Assert.True(map.Grid[1,0].Blocking);
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

            RandomPlacementAlgorythm alg = new RandomPlacementAlgorythm(map);
			alg.Generate(graph);
            Assert.Equal(graph.NodeCount, alg.Rooms.Count);
        }
    }
}